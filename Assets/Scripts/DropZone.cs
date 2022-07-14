using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    public void OnPointerEnter(PointerEventData eventData) {
        // Debug.Log("OnPointerEnter");
        if (!eventData.pointerDrag) return;

        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable != null) draggable.placeholderParent = transform;
    }

    public void OnPointerExit(PointerEventData eventData) {
        // Debug.Log("OnPointerExit");
        if (!eventData.pointerDrag) return;

        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable != null && draggable.placeholderParent == transform) draggable.placeholderParent = draggable.defaultParent;
    }
    public void OnDrop(PointerEventData eventData) {
        // Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable != null) draggable.defaultParent = transform;
    }
}