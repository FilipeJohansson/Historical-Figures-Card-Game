using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveMana : Action {
    [SerializeField] private int manaToGive;

    public override void Cast() {
        GameManager.ManaController.AddMana(manaToGive);
    }
}
