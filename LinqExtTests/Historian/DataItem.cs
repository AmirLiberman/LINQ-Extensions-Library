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
    }

    [TestMethod()]
    public void InterpolateTest()
    {
      DataItem a = new DataItem(5.0, new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero));
      DataItem b = new DataItem(6.0, new DateTimeOffset(2010, 1, 1, 1, 0, 0, TimeSpan.Zero));
      DataItem expected = new DataItem(5.5, new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero));
      DataItem actual = DataItem.Interpolate(new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), a, b, false);
      Assert.AreEqual(expected, actual);

      expected = new DataItem(5, new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero));
      actual = DataItem.Interpolate(new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), a, b, true);
      Assert.AreEqual(expected, actual);

      a = new DataItem(5.0, new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero));
      b = new DataItem(double.NaN, new DateTimeOffset(2010, 1, 1, 1, 0, 0, TimeSpan.Zero));
      expected = new DataItem(5.0, new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), DataItemStatus.Empty);
      actual = DataItem.Interpolate(new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), a, b, false);
      Assert.AreEqual(expected, actual);

      a = new DataItem(double.NaN, new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero));
      b = new DataItem(5.0, new DateTimeOffset(2010, 1, 1, 1, 0, 0, TimeSpan.Zero));
      expected = new DataItem(double.NaN, new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), DataItemStatus.Empty);
      actual = DataItem.Interpolate(new DateTimeOffset(2010, 1, 1, 0, 30, 0, TimeSpan.Zero), a, b, false);
      Assert.AreEqual(expected, actual);
    }
  }
}
