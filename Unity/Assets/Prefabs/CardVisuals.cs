using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider))]
public class CardVisuals : MonoBehaviour {

    Vector3 initYPos;
    Vector3 screenPoint;
    Vector3 offset;
    float lockedYPosition;

	// Use this for initialization
	void Start () {
        initYPos = gameObject.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseDown()
    {
        GameManager.Instance.dragging = true;
        lockedYPosition = screenPoint.y;
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        Cursor.visible = false;
    }

    public void OnMouseDrag()
    {
        Sequence s = DOTween.Sequence();
        s.Insert(0, gameObject.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.15f));

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        //curPosition.x = lockedYPosition;
        transform.position = curPosition;
    }

    public void OnMouseUp()
    {
        Sequence s = DOTween.Sequence();
        s.Append(gameObject.transform.DOLocalMove(initYPos, 0.25f));
        Cursor.visible = true;
        GameManager.Instance.dragging = false;
    }

    public void BlowUp()
    {
        if (!GameManager.Instance.dragging)
        {
            gameObject.transform.parent.SetAsLastSibling();
            gameObject.transform.SetAsLastSibling();
            Sequence s = DOTween.Sequence();
            s.Append(gameObject.transform.DOLocalMoveY(initYPos.y + 100f, 0.25f));
            s.Insert(0, gameObject.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.25f));
        }
    }

    public void Shrink()
    {
        if (!GameManager.Instance.dragging)
        {
            Sequence s = DOTween.Sequence();
            s.Append(gameObject.transform.DOLocalMoveY(initYPos.y, 0.25f));
            s.Insert(0, gameObject.transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.25f));
        }
    }
}
