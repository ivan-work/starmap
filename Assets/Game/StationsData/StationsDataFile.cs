using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.StationsData {
  [Serializable]
  public class StationsDataFile {
    [field: SerializeField]
    public Dictionary<int, StationData> Stations { get; set; } = new();
  }
}
