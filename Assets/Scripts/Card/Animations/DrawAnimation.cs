using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DrawAnimation : MonoBehaviour {
    public GameObject voidPoint;
    public GameObject cameraPoint;
    public GameObject playerHand;

    private Queue<IEnumerator> drawQueue = new Queue<IEnumerator>();
    private Coroutine currentDraw = null;

    void Update() {
        if (drawQueue.Count <= 0 || currentDraw != null) return;
        currentDraw = StartCoroutine(DequeueCard());
    }

    public void CardSummon() {
        drawQueue.Enqueue(CardSummonRoutine());
    }

    IEnumerator DequeueCard() {
        yield return StartCoroutine(drawQueue.Dequeue());
        currentDraw = null;
    }

    IEnumerator CardSummonRoutine() {
        // Get the card in the void
        GameObject card = voidPoint.transform.GetChild(0).gameObject;
        card.transform.SetParent(cameraPoint.transform);

        // DOTween card from the void to the camera
        card.transform.DOMove(cameraPoint.transform.position, 0.5f).OnComplete(() => {
            Vector3 newPosition = new Vector3(playerHand.transform.position.x, playerHand.GetComponent<RectTransform>().rect.height / 2, 0);

            if (playerHand.transform.childCount > 0) {
                GameObject lastCard = playerHand.transform.GetChild(playerHand.transform.childCount - 1).gameObject;

                float lastCardWidth = lastCard.GetComponent<RectTransform>().rect.width;
                float lastCardScale = lastCard.transform.localScale.x;
                float xPosition = ((lastCardWidth * lastCardScale) / 2) + lastCard.transform.position.x;
                newPosition = new Vector3(xPosition, lastCard.transform.position.y, 0);
            }

            // Move the card to the player's hand
            card.transform.DOMove(newPosition, 0.5f).OnComplete(() => {
                // Put card inside the player's hand
                card.transform.SetParent(playerHand.transform);
            });
        });

        yield return new WaitForSeconds(.5f);
    }
}