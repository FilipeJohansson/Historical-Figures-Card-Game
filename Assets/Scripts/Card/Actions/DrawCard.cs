using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCard : Action {
    [SerializeField] private int cardsToDraw = 1;

    public override void Cast() {
        GameManager.TableController.DrawCards(cardsToDraw);
    }
}
