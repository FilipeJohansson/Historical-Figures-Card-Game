using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck {
    const int MAX_CARDS = 30;

    [SerializeField] private Stack<Card> deck = new Stack<Card>(MAX_CARDS);
    // public List<Card> discard = new List<Card>();
    // public List<Card> hand = new List<Card>();
    // public List<Card> graveyard = new List<Card>();
    // public List<Card> library = new List<Card>();

    public PlayerDeck() {
        // for (int i = 0; i < MAX_CARDS; i++)
        //     deck.Push(Database.GetRandomCard());

        // Debug.Log("PlayerDeck: " + deck.Count);
    }

    // public Card DrawCard() {
    //     Card card = deck.Pop();

    //     // Debug.Log("PlayerDeck: " + deck.Count);
    //     // Debug.Log("Card: " + card.id + ", " + card.name);
    //     return card;
    // }

    // public bool VerifyIfDeckHasCard() {
    //     return deck.Count > 0;
    // }

    // public void AddCardToDeck(Card card) {
    //     deck.Push(card);
    // }

    public int GetCardsInDeck() {
        return deck.Count;
    }
    
    public int GetMaxCards() {
        return MAX_CARDS;
    }
}