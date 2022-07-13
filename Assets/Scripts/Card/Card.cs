using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject {
    [SerializeField] public new string name;
    [SerializeField] public string description;
    [SerializeField] public Sprite artwork;

    [SerializeField] public CardType type;

    [SerializeField] public int manaCost;
    [SerializeField] public int attack;
    [SerializeField] public int health;
}