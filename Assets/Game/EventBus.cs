using System;
using Game.Station;

namespace Game
{
  public static class EventBus {
    public static event Action<StationPrefab>? StationHoverStart; 
    public static event Action<StationPrefab>? StationHoverStop; 
    public static event Action<StationPrefab>? StationSelected; 
    public static void InvokeStationHoverStart(StationPrefab station) {
      StationHoverStart?.Invoke(station);
    }

    public static void InvokeStationHoverStop(StationPrefab station) {
      StationHoverStop?.Invoke(station);
    }
    
    public static void InvokeStationSelected(StationPrefab station) {
      StationSelected?.Invoke(station);
    }
  }
}
