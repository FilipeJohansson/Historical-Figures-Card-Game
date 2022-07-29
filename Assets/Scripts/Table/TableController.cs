using UnityEngine;
using Mirror;

public class TableController : MonoBehaviour {

    [SerializeField] private GameObject _drawSpawn_voidPoint;
    [SerializeField] private GameObject _drawSpawn_cameraPoint;

    [SerializeField] private GameObject _playerHand;

    [SerializeField] private GameObject _card_MinionPrefab;
    [SerializeField] private GameObject _card_SpellPrefab;

    private DrawAnimation _drawAnimation;

    void Awake() {
        _drawAnimation = (new GameObject("Draw Animation")).AddComponent<DrawAnimation>();
        _drawAnimation.voidPoint = _drawSpawn_voidPoint;
        _drawAnimation.cameraPoint = _drawSpawn_cameraPoint;
        _drawAnimation.playerHand = _playerHand;
    }

    public void DrawCard() {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        GameManager gameManager = networkIdentity.GetComponent<GameManager>();
        gameManager.CmdDrawCard();

        // if (!GameManager.PlayerDeck.VerifyIfDeckHasCard()) return;

        // Card card = GameManager.PlayerDeck.DrawCard();

        // GameObject cardObject = null;
        // switch (card.type) {
        //     case CardType.Minion:
        //         cardObject = Instantiate(_card_MinionPrefab, _drawSpawn_voidPoint.transform);
        //         break;
        //     case CardType.Spell:
        //         cardObject = Instantiate(_card_SpellPrefab, _drawSpawn_voidPoint.transform);
        //         break;
        // }
        // cardObject.GetComponent<CardController>().SetCard(card);

        // _drawAnimation.CardSummon();
    }

    public void DrawCards(int count) {
        for (int i = 0; i < count; i++)
            DrawCard();
    }

    public void EraseHand() {
        foreach (Transform child in _playerHand.transform)
            Destroy(child.gameObject);
    }
}