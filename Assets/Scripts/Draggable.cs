using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    [HideInInspector] public Transform defaultParent = null;
    [HideInInspector] public Transform placeholderParent = null;

    [HideInInspector] public GameObject placeholder = null;

    private Camera _mainCamera;
    
    private Vector3 previousPosition;
    private Rigidbody2D _rigidbody;

    [Range(1f, 1.5f)] [SerializeField] private float scaleMultiplier = 1.0f;

    void Awake() {
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        // Debug.Log("OnBeginDrag");
        defaultParent = transform.parent;
        transform.SetParent(defaultParent.parent);

        transform.localScale *= scaleMultiplier;

        placeholder = new GameObject();
        placeholder.transform.SetParent(defaultParent);

        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

        placeholderParent = defaultParent;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        // Debug.Log("OnDrag");

        transform.position = eventData.position;

        Vector3 newWorldPosition = _mainCamera.ScreenToWorldPoint(eventData.position);
        var velocity = (newWorldPosition - previousPosition).magnitude / Time.deltaTime;
        previousPosition = newWorldPosition;

        var angle = Mathf.Atan2(-newWorldPosition.x * velocity, newWorldPosition.y * velocity) * Mathf.Rad2Deg;
        _rigidbody.MoveRotation(angle);
        // transform.rotation = Quaternion.Euler(Mathf.Abs(angle / 1.1f), 0, angle);
        // transform.rotation = Quaternion.Euler(angle / -1.1f, angle / 2.5f, angle / 1.5f);

        if (placeholder.transform.parent != placeholderParent)
            placeholder.transform.SetParent(placeholderParent);

        int newSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; i++) {
            if (transform.position.x < placeholderParent.GetChild(i).position.x) {
                newSiblingIndex = i;

                if (placeholder.transform.GetSiblingIndex() < newSiblingIndex) newSiblingIndex--;

                break;
            }
        }

        placeholder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData) {
        // Debug.Log("OnEndDrag");

        transform.SetParent(defaultParent);

        transform.localScale /= scaleMultiplier;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        Destroy(placeholder);

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}