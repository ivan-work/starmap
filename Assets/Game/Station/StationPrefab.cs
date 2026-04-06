using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Station {
  public class StationPrefab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    private Vector2 loc;

    public Vector2 Loc {
      get => loc;
      set {
        loc = value;
        transform.localPosition = new Vector3(loc.x, loc.y, 0);
      }
    }

    [field: SerializeField] public StationData Data { get; set; } = new();
    private bool IsHidden => Data.Status.IsHidden;

    [field: SerializeField] private GameObject _selectionCircle = null!;
    private bool isSelected;
    public bool IsSelected {
      get => isSelected;
      set {
        isSelected = value;
        _selectionCircle.SetActive(value);
      }
    }
    [field: SerializeField] private GameObject _highlightCircle = null!;
    private bool isHighlighted;
    public bool IsHighlighted {
      get => isHighlighted;
      set {
        isHighlighted = value;
        _highlightCircle.SetActive(value);
      }
    }

    private void Start() {
      GetComponentInChildren<SpriteRenderer>().color = Data.Status.Color;
      gameObject.SetActive(!IsHidden);
      _selectionCircle.SetActive(false);
      _highlightCircle.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
      EventBus.InvokeStationHoverStart(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
      EventBus.InvokeStationHoverStop(this);
    }

    public void OnPointerClick(PointerEventData eventData) {
      EventBus.InvokeStationSelected(this);
    }
  }
}
