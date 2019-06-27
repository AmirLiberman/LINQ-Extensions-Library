using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LinqExtensions.Historian
{
  [DebuggerDisplay("{Timestamp}, {Value}, {Status} - {Code}, {Confidence}")]
  [StructLayout(LayoutKind.Sequential)]
  public struct DataItem : IEquatable<DataItem>
  {
    private const int CODE_BIT_MASK = 0x0000_FFFF;
    private const int CONFIDENCE_BIT_MASK = 0x000F_0000;

    #region Declarations

#pragma warning disable IDE0032 // Use auto property

    private readonly double _originalValue;               //        8            

    private readonly DataItemStatus _status;              //        4 
    private int _bitData;                                 //        4 
    private long _utcTimestampTicks;                      //        8

#pragma warning restore IDE0032 // Use auto property

    #endregion

    #region Constructors

    public DataItem(int value, DateTimeOffset timestamp)
      : this(value, timestamp, DataItemStatus.OK) { }

    public DataItem(double value, DateTimeOffset timestamp)
      : this(value, timestamp.UtcTicks) { }

    public DataItem(double value, long timestampTicks)
      : this(value, timestampTicks, double.IsNaN(value) ? DataItemStatus.Empty : DataItemStatus.OK) { }

    public DataItem(double value, DateTimeOffset timestamp, DataItemStatus status)
      : this(value, timestamp.UtcTicks, status) { }

    public DataItem(double value, long timestampTicks, DataItemStatus status)
      : this(value, timestampTicks, status, 0, Quality.NotVerified) { }

    public DataItem(double value, DateTimeOffset timestamp, DataItemStatus status, int code)
      : this(value, timestamp.UtcTicks, status, code) { }

    public DataItem(double value, long timestampTicks, DataItemStatus status, int code)
      : this(value, timestampTicks, status, code, Quality.NotVerified) { }


    public DataItem(double value, DateTimeOffset timestamp, DataItemStatus status, int code, Quality confidence)
    : this(value, timestamp.UtcTicks, status, code, confidence) { }

    public DataItem(double value, long timestampTicks, DataItemStatus status, int code, Quality confidence)
    : this(value, timestampTicks, code | (((int)confidence & 0xF) << 16), status) { }

    private DataItem(double value, long timestampTicks, int bitData, DataItemStatus status)
    {
      _originalValue = value;
      _utcTimestampTicks = timestampTicks;
      _bitData = bitData;
      if (double.IsNaN(value))
        _status = status | DataItemStatus.Empty;
      else
        _status = status;
    }

    #endregion

    #region Properties

    public double Value
    {
      get
      {
        if (IsGood)
          return _originalValue;
        return double.NaN;
      }
    }

    public double OriginalValue => _originalValue;

    public DateTimeOffset Timestamp
    {
      get { return new DateTimeOffset(_utcTimestampTicks, TimeSpan.Zero); }
    }

    public long UtcTicks => _utcTimestampTicks;

    public DataItemStatus Status => _status;

    public int Code
    {
      get => _bitData & CODE_BIT_MASK;
      //private set => _bitData = (_bitData & ~CODE_BIT_MASK) | value;
    }

    public Quality Confidence
    {
      get => (Quality)((_bitData & CONFIDENCE_BIT_MASK) >> 16);
      //set => _bitData = (_bitData & ~CONFIDENCE_BIT_MASK) | (((int)value & 0xF) << 16);
    }


    public bool IsGood => !double.IsNaN(_originalValue);

    public bool IsEmpty => (_status & DataItemStatus.Empty) == DataItemStatus.Empty;

    #endregion

    #region Create empty item methods (static)

    public static DataItem Empty(long timestampTicks, DataItemStatus status)
    {
      return new DataItem(double.NaN, timestampTicks, status | DataItemStatus.Empty);
    }

    public static DataItem Empty(DateTimeOffset timestamp, DataItemStatus status)
    {
      return new DataItem(double.NaN, timestamp, status | DataItemStatus.Empty);
    }

    public static DataItem Empty(DateTimeOffset timestamp)
    {
      return new DataItem(double.NaN, timestamp, DataItemStatus.Empty);
    }

    public static DataItem Empty()
    {
      return Empty(DateTime.Now);
    }

    #endregion

    #region  Equal and other operators overrides

    public override bool Equals(object obj)
    {
      if (obj == null || GetType() != obj.GetType())
        return false;

      return Equals((DataItem)obj);
    }

    public override int GetHashCode()
    {
      unchecked // Overflow is fine, just wrap
      {
        int hash = Utilities.HASH_PRIME_1;
        hash = hash * Utilities.HASH_PRIME_2 + _originalValue.GetHashCode();
        hash = hash * Utilities.HASH_PRIME_2 + (_utcTimestampTicks).GetHashCode();
        hash = hash * Utilities.HASH_PRIME_2 + _status.GetHashCode();
        hash = hash * Utilities.HASH_PRIME_2 + _bitData.GetHashCode();
        return hash;
      }
    }

    public bool Equals(DataItem other)
    {
      return Equals(_originalValue, other._originalValue) && (_status == other._status) && (_bitData == other._bitData) && (_utcTimestampTicks == other._utcTimestampTicks);
    }

    public static bool operator ==(DataItem left, DataItem right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(DataItem left, DataItem right)
    {
      return !(left == right);
    }

    public static DataItem Add(DataItem left, DataItem right)
    {
      return left + right;
    }

    public static DataItem Subtract(DataItem left, DataItem right)
    {
      return left - right;
    }

    public static DataItem Multiply(DataItem left, DataItem right)
    {
      return left * right;
    }

    public static DataItem Divide(DataItem left, DataItem right)
    {
      return left / right;
    }

    public static DataItem Negate(DataItem item)
    {
      return -item;
    }

    public static DataItem operator +(DataItem left, DataItem right)
    {
      Utilities.ValidateIsNotNull(left, nameof(left));
      Utilities.ValidateIsNotNull(right, nameof(right));

      return Combine(left._originalValue + right._originalValue, left.Timestamp, left, right);
    }

    public static DataItem operator -(DataItem left, DataItem right)
    {
      Utilities.ValidateIsNotNull(left, nameof(left));
      Utilities.ValidateIsNotNull(right, nameof(right));

      return Combine(left._originalValue - right._originalValue, left.Timestamp, left, right);
    }

    public static DataItem operator *(DataItem left, DataItem right)
    {
      Utilities.ValidateIsNotNull(left, nameof(left));
      Utilities.ValidateIsNotNull(right, nameof(right));

      return Combine(left._originalValue * right._originalValue, left.Timestamp, left, right);
    }

    public static DataItem operator /(DataItem left, DataItem right)
    {
      Utilities.ValidateIsNotNull(left, nameof(left));
      Utilities.ValidateIsNotNull(right, nameof(right));

      return Combine(left._originalValue / right._originalValue, left.Timestamp, left, right);
    }

    public static DataItem operator -(DataItem item)
    {
      Utilities.ValidateIsNotNull(item, nameof(item));

      return new DataItem(-item._originalValue, item.Timestamp.UtcTicks, item._bitData, item._status);
    }


    public static DataItem operator +(DataItem item, double value)
    {
      Utilities.ValidateIsNotNull(item, nameof(item));
      return new DataItem(item._originalValue + value, item.Timestamp.UtcTicks, item._bitData, item._status);
    }

    public static DataItem operator -(DataItem item, double value)
    {
      Utilities.ValidateIsNotNull(item, nameof(item));
      return new DataItem(item._originalValue - value, item.Timestamp.UtcTicks, item._bitData, item._status);
    }

    public static DataItem operator *(DataItem item, double value)
    {
      Utilities.ValidateIsNotNull(item, nameof(item));
      return new DataItem(item._originalValue * value, item.Timestamp.UtcTicks, item._bitData, item._status);
    }

    public static DataItem operator /(DataItem item, double value)
    {
      Utilities.ValidateIsNotNull(item, nameof(item));
      return new DataItem(item._originalValue / value, item.Timestamp.UtcTicks, item._bitData, item._status);
    }

    public override string ToString()
    {
      return $"{Timestamp}, {Value}, {_status}-{Code}, {Confidence}";
    }

    #endregion

    #region Item interpolation methods (static)

    public static DataItem Interpolate(DateTimeOffset timestamp, DataItem item1, DataItem item2, bool stepInterpolation)
    {

      if (item2.UtcTicks < item1.UtcTicks)
      {
        DataItem temp = item1;
        item1 = item2;
        item2 = temp;
      }

      //timestamp is the same as item1 timestamp, return it.
      if (item1.UtcTicks == timestamp.UtcTicks)
        return item1;

      //timestamp is the same as item2 timestamp, return it.
      if (item2.UtcTicks == timestamp.UtcTicks)
        return item2;

      //timestamp is before item1 timestamp, return an  empty item with the right status.
      if (item1.UtcTicks > timestamp.UtcTicks)
        return DataItem.Empty(timestamp, DataItemStatus.TimeStampBeforeRangeStart);

      //timestamp is after item2 timestamp, return an  empty item with the right status.
      if (item2.UtcTicks < timestamp.UtcTicks)
        return DataItem.Empty(timestamp, DataItemStatus.TimeStampAfterRangeEnd);


      if (stepInterpolation)
        return new DataItem(item1.OriginalValue, timestamp, item1.Status, item1.Code, item1.Confidence);

      //If at this point, timestamp is in between the items timestamps
      if (!item1.IsGood || !item2.IsGood)
        return Combine(item1.OriginalValue, timestamp, item1, item2);

      //if (!item2.IsGood)
      //  return Combine(item1.OriginalValue, timestamp, item2, item2);

      //Timestamp is in between the items timestamps, both items are good and linear interpolation is needed (stepInterpolation is false)
      //Preform the interpolation:
      double interpolationFactor = (item2.Value - item1.Value) / item2.Timestamp.Subtract(item1.Timestamp).TotalMinutes;
      double interpolatedValue = item1.Value + interpolationFactor * timestamp.Subtract(item1.Timestamp).TotalMinutes;
      return new DataItem(interpolatedValue, timestamp, item1._status | item2._status, CombineCode(item1, item2), MinConfidance(item1, item2));
    }

    #endregion

    #region Private methods

    private static int CombineCode(DataItem item1, DataItem item2)
    {
      return item1.Code != 0 ? item1.Code : item2.Code;
    }

    private static Quality MinConfidance(DataItem item1, DataItem item2)
    {
      return item1.Confidence < item2.Confidence ? item1.Confidence : item2.Confidence;
    }

    private static DataItem Combine(double value, DateTimeOffset timestamp, DataItem item1, DataItem item2)
    {
      return new DataItem(value, timestamp, item1._status | item2._status, CombineCode(item1, item2), MinConfidance(item1, item2));
    }

    #endregion

    public string ToCsv()
    {
      return $"{Timestamp},{Value},{_originalValue},{_status},{Code},{Confidence}";
    }

    public bool HasStatus(DataItemStatus status)
    {
      return (_status & status) == status;
    }
  }
}