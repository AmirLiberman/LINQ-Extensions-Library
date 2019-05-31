using System;
using LinqExtensions.Properties;

namespace LinqExtensions
{
  internal static class Exceptions
  {
    internal static ArgumentNullException ArgumentNull(string parameterName)
    {
      return new ArgumentNullException(parameterName);
    }

    internal static InvalidOperationException ApplyAttempt(string caller, string typeName)
    {
      return new InvalidOperationException(string.Format(Resources.exceptionApplyAttempt, caller, typeName));
    }

    internal static ArgumentOutOfRangeException MatchType(object matchType)
    {
      return new ArgumentOutOfRangeException(nameof(matchType), string.Format(Resources.exceptionMatchType, matchType));
    }

    //internal static ArgumentException MissingFieldOrProperty(string safeRuntimeName)
    //{
    //  return new ArgumentException(string.Format(Resources.exceptionMissingFieldOrProperty, safeRuntimeName), "safeRuntimeName");
    //}

    internal static ArgumentException Invalid2DFlip(object axis)
    {
      return new ArgumentException(string.Format(Resources.excption2DFlip, axis), nameof(axis));
    }

    internal static ArgumentException Invalid3DFlip(object axis)
    {
      return new ArgumentException(string.Format(Resources.excption3DFlip, axis), nameof(axis));
    }

    internal static ArgumentException Invalid4DFlip(object axis)
    {
      return new ArgumentException(string.Format(Resources.excption4DFlip, axis), nameof(axis));
    }

    //internal static ArgumentException Rotate3D(object axis)
    //{
    //  return new ArgumentException(string.Format(Resources.excption3DRotation, axis), "axis");
    //}

    internal static ArgumentException Invalid4DRotaion(object axis)
    {
      return new ArgumentException(string.Format(Resources.excption4DRotation, axis), nameof(axis));
    }

    internal static ArgumentException Invalid3DRotaion(object axis)
    {
      return new ArgumentException(string.Format(Resources.excption3DRotation, axis), nameof(axis));
    }

    //internal static ArgumentException DynamicPropertyAccess(string nameColumn)
    //{
    //  return new ArgumentException(string.Format(Resources.excptionDynamicPropertyAccess, nameColumn));
    //}

    internal static ArgumentException InvalidAngle(int angle)
    {
      return new ArgumentException(string.Format(Resources.excptionInvalidAngle, angle), nameof(angle));
    }

    internal static ArgumentException InvalidAxis(object axis)
    {
      return new ArgumentException(string.Format(Resources.excptionInvalidAxis, axis), nameof(axis));
    }

    //internal static InvalidOperationException InvalidGenerateGenericType(string typeName)
    //{
    //  return new InvalidOperationException(string.Format(Resources.excptionInvalidGenerateGenericType, typeName));
    //}

    internal static ArgumentException InvalidItemsCount(int count, string parameterName)
    {
      return new ArgumentException(string.Format(Resources.excptionInvalidItemsCount, count), parameterName);
    }

    internal static InvalidOperationException InvalidNoiseFilterType()
    {
      return new InvalidOperationException(Resources.excptionInvalidNoiseFilterType);
    }

    internal static Exception InvalidNoiseFilterTypeLimits()
    {
      return new InvalidOperationException(Resources.excptionInvalidNoiseFilterTypeLimits);
    }

    //internal static InvalidOperationException InvalidRangeGenericType(string typeName)
    //{
    //  return new InvalidOperationException(string.Format(Resources.excptionInvalidRangeGenericType, typeName));
    //}

    internal static ArgumentException InvalidSkipCount(string parameterName)
    {
      return new ArgumentException(Resources.excptionInvalidSkipCount, parameterName);
    }

    internal static ArgumentException InvalidTakeCount(string parameterName)
    {
      return new ArgumentException(Resources.excptionInvalidTakeCount, parameterName);
    }

    internal static ArgumentException ParamBelowMinValue(string parameterName, double min)
    {
      return new ArgumentException(string.Format(Resources.excptionParamMinRange, parameterName, min));
    }

    internal static ArgumentException ParamAboveMaxValue(string parameterName, double max)
    {
      return new ArgumentException(string.Format(Resources.excptionParamMaxRange, parameterName, max));
    }

    internal static ArgumentOutOfRangeException RoundingDigits(int numberOfDigits, string parameterName)
    {
      return new ArgumentOutOfRangeException(parameterName, string.Format(Resources.excptionRoundingDigits, numberOfDigits));
    }

    internal static ArgumentException SequenceIsEmpty(string parameterName)
    {
      return new ArgumentException(Resources.excptionSequenceMinOne, parameterName);
    }

    internal static Exception DurationIsZero(string parameterName)
    {
      return new ArgumentOutOfRangeException(parameterName, Resources.excptionStepDuration);
    }

    internal static ArgumentException SequenceMinTwo(string parameterName)
    {
      return new ArgumentException(Resources.excptionSequenceMinTwo, parameterName);
    }

    internal static ArgumentException SequenceMinTwoNotNull(string parameterName)
    {
      return new ArgumentException(Resources.excptionSequenceMinTwoNotNull, parameterName);
    }

    internal static ArgumentNullException SourceSequenceIsNull(int idx, string parameterName)
    {
      return new ArgumentNullException(string.Format(Resources.excptionSourceSequenceIsNull, idx), parameterName);
    }

    //internal static ArgumentOutOfRangeException StepDuration(string parameterName)
    //{
    
    //}

    internal static ArgumentException ValueMinBlockSize(string parameterName)
    {
      return new ArgumentException(Resources.excptionValueMinBlockSize, parameterName);
    }

    internal static ArgumentException ValueMinTwo(string parameterName)
    {
      return new ArgumentException(Resources.excptionValueMinTwo, parameterName);
    }

    internal static DivideByZeroException CumulativeVarianceZeroMembers()
    {
      return new DivideByZeroException();
    }
  }
}
