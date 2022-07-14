using UnityEngine;

public class TableController : MonoBehaviour {

    [SerializeField] private GameObject playerHand;
    
    [SerializeField] private GameObject card_MinionPrefab;
    [SerializeField] private GameObject card_SpellPrefab;
    
    public void DrawCard() {
        Card card = Database.GetRandomCard();

        GameObject cardObject = null;
        switch (card.type) {
            case CardType.Minion:
                cardObject = Instantiate(card_MinionPrefab, playerHand.transform);
                break;
            case CardType.Spell:
                cardObject = Instantiate(card_SpellPrefab, playerHand.transform);
                break;
        }
        cardObject.GetComponent<CardController>().SetCard(card);
    }

    public void DrawCards(int count) {
        for (int i = 0; i < count; i++) {
            DrawCard();
        }
    }
    
    public void EraseHand() {
        foreach (Transform child in playerHand.transform)
            Destroy(child.gameObject);
    }
}