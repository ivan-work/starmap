using Game.Station;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Station_Info {
  public class StationInfoPanelController: MonoBehaviour {
    private VisualElement stationInfoRoot = null!;
    private UIDocument uiDocument = null!;

    private void OnEnable() {
      uiDocument = GetComponent<UIDocument>();
      stationInfoRoot = uiDocument.rootVisualElement.Q("StationInfoRoot");
      stationInfoRoot.visible = false;
      
      EventBus.StationHoverStart += OnStationHoverStart;
      EventBus.StationHoverStop += OnStationHoverStop;
    }
    
    private void OnDisable() {
      EventBus.StationHoverStart -= OnStationHoverStart;
      EventBus.StationHoverStop -= OnStationHoverStop;
    }

    private void OnStationHoverStart(StationPrefab station) {
      stationInfoRoot.dataSource = station.Data;
      stationInfoRoot.visible = true;
      foreach (var item in uiDocument.rootVisualElement.Query(className: "dynamicTinting").ToList()) {
        // item.style.backgroundColor = data.Status.Color;
        var color = station.Data.Status.Color;
        color.a = .8f;
        item.style.unityBackgroundImageTintColor = color;
      }
    }

    private void OnStationHoverStop(StationPrefab station) {
      stationInfoRoot.dataSource = null;
      stationInfoRoot.visible = false;
    }
  }
}
