using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OnHandHoverAnimation : MonoBehaviour {

    public void CardHoverEnter(CardController card, float scaleMultiplier) {
        // var cardSequence = DOTween.Sequence();

        // cardSequence.Append(card.sprite.transform.DOMove(
        //     new Vector3(
        //         card.sprite.transform.GetComponent<RectTransform>().position.x,
        //         card.sprite.transform.GetComponent<RectTransform>().position.y + 150,
        //         card.sprite.transform.GetComponent<RectTransform>().position.z
        //         ), .5f))
        // .Append(card.transform.DOScale(card.transform.localScale *= (scaleMultiplier * 2), .5f));
    }

    public void CardHoverExit(CardController card, float scaleMultiplier) {
        // var cardSequence = DOTween.Sequence();

        // cardSequence.Append(card.sprite.transform.DOMove(
        //     new Vector3(
        //         card.sprite.transform.GetComponent<RectTransform>().position.x,
        //         card.sprite.transform.GetComponent<RectTransform>().position.y - 150,
        //         card.sprite.transform.GetComponent<RectTransform>().position.z
        //         ), .5f))
        // .Append(card.transform.DOScale(card.transform.localScale /= (scaleMultiplier * 2), .5f));
    }
}
