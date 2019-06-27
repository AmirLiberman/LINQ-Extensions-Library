using LinqExtensions.Historian;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqExtTests.Historian
{
  [TestClass()]
  public class DataItemsProcessorTest
  {
    private static IEnumerable<DataItem> GetTestData()
    {
      DateTimeOffset ts = new System.DateTimeOffset(2019, 1, 1, 10, 30, 0, TimeSpan.Zero);
      //0
      yield return new DataItem(19.0, ts);
      ts = ts.AddMinutes(1);
      //1
      yield return new DataItem(10.0, ts);
      ts = ts.AddMinutes(1);
      //2
      yield return new DataItem(10.0, ts);
      ts = ts.AddMinutes(1);
      //3
      yield return new DataItem(10.0, ts);
      ts = ts.AddMinutes(1);
      //4
      yield return new DataItem(12.0, ts);
      ts = ts.AddMinutes(1);
      //5
      yield return new DataItem(10.0, ts);
      ts = ts.AddMinutes(1);
      //6
      yield return new DataItem(10.0, ts);
      ts = ts.AddMinutes(1);
      //7
      yield return new DataItem(10.0, ts);
      ts = ts.AddMinutes(1);
      //8
      yield return new DataItem(10.0, ts);
      ts = ts.AddMinutes(1);
      //9
      yield return new DataItem(10.0, ts);
      ts = ts.AddMinutes(1);
      //10
      yield return new DataItem(10.0, ts);
      ts = ts.AddMinutes(1);
      //11
      yield return new DataItem(11.0, ts);
      ts = ts.AddMinutes(1);
      //12
      yield return new DataItem(12.0, ts);
      ts = ts.AddMinutes(1);
      //13
      yield return new DataItem(13.0, ts);
      ts = ts.AddMinutes(1);
      //14
      yield return new DataItem(14.0, ts);
      ts = ts.AddMinutes(1);
      //15
      yield return new DataItem(15.0, ts);
      ts = ts.AddMinutes(1);
      //16
      yield return new DataItem(15.0, ts);
      ts = ts.AddMinutes(1);
      //17
      yield return new DataItem(15.0, ts);
      ts = ts.AddMinutes(1);
      //18
      yield return new DataItem(14.0, ts);
    }

    [TestMethod()]
    public void ProcessExceptions1()
    {
      DataItem[] input = GetTestData().ToArray();
      DataItem[] res = input.ProcessExceptions().ToArray();

      Assert.IsTrue(res.Length == 13);

      Assert.IsTrue(res[0] == input[0]);
      Assert.IsTrue(res[1] == input[1]);
      Assert.IsTrue(res[2] == input[3]);
      Assert.IsTrue(res[3] == input[4]);
      Assert.IsTrue(res[4] == input[5]);
      Assert.IsTrue(res[5] == input[10]);
      Assert.IsTrue(res[6] == input[11]);
      Assert.IsTrue(res[7] == input[12]);
      Assert.IsTrue(res[8] == input[13]);
      Assert.IsTrue(res[9] == input[14]);
      Assert.IsTrue(res[10] == input[15]);
      Assert.IsTrue(res[11] == input[17]);
      Assert.IsTrue(res[12] == input[18]);
    }

    [TestMethod()]
    public void ProcessExceptions2()
    {
      DataItem[] input = GetTestData().ToArray();
      DataItem[] res = input.ProcessExceptions(1.25).ToArray();

      Assert.IsTrue(res.Length == 10);

      Assert.IsTrue(res[0] == input[0]);
      Assert.IsTrue(res[1] == input[1]);
      Assert.IsTrue(res[2] == input[3]);
      Assert.IsTrue(res[3] == input[4]);
      Assert.IsTrue(res[4] == input[5]);
      Assert.IsTrue(res[5] == input[11]);
      Assert.IsTrue(res[6] == input[12]);
      Assert.IsTrue(res[7] == input[13]);
      Assert.IsTrue(res[8] == input[14]);
      Assert.IsTrue(res[9] == input[18]);
    }

    [TestMethod()]
    public void ProcessCompression1()
    {
      DataItem[] input = GetTestData().ToArray();
      DataItem[] res = input.ProcessCompression().ToArray();

      Assert.IsTrue(res.Length == 9);

      Assert.IsTrue(res[0] == input[0]);
      Assert.IsTrue(res[1] == input[1]);
      Assert.IsTrue(res[2] == input[3]);
      Assert.IsTrue(res[3] == input[4]);
      Assert.IsTrue(res[4] == input[5]);
      Assert.IsTrue(res[5] == input[10]);
      Assert.IsTrue(res[6] == input[15]);
      Assert.IsTrue(res[7] == input[17]);
      Assert.IsTrue(res[8] == input[18]);
    }

    [TestMethod()]
    public void ProcessCompression2()
    {
      DataItem[] input = GetTestData().ToArray();
      DataItem[] res = input.ProcessCompression(1.5).ToArray();

      Assert.IsTrue(res.Length == 6);

      Assert.IsTrue(res[0] == input[0]);
      Assert.IsTrue(res[1] == input[1]);
      Assert.IsTrue(res[2] == input[4]);
      Assert.IsTrue(res[3] == input[8]);
      Assert.IsTrue(res[4] == input[17]);
      Assert.IsTrue(res[5] == input[18]);
    }


    [TestMethod()]
    public void InterpolateTest()
    {
      DateTimeOffset start = new System.DateTimeOffset(2019, 1, 1, 10, 20, 0, TimeSpan.Zero);
      DateTimeOffset end = new System.DateTimeOffset(2019, 1, 1, 10, 55, 0, TimeSpan.Zero);

      InterpolateTest(start, end, false);

      start = start.AddSeconds(-4);
      end = end.AddSeconds(14);

      InterpolateTest(start, end, true);
    }

    private static void InterpolateTest(DateTimeOffset start, DateTimeOffset end, bool autoAlign)
    {
      DataItem[] testData = GetTestData().ToArray();

      DataItem[] res = testData.Interpolate(start, end, TimeSpan.FromSeconds(30), false, autoAlign).ToArray();

      Assert.IsTrue(res.Length == 71);

      for (int i = 1; i < res.Length; i++)
        Assert.IsTrue(res[i].Timestamp == res[i - 1].Timestamp.AddSeconds(30));

      HashSet<DataItem> testUnique = new HashSet<DataItem>(res);
      Assert.IsTrue(res.Length == testUnique.Count);

      for (int i = 0; i < res.Length; i++)
      {        
        if (i < 20)
          Assert.IsTrue(res[i].HasStatus(DataItemStatus.TimeStampBeforeRangeStart));
        else if (i > 56)
          Assert.IsTrue(res[i].HasStatus(DataItemStatus.TimeStampAfterRangeEnd));
        else if (i % 2 == 0)
          Assert.IsTrue(res[i] == testData[(i - 19) / 2]);
        else
          switch (i)
          {
            case 21:
              Assert.IsTrue(res[i].Value == 14.5);
              break;
            case 23:
            case 25:
            case 31:
            case 33:
            case 35:
            case 37:
            case 39:
              Assert.IsTrue(res[i].Value == 10);
              break;
            case 27:
            case 29:
              Assert.IsTrue(res[i].Value == 11);
              break;
            case 41:
              Assert.IsTrue(res[i].Value == 10.5);
              break;
            case 43:
              Assert.IsTrue(res[i].Value == 11.5);
              break;
            case 45:
              Assert.IsTrue(res[i].Value == 12.5);
              break;
            case 47:
              Assert.IsTrue(res[i].Value == 13.5);
              break;
            case 49:
            case 55:
              Assert.IsTrue(res[i].Value == 14.5);
              break;
            case 51:
            case 53:
              Assert.IsTrue(res[i].Value == 15);
              break;
            default:
              Assert.Fail();
              break;
          }
      }
    }
  }
}
