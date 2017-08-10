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
        card.transform.SetAsLastSibling();
        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOScale(new Vector3(3.5f, 3.5f, 3.5f), 0.25f));
    }

    public void Shrink(GameObject card)
    {
        Sequence s = DOTween.Sequence();
        s.Append(card.transform.DOScale(Vector3.one, 0.25f));
    }
}
