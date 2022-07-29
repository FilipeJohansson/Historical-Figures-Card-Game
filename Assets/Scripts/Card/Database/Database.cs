using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Database : MonoBehaviour {
    public CardDatabase cardDatabase;

    private static Database _instance;

    [SerializeField] private List<Card> cards;

    void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);

        if (cardDatabase == null) {
            cardDatabase = Resources.Load<CardDatabase>("CardDatabase");

            if (cardDatabase == null) Debug.LogError("CardDatabase not found!");
        }

        cards = GetAllCards();
        Debug.Log("Found " + cards.Count + " cards in the database.");

        if(!VerifyIfIdIsUnique()) Debug.LogError("There are duplicate IDs in the database!");
    }

    public static Card GetCardById(string id) {
        return _instance.cardDatabase.cards.FirstOrDefault(c => c.id == id);
    }

    public static Card GetCardByName(string name) {
        return _instance.cardDatabase.cards.FirstOrDefault(c => c.name == name);
    }

    public static Card GetRandomCard() {
        return _instance.cardDatabase.cards[Random.Range(0, _instance.cardDatabase.cards.Count)];
    }

    public static List<Card> GetAllCards() {
        return _instance.cardDatabase.cards;
    }

    public static List<Card> GetCardsByType(CardType type) {
        return _instance.cardDatabase.cards.Where(c => c.type == type).ToList();
    }

    public static List<Card> GetCardsByManaCost(int manaCost) {
        return _instance.cardDatabase.cards.Where(c => c.manaCost == manaCost).ToList();
    }

    public static List<Card> GetCardsByAttack(int attack) {
        return _instance.cardDatabase.cards.Where(c => c.attack == attack).ToList();
    }

    public static List<Card> GetCardsByHealth(int health) {
        return _instance.cardDatabase.cards.Where(c => c.health == health).ToList();
    }

    public static bool VerifyIfIdIsUnique() {
        return _instance.cardDatabase.cards.Select(c => c.id).Distinct().Count() == _instance.cardDatabase.cards.Count;
    }
}