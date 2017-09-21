using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider))]
public class CardVisuals : MonoBehaviour {

    public Vector3 initPos;
    Vector3 screenPoint;
    Vector3 offset;
    float lockedYPosition;

	// Use this for initialization
	void Start () {
        initPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnMouseDown()
    {
        GameManager.Instance.dragging = true;
        lockedYPosition = screenPoint.y;
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        Cursor.visible = false;
    }

    public void OnMouseDrag()
    {
        Sequence s = DOTween.Sequence();
        s.Insert(0, transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.15f));

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        //curPosition.x = lockedYPosition;
        transform.position = curPosition;
    }

    public void OnMouseUp()
    {
        int leftX;
        int rightX;
        float newXPos = transform.localPosition.x;
        Vector3 moveToPosition = Vector3.zero; 
        List<GameObject> cardsToShift = new List<GameObject>();

        if (GameManager.Instance.HandParent.transform.childCount % 2 == 0)leftX = -45 * (GameManager.Instance.HandParent.transform.childCount - 1);
        else leftX = -90 * ((GameManager.Instance.HandParent.transform.childCount - 1) / 2);
        rightX = leftX + ((GameManager.Instance.HandParent.transform.childCount - 1) * 90);

        if (Mathf.Abs(newXPos - initPos.x) <= 90) moveToPosition = initPos;
        else
        {
            string direction = "";
            if (newXPos < initPos.x) direction = "left";
            else direction = "right";
            foreach (Transform card in GameManager.Instance.HandParent.transform)
                if ((direction.Equals("left") && card.localPosition.x > newXPos && card.localPosition.x < initPos.x) ||
                    (direction.Equals("right") && card.localPosition.x < newXPos && card.localPosition.x > initPos.x)) cardsToShift.Add(card.gameObject);

            if (newXPos < leftX) moveToPosition = new Vector3(leftX, initPos.y, initPos.z);
            else if (newXPos > rightX) moveToPosition = new Vector3(rightX, initPos.y, initPos.z);
            else
            {
                for (int i = leftX; i <= rightX; i += 90)
                {
                    if (i < newXPos)
                    {
                        if (direction.Equals("left")) moveToPosition = new Vector3(i + 90, initPos.y, initPos.z);
                        else moveToPosition = new Vector3(i, initPos.y, initPos.z);
                    }
                }
            }

            if (direction.Equals("left")) GameManager.Instance.ShiftCards(90f, cardsToShift);
            else GameManager.Instance.ShiftCards(-90f, cardsToShift);
        }

        Sequence s = DOTween.Sequence();
        s.Append(transform.DOLocalMove(moveToPosition, 0.25f));
        Cursor.visible = true;
        s.OnComplete(() =>
        {
            initPos = moveToPosition;
            GameManager.Instance.dragging = false;
        });
    }

    public void BlowUp()
    {
        if (!GameManager.Instance.dragging)
        {
            transform.parent.SetAsLastSibling();
            transform.SetAsLastSibling();
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOLocalMoveY(initPos.y + 100f, 0.25f));
            s.Insert(0, transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.25f));
        }
    }

    public void Shrink()
    {
        if (!GameManager.Instance.dragging)
        {
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOLocalMoveY(initPos.y, 0.25f));
            s.Insert(0, transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.25f));
        }
    }
}
