using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    public static PlayerDeck PlayerDeck;
    public static ManaController ManaController;
    public static TableController TableController;

    public ManaController manaController;

    [SerializeField] private TMP_Text _text_DeckQuantity;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);
    }

    void Start() {
        ManaController = manaController;
        PlayerDeck = new PlayerDeck();

        UpdateCardsQuantityInDeck();
    }

    void Update() {
        UpdateCardsQuantityInDeck();
    }

    private void UpdateCardsQuantityInDeck() {
        _text_DeckQuantity.text = PlayerDeck.GetCardsInDeck() + "/" + PlayerDeck.GetMaxCards();
    }

    public int GetMana() {
        return ManaController.GetMana();
    }

    public void SpendMana(int amount) {
        ManaController.SpendMana(amount);
    }
}