using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqExtensions.Historian
{
  public static class DataItemsProcessor
  {
    public static IEnumerable<DataItem> ProcessExceptions(this IEnumerable<DataItem> source)
    {
      return source.ProcessExceptions(0);
    }

    public static IEnumerable<DataItem> ProcessExceptions(this IEnumerable<DataItem> source, double exceptionMax)
    {
      DataItem archive;
      DataItem lastSnapshot;
      DataItem currentSnapshot;


      using (IEnumerator<DataItem> rator = source.GetEnumerator())
        if (rator.MoveNext()) //Check if stream has data
        {
          archive =
            lastSnapshot =
            currentSnapshot = rator.Current;

          //Always return the first item.
          yield return archive;

          //Process the rest of the stream
          while (rator.MoveNext())
          {
            currentSnapshot = rator.Current;

            if ((archive.IsGood && currentSnapshot.IsGood && archive.OriginalValue + exceptionMax >= currentSnapshot.OriginalValue && archive.OriginalValue - exceptionMax <= currentSnapshot.OriginalValue) || archive.Equals(currentSnapshot))
              lastSnapshot = currentSnapshot; // replace the last and reduce set size
            else
            {
              // Value is out of compression range or bad, yield it.
              if (lastSnapshot != archive)
                yield return lastSnapshot;
              yield return currentSnapshot;
              archive =
                lastSnapshot = currentSnapshot;
            }
          }
        }
        else
          yield break;

      //Yield the last item if it was not yet provided
      if (currentSnapshot != archive)
        yield return currentSnapshot;
    }

    public static IEnumerable<DataItem> ProcessCompression(this IEnumerable<DataItem> source)
    {
      return source.ProcessCompression(0);
    }

    public static IEnumerable<DataItem> ProcessCompression(this IEnumerable<DataItem> source, double compressionMax)
    {
      DataItem archive;
      DataItem lastSnapshot;
      DataItem currentSnapshot;

      double slope1 = double.MaxValue;
      double slope2 = double.MinValue;

      using (IEnumerator<DataItem> rator = source.GetEnumerator())
        if (rator.MoveNext())
        {
          archive =
            lastSnapshot =
            currentSnapshot = rator.Current;
          yield return archive;

          while (rator.MoveNext())
          {
            currentSnapshot = rator.Current;

            if (currentSnapshot.IsGood)
            {
            repeatCalc:
              long dur = (currentSnapshot.UtcTicks - archive.UtcTicks) / TimeSpan.TicksPerSecond;
              if (dur == 0)
                continue;
              if (archive.IsGood)
              {
                double currentSlope = (currentSnapshot.OriginalValue - archive.OriginalValue) / dur;
                if (currentSlope < slope1 && currentSlope > slope2)
                {
                  double tempSlope1 = (currentSnapshot.OriginalValue - (archive.OriginalValue - compressionMax)) / dur;
                  double tempSlope2 = (currentSnapshot.OriginalValue - (archive.OriginalValue + compressionMax)) / dur;

                  if (slope1 > tempSlope1)
                    slope1 = tempSlope1;
                  if (slope2 < tempSlope2)
                    slope2 = tempSlope2;

                  lastSnapshot = currentSnapshot;
                  continue;
                }
              }

              // Value is out of compression range or bad, yield it.
              archive = lastSnapshot;
              yield return archive;

              slope1 = double.MaxValue;
              slope2 = double.MinValue;

              goto repeatCalc;
            }
          }
          if (currentSnapshot != archive)
            yield return currentSnapshot;
        }
    }

    public static IEnumerable<DataItem> Interpolate(this IEnumerable<DataItem> source, DateTimeOffset start, DateTimeOffset end, TimeSpan interval)
    {
      return Interpolate(source, start, end, interval, false);
    }

    public static IEnumerable<DataItem> Interpolate(this IEnumerable<DataItem> source, DateTimeOffset start, DateTimeOffset end, TimeSpan interval, bool autoAlignDates)
    {
      if (autoAlignDates)
      {
        start = start.AlignDate(interval);
        end = end.AlignDate(interval);
      }

      DataItem[] data = GetItemsSubset(source, start, end).ToArray();
      int dataItems = data.Length;

      if (dataItems <= 1)
        yield break;

      int interploatedItems = (int)((double)(end.Subtract(start).Ticks) / interval.Ticks) + 1;

      long interpolatedTimestamp = start.UtcTicks;
      int dataIndex = 0;
      int interpolatedIndex = 0;
      int lastDataIndex = dataItems - 2;

      while (dataIndex <= lastDataIndex)
      {
        while (data[dataIndex].UtcTicks <= interpolatedTimestamp && data[dataIndex + 1].UtcTicks >= interpolatedTimestamp)
        {
          yield return DataItem.Interpolate(new DateTimeOffset(interpolatedTimestamp, TimeSpan.Zero), data[dataIndex], data[dataIndex + 1], false);
          interpolatedIndex++;
          if (interpolatedIndex == interploatedItems)
            yield break;

          interpolatedTimestamp = interpolatedTimestamp + interval.Ticks;
        }
        dataIndex++;
      }
    }

    private static IEnumerable<DataItem> GetItemsSubset(IEnumerable<DataItem> source, DateTimeOffset start, DateTimeOffset end)
    {
      DataItem prev = DataItem.Empty(start, DataItemStatus.TimeStampBeforeRangeStart);
      DataItem current = DataItem.Empty(end, DataItemStatus.TimeStampAfterRangeEnd);
      using (IEnumerator<DataItem> rator = source.GetEnumerator())
      {
        while (rator.MoveNext())
        {
          current = rator.Current;
          if (current.UtcTicks == start.UtcTicks)
          {
            yield return current;
            break;
          }
          else if (current.UtcTicks > start.UtcTicks)
          {
            yield return prev;
            yield return current;
            break;
          }
          else
            prev = current;
        }

        while (rator.MoveNext())
        {
          current = rator.Current;
          if (current.Timestamp < end)
            yield return current;
          else
          {
            yield return current;
            break;
          }
        }
      }
      if (current.Timestamp < end)
        yield return DataItem.Empty(end, DataItemStatus.TimeStampAfterRangeEnd);
    }
  }
}