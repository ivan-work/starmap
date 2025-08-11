using System.Collections.Generic;
using Game.Station;
using Game.Station_Status;
using Game.StationsData;
using UnityEngine;

namespace Game {
  public class Galaxy : MonoBehaviour {
    [SerializeField] private List<StationStatus> _stationStatusList = new();
    [SerializeField] private StationPrefab _stationPrefab = null!;
    public List<StationPrefab> Stations { get; } = new();

    private const int Seed = 123;
    private const float StationDistance = 15;
    private const float MaxOffset = .4f;

    private async void Awake() {
      var stationsData = await StationsDataLoader.Instance.Load(_stationStatusList);
      generateStations(stationsData);
    }

    private void generateStations(StationsDataFile stationsDataFile) {
      var screenSize = GetSize() - StationDistance;
      var stationsPerSide = (int) (screenSize / StationDistance);
      var maxIndex = stationsPerSide * stationsPerSide;
      Debug.Log($"stationsPerSide: {stationsPerSide}");
      Random.InitState(Seed);
      for (var i = 0; i < maxIndex; ++i) {
        var station = Instantiate(_stationPrefab, transform);

        // Debug.Log($"Station data setting: {stationsDataFile.Stations.GetValueOrDefault(i, new StationData()).Name}");
        station.Data = stationsDataFile.Stations.GetValueOrDefault(i, new StationData());

        var cameraOffset = new Vector2(stationsPerSide - 1, stationsPerSide - 1) * StationDistance * -.5f;
        var loc = new Vector2Int(
          i % stationsPerSide,
          i / stationsPerSide
        );
        var rngOffset = new Vector2(
          Random.Range(-MaxOffset, MaxOffset),
          Random.Range(-MaxOffset, MaxOffset)
        );
        station.Loc = (loc + rngOffset) * StationDistance + cameraOffset;
      }
    }

    public static float GetSize() {
      return (2 * Camera.main?.orthographicSize ?? 1);
    }
  }
}
