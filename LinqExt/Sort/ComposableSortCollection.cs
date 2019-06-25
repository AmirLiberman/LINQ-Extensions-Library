using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;

namespace LinqExtensions.Sort
{
  /// <summary>
  /// Documentation
  /// </summary>
  /// <typeparam name="TElement"></typeparam>
  public abstract class ComposableSortCollection<TElement> : IComposableSortCollection<TElement>
  {
    internal IEnumerable<TElement> Source;
    internal ComposableSorter<TElement> ComposableSorter;

    /// <summary>
    /// Documentation
    /// </summary>
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected ComposableSortCollection()
    { }

    public void AppendSorter<TKey>(Func<TElement, TKey> keySelector, SortType sortType, IComparer<TKey> comparer, bool descending)
    {
      ComposableSortCollection<TElement, TKey> composableSortEnumerable = new ComposableSortCollection<TElement, TKey>(this, keySelector, sortType, comparer, descending);
      SetNext(ComposableSorter, composableSortEnumerable.ComposableSorter);
    }

    private static void SetNext(ComposableSorter<TElement> firstSorter, ComposableSorter<TElement> secondSorter)
    {
      if (firstSorter.Next == null)
        firstSorter.Next = secondSorter;
      else
        SetNext(firstSorter.Next, secondSorter);
    }

    /// <summary>
    /// Documentation
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator<TElement> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }

  internal class ComposableSortCollection<TElement, TKey> : ComposableSortCollection<TElement>
  {
    internal Func<TElement, TKey> KeySelector;
    internal IComparer<TKey> Comparer;
    internal bool Descending;
    internal SortType SortType;

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal ComposableSortCollection(IEnumerable<TElement> source, Func<TElement, TKey> keySelector, SortType sortType, IComparer<TKey> comparer, bool descending)
    {
      Utilities.ValidateIsNotNull(source, nameof(source));
      Utilities.ValidateIsNotNull(keySelector, nameof(keySelector));

      if (!Enum.IsDefined(typeof(SortType), sortType))
        sortType = SortType.Quick;

      Source = source;
      KeySelector = keySelector;
      SortType = sortType;

      IComparer<TKey> defaultComparer = comparer ?? Comparer<TKey>.Default;

      Comparer = defaultComparer;
      Descending = descending;

      ComposableSorter = new ComposableSorter<TElement, TKey>(KeySelector, SortType, Comparer, Descending);
    }

    public override IEnumerator<TElement> GetEnumerator()
    {
      return ComposableSorter.Sort(Source);
    }
  }
}
