using System.Collections.Generic;

namespace LinqExtTests
{
  public class TestComparer : IComparer<int>
  {
    public int Compare(int x, int y)
    {
      return x.CompareTo(y);
    }
  }
}
