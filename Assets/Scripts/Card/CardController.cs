using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CardController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {
    [HideInInspector] public Transform defaultParent = null;
    [HideInInspector] public Transform placeholderParent = null;

    [HideInInspector] public GameObject placeholder = null;

    public GameObject sprite;

    [SerializeField] private Card _card;

    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _manaCostText;
    [SerializeField] private TMP_Text _attackText;
    [SerializeField] private TMP_Text _healthText;

    [SerializeField] private Image _artwork;
    [SerializeField] private Image _glow;

    [SerializeField] public bool inField { get; private set; } = false;

    private Camera _mainCamera;

    private Vector3 _previousPosition;
    private Rigidbody2D _rigidbody;

    [Range(1f, 1.5f)][SerializeField] private float _scaleMultiplier = 1.1f;

    public UnityEvent OnFieldDrop = new UnityEvent();

    private OnHandHoverAnimation _hoverAnimation;

    void Awake() {
        _mainCamera = Camera.main;
        _rigidbody = GetComponent<Rigidbody2D>();

        _hoverAnimation = (new GameObject("On Hand Hover Animation")).AddComponent<OnHandHoverAnimation>();
    }

    void Start() {
        OnFieldDrop.AddListener(() => {
            inField = true;
            _glow.enabled = false;

            _card.actions.ForEach(action => action.Cast());
        });

        _nameText.text = _card.name;
        _descriptionText.text = _card.description;
        _manaCostText.text = _card.manaCost.ToString();
        _artwork.sprite = _card.artwork;

        if (_card.type == CardType.Spell) return;

        _attackText.text = _card.attack.ToString();
        _healthText.text = _card.health.ToString();
    }

    void FixedUpdate() {
        if (inField) return;

        if (GameManager.ManaController.GetMana() < _card.manaCost) {
            _glow.enabled = false;
            return;
        }
        _glow.enabled = true;
    }

    public void SetCard(Card card) {
        this._card = card;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        // Debug.Log("OnBeginDrag");
        if (inField) return;

        defaultParent = transform.parent;
        transform.SetParent(defaultParent.parent);

        transform.localScale *= _scaleMultiplier;

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
        if (inField) return;

        transform.position = eventData.position;

        Vector3 newWorldPosition = _mainCamera.ScreenToWorldPoint(eventData.position);
        var velocity = (newWorldPosition - _previousPosition).magnitude / Time.deltaTime;
        _previousPosition = newWorldPosition;

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

        if (!inField)
            transform.localScale /= _scaleMultiplier;

        transform.rotation = Quaternion.Euler(0, 0, 0);

        if (!placeholder) return;
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        Destroy(placeholder);

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        // Debug.Log("OnPointerEnter");
        if (inField) return;

        // Increase card size
        _hoverAnimation.CardHoverEnter(this, _scaleMultiplier);
        // transform.localScale *= _scaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData) {
        // Debug.Log("OnPointerExit");
        if (inField) return;

        // Decrease card size
        _hoverAnimation.CardHoverExit(this, _scaleMultiplier);
        // transform.localScale /= _scaleMultiplier;
    }

    public int GetManaCost() {
        return _card.manaCost;
    }
}