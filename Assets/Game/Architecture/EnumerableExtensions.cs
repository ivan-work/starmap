using System.Collections.Generic;
using System.Linq;

namespace Game.Architecture {
  public static class EnumerableExtensions {
    public static IEnumerable<T> MyNotNull<T>(this IEnumerable<T?> enumerable) where T : class {
      return enumerable.Where(i => i is not null).OfType<T>();
    }

    public static TSource? MaybeFirst<TSource>(this IEnumerable<TSource> source) {
      return (TSource?) source.FirstOrDefault();
    }
  }
}
