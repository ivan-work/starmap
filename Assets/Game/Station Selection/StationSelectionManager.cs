using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Game.Station;
using UnityEngine;

namespace Game.Station_Selection {
  public class StationSelectionManager : MonoBehaviour {
    private const float DefaultJumpRadius = 20f;
    
    [DllImport("__Internal")]
    private static extern float GetJumpRadius();
    
    [field: SerializeField] private GameObject _jumpCircle = null!;
    private readonly List<StationPrefab> selection = new();
    private readonly List<StationPrefab> highlightedStations = new();
    [SerializeField] private float _jumpRadius = DefaultJumpRadius;
    private float JumpRadius {
      get => _jumpRadius;
      set {
        _jumpRadius = value > 1 ? value : DefaultJumpRadius;
        _jumpCircle.transform.localScale = new Vector3(_jumpRadius, _jumpRadius) * 2;
      }
    }

    private void Start() {
#if UNITY_WEBGL && !UNITY_EDITOR
      JumpRadius = GetJumpRadius();
#endif
      
      EventBus.StationSelected += OnStationSelected;
      EventBus.StationHoverStart += OnStationHoverStart;
      EventBus.StationHoverStop += OnStationHoverStop;
    }

    private void OnStationSelected(StationPrefab station) {
      if (!station.IsSelected) {
        station.IsSelected = true; // Должно быть в самой станции
        selection.Add(station);
      } else {
        station.IsSelected = false;
        selection.Remove(station);
      }

      var lineRenderer = GetComponent<LineRenderer>();
      lineRenderer.positionCount = selection.Count;
      lineRenderer.SetPositions(selection.Select(selectedStation => selectedStation.transform.position).ToArray());
    }

    private void OnStationHoverStart(StationPrefab station) {
      var position = station.transform.position;
      _jumpCircle.transform.position = position;
      _jumpCircle.SetActive(true);

      var hitColliders = Physics2D.OverlapCircleAll(position, JumpRadius);
      foreach (var hitCollider in hitColliders) {
        if (hitCollider.TryGetComponent<StationPrefab>(out var stationPrefab)) {
          if (Vector3.Distance(position, stationPrefab.transform.position) < JumpRadius) {
            highlightedStations.Add(stationPrefab);
            stationPrefab.IsHighlighted = true;
          }
        }
      }
    }

    private void OnStationHoverStop(StationPrefab station) {
      _jumpCircle.SetActive(false);
      foreach (var highlightedStation in highlightedStations) {
        highlightedStation.IsHighlighted = false;
      }
    }
  }
}
