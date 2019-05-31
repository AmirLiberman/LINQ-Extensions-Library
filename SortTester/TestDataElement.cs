using System;

namespace SortTester
{
  public class TestDataElement
  {
    public string Property1 { get; set; }
    public int Property2 { get; set; }
    public DateTime Property3 { get; set; }
    public decimal Property4 { get; set; }
    public int Index { get; set; }

    public TestDataElement(int index)
    {
      Guid guid = Guid.NewGuid();

      Property1 = guid.ToString();
      Property2 = guid.GetHashCode();
      Property3 = DateTime.Now.AddTicks(Property2);
      Property4 = (decimal)Property2 + 1 / (decimal)Property2;

      Index = index;
    }
  }  
}
