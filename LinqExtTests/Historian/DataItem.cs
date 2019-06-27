using LinqExtensions.Historian;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LinqExtTests.Historian
{
  [TestClass()]
  public class DataItemTests
  {
    [TestMethod()]
    public void EqualTest()
    {
      DateTimeOffset ts = DateTimeOffset.Now;
      DataItem a = new DataItem(3.63, ts);
      DataItem b = new DataItem(3.63, ts);

      Assert.IsTrue(a == b);
      Assert.IsTrue(a.Equals(b));
    }

    [TestMethod()]
    public void EmptyTest()
    {
      DateTimeOffset ts = DateTimeOffset.Now;
      DataItem a = new DataItem(double.NaN, ts);
      DataItem b = DataItem.Empty(ts);

      Assert.IsTrue(a == b);
      Assert.IsTrue(a.Equals(b));

      b = new DataItem(double.NaN, ts.UtcTicks);
      Assert.IsTrue(a == b);

      DateTimeOffset ts1 = DateTimeOffset.Now;
      DataItem c = DataItem.Empty();
      DateTimeOffset ts2 = DateTimeOffset.Now;

      Assert.IsTrue(double.IsNaN(c.Value));
      Assert.IsTrue(c.Timestamp >= ts1 && c.Timestamp <= ts2);

      ts = DateTimeOffset.Now;
      a = new DataItem(double.NaN, ts, DataItemStatus.Error);
      b = DataItem.Empty(ts, DataItemStatus.Error);

      Assert.IsTrue(a == b);
      Assert.IsTrue(a.HasStatus(DataItemStatus.Error));

      Assert.IsTrue(a.IsEmpty);
      Assert.IsFalse(a.IsGood);

    }

    [TestMethod()]
    public void DateItemStringOutputTest()
    {
      DataItem a = new DataItem(double.NaN, new DateTimeOffset(2018, 1, 1, 11, 51, 32, TimeSpan.FromHours(-5)));
      Assert.IsTrue(a.ToString() == "1/1/2018 4:51:32 PM +00:00, NaN, Empty-0, NotVerified");
      Assert.IsTrue(a.ToCsv() == "1/1/2018 4:51:32 PM +00:00,NaN,NaN,Empty,0,NotVerified");

      a = new DataItem(Math.PI, new DateTimeOffset(2018, 1, 1, 11, 51, 32, TimeSpan.FromHours(-5)));
      Assert.IsTrue(a.ToCsv() == "1/1/2018 4:51:32 PM +00:00,3.14159265358979,3.14159265358979,OK,0,NotVerified");
    }

    [TestMethod()]
    public void InterpolateTest()
    {
      DataItem a = new DataItem(5.0, new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero));
      DataItem b = new DataItem(6.0, new DateTimeOffset(2010, 1, 1, 1, 0, 0, TimeSpan.Zero));
      DataItem expected = new DataItem(5.5, new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero));
      DataItem actual = DataItem.Interpolate(new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), a, b, false);
      Assert.AreEqual(expected, actual);

      actual = DataItem.Interpolate(new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), a, b, true);
      expected = new DataItem(5, new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero));
      Assert.AreEqual(expected, actual);

      a = new DataItem(5.0, new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero));
      b = new DataItem(double.NaN, new DateTimeOffset(2010, 1, 1, 1, 0, 0, TimeSpan.Zero));
      actual = DataItem.Interpolate(new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), a, b, false);
      expected = new DataItem(5.0, new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), DataItemStatus.Empty);
      Assert.AreEqual(expected, actual);

      a = new DataItem(double.NaN, new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero));
      b = new DataItem(5.0, new DateTimeOffset(2010, 1, 1, 1, 0, 0, TimeSpan.Zero));

      actual = DataItem.Interpolate(new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), a, b, false);
      expected = new DataItem(double.NaN, new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), DataItemStatus.Empty);
      Assert.AreEqual(expected, actual);

      actual = DataItem.Interpolate(new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), b, a, false);
      expected = new DataItem(double.NaN, new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), DataItemStatus.Empty);
      Assert.AreEqual(expected, actual);

      actual = DataItem.Interpolate(new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero), b, a, false);
      expected = a;
      Assert.AreEqual(expected, actual);

      actual = DataItem.Interpolate(new DateTimeOffset(2010, 1, 1, 1, 0, 0, TimeSpan.Zero), b, a, false);
      expected = b;
      Assert.AreEqual(expected, actual);

      actual = DataItem.Interpolate(new DateTimeOffset(2010, 12, 1, 1, 0, 0, TimeSpan.Zero), b, a, false);
      Assert.IsTrue(actual.HasStatus(DataItemStatus.TimeStampAfterRangeEnd));

      actual = DataItem.Interpolate(new DateTimeOffset(2009, 12, 1, 1, 0, 0, TimeSpan.Zero), b, a, false);
      Assert.IsTrue(actual.HasStatus(DataItemStatus.TimeStampBeforeRangeStart));
    }

    [TestMethod()]
    public void MathTest()
    {
      DateTimeOffset date = new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero);
      DataItem left = new DataItem(5.0, date);
      DataItem right = new DataItem(2.0, date);

      DataItem actual;
      DataItem expected;

      actual = DataItem.Add(left, right);
      expected = new DataItem(7, date);
      Assert.AreEqual(expected, actual);

      actual = DataItem.Subtract(left, right);
      expected = new DataItem(3, date);
      Assert.AreEqual(expected, actual);

      actual = DataItem.Multiply(left, right);
      expected = new DataItem(10, date);
      Assert.AreEqual(expected, actual);

      actual = DataItem.Divide(left, right);
      expected = new DataItem(2.5, date);
      Assert.AreEqual(expected, actual);

      actual = DataItem.Negate(left + 2);
      expected = new DataItem(-7, date);
      Assert.AreEqual(expected, actual);

      actual = DataItem.Negate(left - 2);
      expected = new DataItem(-3, date);
      Assert.AreEqual(expected, actual);

      actual = DataItem.Negate(left * 2);
      expected = new DataItem(-10, date);
      Assert.AreEqual(expected, actual);

      actual = DataItem.Negate(left / 2);
      expected = new DataItem(-2.5, date);
      Assert.AreEqual(expected, actual);
    }
  }
}
