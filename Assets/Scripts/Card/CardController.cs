using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {
    [HideInInspector] public Transform defaultParent = null;
    [HideInInspector] public Transform placeholderParent = null;

    [HideInInspector] public GameObject placeholder = null;

    [SerializeField] private Card card;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text manaCostText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private Image artwork;

    private Camera _mainCamera;
    
    private Vector3 previousPosition;
    private Rigidbody2D _rigidbody;

    [Range(1f, 1.5f)] [SerializeField] private float scaleMultiplier = 1.1f;

    void Awake() {
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start() {
        nameText.text = card.name;
        descriptionText.text = card.description;
        manaCostText.text = card.manaCost.ToString();
        artwork.sprite = card.artwork;

        if (card.type == CardType.Spell) return;

        attackText.text = card.attack.ToString();
        healthText.text = card.health.ToString();
    }

    public void SetCard(Card card) {
        this.card = card;
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

    public void OnPointerEnter(PointerEventData eventData) {
        // Debug.Log("OnPointerEnter");
        // Increase card size
        transform.localScale *= scaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData) {
        // Debug.Log("OnPointerExit");
        // Decrease card size
        transform.localScale /= scaleMultiplier;
    }
}