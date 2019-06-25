using System;
using System.Collections.Generic;

namespace LinqExtensions.Sort
{
  /// <summary>
  /// Documentation
  /// </summary>
  public static class SortFunctions
  {
    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <returns></returns>
    public static IComposableSortCollection<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortType sortType)
    {
      return new ComposableSortCollection<TSource, TKey>(source, keySelector, sortType, null, false);
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IComposableSortCollection<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortType sortType, IComparer<TKey> comparer)
    {
      return new ComposableSortCollection<TSource, TKey>(source, keySelector, sortType, comparer, false);
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <returns></returns>
    public static IComposableSortCollection<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortType sortType)
    {
      return new ComposableSortCollection<TSource, TKey>(source, keySelector, sortType, null, true);
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IComposableSortCollection<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, SortType sortType, IComparer<TKey> comparer)
    {
      return new ComposableSortCollection<TSource, TKey>(source, keySelector, sortType, comparer, true);
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <returns></returns>
    public static IComposableSortCollection<TSource> ThenBy<TSource, TKey>(this IComposableSortCollection<TSource> source, Func<TSource, TKey> keySelector, SortType sortType)
    {
      Utilities.ValidateIsNotNull(source, nameof(source));

      source.AppendSorter(keySelector, sortType, null, false);
      return source;
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IComposableSortCollection<TSource> ThenBy<TSource, TKey>(this IComposableSortCollection<TSource> source, Func<TSource, TKey> keySelector, SortType sortType, IComparer<TKey> comparer)
    {
      Utilities.ValidateIsNotNull(source, nameof(source));

      source.AppendSorter(keySelector, sortType, comparer, false);
      return source;
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <returns></returns>
    public static IComposableSortCollection<TSource> ThenByDescending<TSource, TKey>(this IComposableSortCollection<TSource> source, Func<TSource, TKey> keySelector, SortType sortType)
    {
      Utilities.ValidateIsNotNull(source, nameof(source));

      source.AppendSorter(keySelector, sortType, null, true);
      return source;
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="sortType"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IComposableSortCollection<TSource> ThenByDescending<TSource, TKey>(this IComposableSortCollection<TSource> source, Func<TSource, TKey> keySelector, SortType sortType, IComparer<TKey> comparer)
    {
      Utilities.ValidateIsNotNull(source, nameof(source));

      source.AppendSorter(keySelector, sortType, comparer, true);
      return source;
    }
  }
}







