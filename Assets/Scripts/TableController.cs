using UnityEngine;

public class TableController : MonoBehaviour {

    [SerializeField] private GameObject drawSpawn_voidPoint;
    [SerializeField] private GameObject drawSpawn_cameraPoint;

    [SerializeField] private GameObject playerHand;

    [SerializeField] private GameObject card_MinionPrefab;
    [SerializeField] private GameObject card_SpellPrefab;
    
    [SerializeField] private DrawAnimation drawAnimation;

    void Awake() {
        drawAnimation = (new GameObject("Draw Animation")).AddComponent<DrawAnimation>();
        drawAnimation.voidPoint = drawSpawn_voidPoint;
        drawAnimation.cameraPoint = drawSpawn_cameraPoint;
        drawAnimation.playerHand = playerHand;
    }

    public void DrawCard() {
        Card card = Database.GetRandomCard();

        GameObject cardObject = null;
        switch (card.type) {
            case CardType.Minion:
                cardObject = Instantiate(card_MinionPrefab, drawSpawn_voidPoint.transform);
                break;
            case CardType.Spell:
                cardObject = Instantiate(card_SpellPrefab, drawSpawn_voidPoint.transform);
                break;
        }
        cardObject.GetComponent<CardController>().SetCard(card);

        drawAnimation.CardSummon();
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