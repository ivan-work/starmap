using UnityEngine;

namespace Game.Station_Status {
  public class StationStatus : ScriptableObject {
    [field: SerializeField] public string Status { get; set; } = "";
    [field: SerializeField] public Color Color { get; set; } = Color.white;
    [field: SerializeField] public bool IsHidden { get; set; } = false;
  }
}
