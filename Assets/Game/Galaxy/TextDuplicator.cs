using System;
using Game;
using TMPro;
using UnityEngine;

public class TextDuplicator : MonoBehaviour {
  [SerializeField] private TextMeshPro _exampleText = null!;
  private const int NumberOfTiles = 8;

  private void Start() {
    var tileSize = Galaxy.GetSize() / NumberOfTiles;
    transform.localPosition = new Vector3(-Galaxy.GetSize() / 2, -Galaxy.GetSize() / 2);

    for (var x = 0; x < NumberOfTiles; x++) {
      var text = Instantiate(_exampleText, transform);
      text.text = $"{Convert.ToChar(65 + x)}";
      text.name = $"H{x}";
      text.transform.localPosition = new Vector3((x + .5f) * tileSize, Galaxy.GetSize() - tileSize / 2);
    }

    for (var y = 0; y < NumberOfTiles; y++) {
      var text = Instantiate(_exampleText, transform);
      text.text = $"{NumberOfTiles - y}";
      text.name = $"V{y}";
      text.alignment = TextAlignmentOptions.Left;
      text.verticalAlignment = VerticalAlignmentOptions.Middle;
      text.transform.localPosition = new Vector3(tileSize / 2, (y + .5f) * tileSize);
    }

    // galaxy.
    _exampleText.gameObject.SetActive(false);
  }
}
