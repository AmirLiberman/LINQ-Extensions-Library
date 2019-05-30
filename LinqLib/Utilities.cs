using System;
using System.Collections.Generic;
using System.Linq;
using LinqExtensions.Array;

namespace LinqExtensions
{
  internal static class Utilities
  {
    // Static hash set that holds primitive numeric types for fast lookup.   
    private static readonly HashSet<string> numericTypes = new HashSet<string> {"Byte",
                                                                                "Int16",
                                                                                "Int32",
                                                                                "Int64",
                                                                                "SByte",
                                                                                "UInt16",
                                                                                "UInt32",
                                                                                "UInt64",
                                                                                "Decimal",
                                                                                "Double",
                                                                                "Single"};

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    internal static void ValidateIsNumeric<T>(string caller)
    {
      Type tt = typeof(T);
      if (!IsNumeric(tt))
        throw Exceptions.ApplyAttempt(caller, tt.Name);
    }

    internal static void ValidateIsNotNull(object value, string name)
    {
      if (value == null)
        throw Exceptions.ArgumentNull(name);
    }

    internal static void ValidateSquareAngle(int angle)
    {
      if (angle % 90 != 0)
        throw Exceptions.InvalidAngle(angle);
    }

    internal static bool IsNumeric(Type value)
    {
      string name = value.Name;
      if (value.IsValueType && numericTypes.Contains(name)) // If type is primitive and name is found in lookup table
        return true;

      if (name == "Nullable`1") // If type is nullable<T> and T is numeric.
        return IsNumeric(value.GetGenericArguments()[0]);

      return false;
    }

    internal static void ValidateEnumeratedParamIsNotNull<T>(IEnumerable<T> parameter, int count, string parameterName)
    {
      if (parameter == null)
        throw Exceptions.ArgumentNull(parameterName);

      T[] paramArr = parameter.ToArray();
      if (paramArr.Count() != count)
        throw Exceptions.InvalidItemsCount(count, parameterName);

      if (typeof(T).IsValueType)
        return;

      int idx = 0;
      foreach (T element in paramArr)
      {
        if (element == null)
          throw Exceptions.SourceSequenceIsNull(idx, parameterName);

        idx++;
      }
    }

    internal static void ValidateIsPremitive<T>(string caller)
    {
      Type tt = typeof(T);
      if (!tt.IsPrimitive)
        throw Exceptions.ApplyAttempt(caller, tt.Name);
    }

    internal static void ValidateBlockSizeTwoOrMore(int size)
    {
      if (size < 2)
        throw Exceptions.ValueMinTwo("blockSize");
    }

    internal static void ValidateValueIsInRange(double value, double min, double max, string parameterName)
    {
      if (value < min)
        throw Exceptions.ParamBelowMinValue(parameterName, min);
      if (value > max)
        throw Exceptions.ParamAboveMaxValue(parameterName, max);
    }

    internal static void ValidateSequenceIsNotEmpty<T>(IEnumerable<T> parameter, string parameterName)
    {
      ValidateIsNotNull(parameter, nameof(parameter));

      if (!parameter.Any())
        throw Exceptions.SequenceIsEmpty(parameterName);
    }

    internal static void Validate2DFlip(FlipAxis axis)
    {
      int axisValue = (int)axis;
      if (axisValue < 0 || axisValue > 3)
        throw Exceptions.Invalid2DFlip(axis);
    }

    internal static void Validate3DFlip(FlipAxis axis)
    {
      int axisValue = (int)axis;
      if (axisValue < 0 || axisValue > 7)
        throw Exceptions.Invalid3DFlip(axis);
    }

    internal static void Validate3DRotation(RotateAxis axis)
    {
      if (axis != RotateAxis.RotateNone && axis != RotateAxis.RotateX && axis != RotateAxis.RotateY && axis != RotateAxis.RotateZ)
        throw Exceptions.Invalid3DRotaion(axis);
    }

    internal static void Validate4DRotation(RotateAxis axis)
    {
      if (axis != RotateAxis.RotateNone && axis != RotateAxis.RotateX && axis != RotateAxis.RotateY && axis != RotateAxis.RotateZ && axis != RotateAxis.RotateA)
        throw Exceptions.Invalid4DRotaion(axis);
    }

    internal static void ValidateTimespan(TimeSpan timespan, string parameterName)
    {
      if (timespan == TimeSpan.Zero)
        throw Exceptions.DurationIsZero(parameterName);
    }
  
    internal static void Swap<T>(ref T a, ref T b)
    {
      T temp = b;
      b = a;
      a = temp;
    }

  }
}






/// <summary>
/// Evaluates if a type is of numeric type or of nullable numeric type. 
/// </summary>
/// <param name="value">A System.Type to evaluate.</param>
///// <returns>true if type is numeric or generic nullable of numeric type, false otherwise.</returns>
//internal static bool IsNumeric(Type value)
//{
//  string name = value.Name;
//  if (value.IsValueType && numericTypes.Contains(name)) // If type is primitive and name is found in lookup table
//    return true;

//  if (name == "Nullable`1") // If type is nullable<T> and T is numeric.
//    return IsNumeric(value.GetGenericArguments()[0]);

//  return false;
//}