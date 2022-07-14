using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Assets/Card")]
public class Card : ScriptableObject {
    public string id;
    public new string name;
    [TextArea] public string description;
    public Sprite artwork;

    public CardType type;

    public int manaCost;
    public int attack;
    public int health;
}