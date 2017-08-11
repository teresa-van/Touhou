using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardVisuals : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BlowUp(GameObject card)
    {
        card.transform.parent.SetAsLastSibling();
        card.transform.SetAsLastSibling();
        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOLocalMoveY(card.transform.localPosition.y + 75f, 0.25f));
        s.Append(card.transform.DOScale(new Vector3(2.2f, 2.2f, 2.2f), 0.25f));
    }

    public void Shrink(GameObject card)
    {
        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOLocalMoveY(card.transform.localPosition.y - 75f, 0.25f));
        s.Append(card.transform.DOScale(Vector3.one, 0.25f));
    }
}
