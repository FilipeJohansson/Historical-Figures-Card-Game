using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Database", menuName = "Assets/Databases/New Card Database")]
public class CardDatabase : ScriptableObject {
    public List<Card> cards;
}