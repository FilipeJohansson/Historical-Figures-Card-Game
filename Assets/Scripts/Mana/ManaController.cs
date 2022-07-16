using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : MonoBehaviour {
    const int MAX_MANA = 10;
    
    private GameObject[] crystals = new GameObject[MAX_MANA];

    public int mana;

    // Start is called before the first frame update
    void Awake() {
        mana = MAX_MANA / 2;

        // Get childrens of crystals and store them in an array
        for (int i = 0; i < transform.childCount; i++) {
            crystals[i] = transform.GetChild(i).gameObject;
        }

        // Update the mana crystals
        UpdateMana();
    }

    public bool VerifyIfCanUseMana(int amount) {
        return mana >= amount;
    }

    public void SpendMana(int amount) {
        RemoveMana(amount);
    }

    public void AddMana(int amount) {
        mana += amount;
        if (mana > MAX_MANA)
            mana = MAX_MANA;
        UpdateMana();
    }

    public void ResetMana() {
        mana = 0;
        UpdateMana();
    }

    public int GetMana() {
        return mana;
    }

    public int GetMaxMana() {
        return MAX_MANA;
    }

    private void RemoveMana(int amount) {
        mana -= amount;
        if (mana < 0)
            mana = 0;
        UpdateMana();
    }

    private void UpdateMana() {
        for (int i = 0; i < MAX_MANA; i++) {
            if (i < mana)
                crystals[i].SetActive(true);
            else
                crystals[i].SetActive(false);
        }
    }
}
