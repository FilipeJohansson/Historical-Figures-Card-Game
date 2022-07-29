using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaController : MonoBehaviour {
    const int MAX_MANA = 6;
    
    private GameObject[] crystals = new GameObject[MAX_MANA];

    [HideInInspector] public int mana;

    [SerializeField] private GameObject _crystalPrefab;
    [SerializeField] private Transform _crystalsTransform;
    [SerializeField] private TMP_Text _quantityText;


    // Start is called before the first frame update
    void Awake() {
        mana = MAX_MANA / 2;

        var crystalRect = _crystalPrefab.GetComponent<RectTransform>();
        var crystalHeight = 45f;
        for (int i = 0; i < MAX_MANA; i++) {
            crystals[i] = Instantiate(_crystalPrefab, _crystalsTransform);
            crystals[i].transform.localPosition = new Vector3(0, crystalHeight, 0);
            
            crystalHeight += (-crystalRect.sizeDelta.y * crystalRect.localScale.y) - 3;
        }

        // Get children of crystals and store them in an array
        for (int i = 0; i < _crystalsTransform.childCount; i++) {
            crystals[i] = _crystalsTransform.GetChild(i).gameObject;
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

        _quantityText.text = mana + "/" + MAX_MANA;
    }
}
