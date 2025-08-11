using System;
using System.Collections.Generic;
using Game.Station;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Station_Selection {
  public class PathPanelController : MonoBehaviour {
    private UIDocument uiDocument = null!;
    private ListView pathListView = null!;
    private readonly List<StationPrefab> selection = new();

    private void Awake() {
      uiDocument = GetComponent<UIDocument>();
      pathListView = uiDocument.rootVisualElement.Q<ListView>("PathListView");
      pathListView.visible = false;
      pathListView.itemsSource = selection;
      pathListView.bindItem = (el, i) => {
        if (el is StationItem item) {
          item.Item = selection[i];
        }
      };
      pathListView.makeItem = () => {
        var label = new StationItem();
        label.RegisterCallback<PointerEnterEvent>(onItemEnter);
        label.RegisterCallback<PointerLeaveEvent>(onItemLeave);
        label.RegisterCallback<PointerDownEvent>(onItemClick);
        return label;
      };
      pathListView.selectionType = SelectionType.None;
    }

    private static void onItemEnter(PointerEnterEvent evt) {
      if (evt.target is StationItem item && item.Item != null) {
        Debug.Log($"Station hovered: {item.Item.Data.Name}");
        EventBus.InvokeStationHoverStart(item.Item);
      } else {
        Debug.Log($"evt.target is invalid: {evt.target}");
      }
    }
    private static void onItemLeave(PointerLeaveEvent evt) {
      if (evt.target is StationItem item && item.Item != null) {
        EventBus.InvokeStationHoverStop(item.Item);
      }
    }
    private static void onItemClick(PointerDownEvent evt) {
      if (evt.target is StationItem item && item.Item != null) {
        EventBus.InvokeStationSelected(item.Item);
        EventBus.InvokeStationHoverStop(item.Item);
      }
    }

    private void OnEnable() {
      EventBus.StationSelected += OnStationSelected;
    }
    
    private void OnDisable() {
      EventBus.StationSelected -= OnStationSelected;
    }

    private void OnStationSelected(StationPrefab station) {
      if (!station.IsSelected) {
        selection.Add(station);
      } else {
        selection.Remove(station);
      }

      pathListView.visible = selection.Count > 0;
      pathListView.Rebuild();
    }
  }

  public class StationItem : Label {
    private StationPrefab? item;

    public StationPrefab? Item {
      get => item;
      set {
        item = value;
        if (item != null) {
          text = item.Data.Name;
        }
      }
    }
  }
}
