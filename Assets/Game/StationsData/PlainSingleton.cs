using System;

namespace Game.StationsData {
  public class PlainSingleton<T> {
    private static T? _instance;
    private static readonly object Lock = new object();

    public static T Instance {
      get {
        lock (Lock) {
          if (_instance != null) {
            return _instance;
          }

          return _instance = Activator.CreateInstance<T>();
        }
      }
    }
  }
}
