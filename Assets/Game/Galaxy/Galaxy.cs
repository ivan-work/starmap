using System;
using System.Collections.Generic;
using Game.Station;
using Game.Station_Status;
using Game.StationsData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game {
  public class Galaxy : MonoBehaviour {
    [SerializeField] private List<StationStatus> _stationStatusList = new();
    [SerializeField] private StationPrefab _stationPrefab = null!;
    public List<StationPrefab> Stations { get; } = new();

    [SerializeField] private int _seed = 123;
    [SerializeField] private float _stationsAreaScale = .95f;
    [SerializeField] private float _maxOffset = .4f;
    [SerializeField] private float _scaleInitial = 128f;

    private async void Awake() {
      var stationsData = await StationsDataLoader.Instance.Load(_stationStatusList);
      generateStations(stationsData);
    }

    private void generateStations(StationsDataFile stationsDataFile) {
      var stationsCount = stationsDataFile.Stations.Count;
      var stationScale = _scaleInitial / stationsCount;
      
      var stationsPerSide = (int) Math.Ceiling(Math.Sqrt(stationsCount));
      var stationDistance = _stationsAreaScale * GetSize() / (stationsPerSide - 1);
      var startOffset = ((stationsPerSide - 1) * stationDistance) / 2;
      var startOffsetVector = new Vector2(-startOffset, -startOffset);
      
      // Debug.Log($"stations: side({stationsPerSide}) total({stationsDataFile.Stations.Count})");
      Random.InitState(_seed);
      for (var i = 0; i < stationsCount; ++i) {
        var station = Instantiate(_stationPrefab, transform);
        station.transform.localScale = new Vector3(stationScale, stationScale, stationScale);

        station.Data = stationsDataFile.Stations.GetValueOrDefault(i, new StationData());

        var loc = new Vector2Int(
          i % stationsPerSide,
          i / stationsPerSide
        );
        var rngOffset = new Vector2(
          Random.Range(-_maxOffset, _maxOffset),
          Random.Range(-_maxOffset, _maxOffset)
        );
        station.Loc = startOffsetVector + (loc + rngOffset) * stationDistance;
        
        // Debug.Log($"Station instantiated: {station.Data.Name} {station.Loc}");
      }
    }

    public static float GetSize() {
      return (2 * Camera.main?.orthographicSize ?? 1);
    }
  }
}
