using UnityEngine;
using UnityEngine.EventSystems;

public class HandZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    public void OnPointerEnter(PointerEventData eventData) {
        // Debug.Log("OnPointerEnter");
        if (!eventData.pointerDrag) return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();
        if (card.GetInField()) return;

        if (card != null) card.placeholderParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData) {
        // Debug.Log("OnPointerExit");
        if (!eventData.pointerDrag) return;

        CardController card = eventData.pointerDrag.GetComponent<CardController>();
        if (card.GetInField()) return;

        if (card != null && card.placeholderParent == transform) card.placeholderParent = card.defaultParent;
    }
    
    public void OnDrop(PointerEventData eventData) {
        // Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        CardController card = eventData.pointerDrag.GetComponent<CardController>();
        if (card.GetInField()) return;

        if (card != null) card.defaultParent = transform;
    }
}