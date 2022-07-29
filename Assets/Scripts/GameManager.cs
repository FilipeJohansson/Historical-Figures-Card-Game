using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Mirror;

public class GameManager : NetworkBehaviour {
    public static PlayerDeck PlayerDeck;

    [Header("Database")]
    // [SerializeField] private Database _database;
    // public static Database Database;
    public CardDatabase cardDatabase;

    [SerializeField] private List<Card> cards;

    [Header("Deck")]
    const int MAX_CARDS = 30;

    [Header("Card Animation")]
    private Queue<IEnumerator> drawQueue = new Queue<IEnumerator>();
    private Coroutine currentDraw = null;

    [SerializeField] private Stack<Card> deck = new Stack<Card>(MAX_CARDS);

    [Header("Controllers")]
    // [SerializeField] private ManaController _manaController;
    public static ManaController ManaController;
    // [SerializeField] private TableController _tableController;
    public static TableController TableController;

    [Header("Draw Spawn")]
    // [SerializeField] private GameObject _drawSpawn_voidPoint;
    public static GameObject DrawSpawn_voidPoint;
    // [SerializeField] private GameObject _drawSpawn_cameraPoint;
    public static GameObject DrawSpawn_cameraPoint;

    [Header("Player")]
    // [SerializeField] private GameObject _playerHand;
    public static GameObject PlayerHand;

    [Header("Enemy")]
    // [SerializeField] private GameObject _enemyHand;
    public static GameObject EnemyHand;

    [Header("Texts")]
    // [SerializeField] private TMP_Text _text_DeckQuantity;
    public static TMP_Text Text_DeckQuantity;

    [Header("Cards Prefabs")]
    [SerializeField] private GameObject _card_MinionPrefab;
    public static GameObject Card_MinionPrefab;
    [SerializeField] private GameObject _card_SpellPrefab;
    public static GameObject Card_SpellPrefab;

    public override void OnStartClient() {
        base.OnStartClient();

        // _database = GameObject.Find("Database Controller").GetComponent<Database>();

        ManaController = GameObject.Find("Mana Controller").GetComponent<ManaController>();
        TableController = GameObject.Find("Table Controller").GetComponent<TableController>();

        DrawSpawn_voidPoint = GameObject.Find("Void Point");
        DrawSpawn_cameraPoint = GameObject.Find("Camera Point");

        PlayerHand = GameObject.Find("Player Side").transform.Find("Hand").gameObject;
        EnemyHand = GameObject.Find("Enemy Side").transform.Find("Hand").gameObject;

        Text_DeckQuantity = GameObject.Find("Deck Quantity").GetComponent<TMP_Text>();

        Card_MinionPrefab = _card_MinionPrefab;
        Card_SpellPrefab = _card_SpellPrefab;
    }

    [Server]
    public override void OnStartServer() {
        base.OnStartServer();

        CreateDatabase();

        CreateDeck();

        // UpdateCardsQuantityInDeck();
    }

    // DATABASE

    public void CreateDatabase() {
        // _database.CmdCreateDatabase();

        if (cardDatabase == null) {
            cardDatabase = Resources.Load<CardDatabase>("CardDatabase");

            if (cardDatabase == null) Debug.LogError("CardDatabase not found!");
        }

        cards = GetAllCards();
        Debug.Log("Found " + cards.Count + " cards in the database.");

        if (!VerifyIfIdIsUnique()) Debug.LogError("There are duplicate IDs in the database!");
    }

    public List<Card> GetAllCards() {
        return cardDatabase.cards;
    }

    public Card GetRandomCard() {
        return cardDatabase.cards[Random.Range(0, cardDatabase.cards.Count)];
    }

    public bool VerifyIfIdIsUnique() {
        return cardDatabase.cards.Select(c => c.id).Distinct().Count() == cardDatabase.cards.Count;
    }

    // END DATABASE

    // DECK

    [Command]
    public void CmdDrawCard() {
        if (!VerifyIfDeckHasCard()) return;

        Card card = DrawCard();

        GameObject cardObject = null;
        switch (card.type) {
            case CardType.Minion:
                cardObject = Instantiate(_card_MinionPrefab, DrawSpawn_voidPoint.transform);
                break;
            case CardType.Spell:
                cardObject = Instantiate(_card_SpellPrefab, DrawSpawn_voidPoint.transform);
                break;
        }
        cardObject.GetComponent<CardController>().SetCard(card);
        NetworkServer.Spawn(cardObject, connectionToClient);

        RpcCardSummonAnimation(cardObject, NetworkType.Dealt);
    }

    public void CreateDeck() {
        for (int i = 0; i < MAX_CARDS; i++)
            deck.Push(GetRandomCard());
    }

    public Card DrawCard() {
        Card card = deck.Pop();
        return card;
    }

    public bool VerifyIfDeckHasCard() {
        return deck.Count > 0;
    }

    private void UpdateCardsQuantityInDeck() {
        Text_DeckQuantity.text = PlayerDeck.GetCardsInDeck() + "/" + PlayerDeck.GetMaxCards();
    }

    // END DECK

    // CARD ANIMATION

    [ClientRpc]
    private void RpcCardSummonAnimation(GameObject card, NetworkType type) {
        switch (type) {
            case NetworkType.Dealt:
                if (hasAuthority)
                    StartCoroutine(CardSummonRoutine(card));
                else
                    card.transform.SetParent(EnemyHand.transform);
                break;
            case NetworkType.Played:
                break;
        }
    }

    IEnumerator CardSummonRoutine(GameObject card) {
        // Get the card in the void
        // GameObject card = DrawSpawn_voidPoint.transform.GetChild(0).gameObject;
        card.transform.SetParent(DrawSpawn_cameraPoint.transform);
        var normalScale = card.transform.localScale;
        card.transform.localScale = new Vector3(1f, 1f, 1f);

        var cardSequence = DOTween.Sequence();

        // DOTween card from the void to the camera
        cardSequence.Append(card.transform.DOMove(DrawSpawn_cameraPoint.transform.position, .5f));

        Vector3 newPosition = new Vector3(PlayerHand.transform.position.x, PlayerHand.GetComponent<RectTransform>().rect.height / 2, 0);

        if (PlayerHand.transform.childCount > 0) {
            GameObject lastCard = PlayerHand.transform.GetChild(PlayerHand.transform.childCount - 1).gameObject;

            float lastCardWidth = lastCard.GetComponent<RectTransform>().rect.width;
            float lastCardScale = lastCard.transform.localScale.x;
            float xPosition = ((lastCardWidth * lastCardScale) / 3f) + lastCard.transform.position.x;
            newPosition = new Vector3(xPosition, lastCard.transform.position.y, 0);
        }

        yield return new WaitForSeconds(1f);

        cardSequence.Append(card.transform.DOMove(newPosition, .5f))
        .Append(card.transform.DOScale(normalScale, .5f));

        yield return new WaitForSeconds(.5f);

        card.transform.SetParent(PlayerHand.transform);
    }

    // END CARD ANIMATION

    public static int GetMana() {
        return ManaController.GetMana();
    }

    public static void SpendMana(int amount) {
        ManaController.SpendMana(amount);
    }
}