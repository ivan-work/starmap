using System;
using Game.Station_Status;
using Game.Station_Type;
using UnityEditor;
using UnityEngine;

namespace Game {
  [Serializable]
  public class StationData {
    private static StationStatus? _defaultStatus;
    [SerializeField] private StationStatus? _status;

    private static StationStatus DefaultStatus {
      get {
        if (_defaultStatus == null) {
          // _defaultStatus = AssetDatabase.LoadAssetAtPath<StationStatus>("Resources/Station Status/Скрыто.asset");
          _defaultStatus = ScriptableObject.CreateInstance<StationStatus>();
          _defaultStatus.Status = "Hidden";
          _defaultStatus.Color = Color.blue;
          _defaultStatus.IsHidden = true;
        }

        return _defaultStatus;
      }
    }

    [field: SerializeField]
    public string Name { get; set; } = "Unnamed station";

    [field: SerializeField] public StationType Type { get; set; }


    public StationStatus Status {
      get => _status != null ? _status : DefaultStatus;
      set => _status = value;
    }

    [field: SerializeField]
    public string Description { get; set; } = "No description";
  }
}
