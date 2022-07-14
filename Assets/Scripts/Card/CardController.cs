using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardController : MonoBehaviour {

    [SerializeField] private Card card;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text manaCostText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private Image artwork;

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
}