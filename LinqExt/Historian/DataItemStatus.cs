#pragma warning disable CA1714 // Flags enums should have plural names

using System;

namespace LinqExtensions.Historian
{
  [Flags]

  public enum DataItemStatus : int
  {
    //Acceptable Codes
    /// <summary>
    /// Good Data - no flags.
    /// </summary>
    OK = 0x0000,                                                                                    // 0000 0000 0000 0000  0000 0000 0000 0000    

    //Provider issues
    /// <summary>
    ///Error getting value. 
    /// </summary>
    Error = 0x0010,                                                                                 // 0000 0000 0000 0000  0000 0000 0001 0000    

    /// <summary>
    /// No value found at the exact timestamp.
    /// </summary>
    TimeStampNotFound = 0x0020,                                                                     // 0000 0000 0000 0000  0000 0000 0010 0000    

    /// <summary>
    /// No value found at or before the timestamp (timestamp is before creation timestamp).
    /// </summary>
    TimeStampBeforeRangeStart = 0x0040,                                                             // 0000 0000 0000 0000  0000 0000 0100 0000    

    /// <summary>
    /// No value found at or before the timestamp (timestamp is after the last point timestamp)
    /// </summary>
    TimeStampAfterRangeEnd = 0x0080,                                                                // 0000 0000 0000 0000  0000 0000 1000 0000    

    /// <summary>
    /// No value found 
    /// </summary>
    SeriesHasNoValues = 0x00C0,                                                                     // 0000 0000 0000 0000  0000 0000 1100 0000    
       
    Empty = 0x4000_0000,                                                                            // 0100 0000 0000 0000  0000 0000 0000 0000    
  }
}
