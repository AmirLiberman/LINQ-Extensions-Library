
namespace LinqExtensions.Sort.Sorters
{
  internal interface ISort<TKey>
  {
    MapItem<TKey>[] Sort();
  }
}
