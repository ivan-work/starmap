using UnityEngine;

namespace Game.Station_Type {
  public class StationType : ScriptableObject {
    [field: SerializeField] public string Name { get; set; } = "";
  }
}
