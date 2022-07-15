using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static PlayerDeck playerDeck;

    [SerializeField] private TMP_Text _text_DeckQuantity;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);
    }

    void Start() {
        playerDeck = new PlayerDeck();

        UpdateCardsQuantityInDeck();
    }

    void Update() {
        UpdateCardsQuantityInDeck();
    }

    private void UpdateCardsQuantityInDeck() {
        _text_DeckQuantity.text = playerDeck.GetCardsInDeck() + "/" + playerDeck.GetMaxCards();
    }
}