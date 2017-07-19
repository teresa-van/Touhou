using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Room item.
/// 
/// It simply acts as a broker to populate its UI content with the right RoomInfo data.
/// It doesn't automatically update and relies on the RoomrListUIController to call RefreshData()
/// </summary>
public class RoomItem : MonoBehaviour {

	public Text RoomNameText;
	public Text PlayersCountText;
	public GameObject ClosedPanel;
	public GameObject JoinPanel;

	public float AnimationSpeed = 5f;

	LayoutElement LayoutElement;
	CanvasGroup CanvasGroup;

	float preferredHeight;
	float animatedTargetHeight;
	float animatedTargetAlpha;

	RoomInfo RoomCache;
    RoomListUIManager Controller;

	void Awake()
	{
		if (LayoutElement ==null)
		{
			LayoutElement = GetComponent<LayoutElement>();
			preferredHeight = LayoutElement.preferredHeight;

			CanvasGroup = GetComponent<CanvasGroup>();
		}
	}

	public void SetController(RoomListUIManager controller)
	{
		Controller = controller;
	}

	public void RefreshData(RoomInfo room)
	{

		RoomCache = room;
		RoomNameText.text = room.Name;
		PlayersCountText.text = room.PlayerCount + "/" + room.MaxPlayers;

		ClosedPanel.SetActive(!room.IsOpen);
		JoinPanel.SetActive(room.IsOpen);
	}

	public void JoinRoomButtonClick()
	{
		Controller.JoinRoom(RoomCache);
	}

	/// <summary>
	/// Animates the item to reveal it. 
	/// Note that is make sure the parent is active, else coroutine is nto allowed and will fire error
	/// This can happen if the UI List GameObject is disabled and PlayerListUIController is calling AnimateRevealItem() 
	/// </summary>
	public void AnimateRevealItem()
	{
		if (this.transform.parent.gameObject.activeInHierarchy)
		{
			StartCoroutine(AnimateRevealItem_cr());
		}

	}

	IEnumerator AnimateRevealItem_cr()
	{
		if (LayoutElement == null) Awake();
		
		animatedTargetHeight = preferredHeight;

		LayoutElement.preferredHeight = 0;
		CanvasGroup.alpha = 0f;

		yield return new WaitForEndOfFrame();

		while(true)
		{
			LayoutElement.preferredHeight = Mathf.Lerp(LayoutElement.preferredHeight, animatedTargetHeight, AnimationSpeed * Time.deltaTime);

			CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, 1, AnimationSpeed * Time.deltaTime);

			yield return new WaitForEndOfFrame();

			if (LayoutElement.preferredHeight>=animatedTargetHeight) break;
		}
	}

	/// <summary>
	/// Animates the remove item. 
	/// Note that is make sure the parent is active, else coroutine is nto allowed and will fire error
	/// This can happen if the UI List GameObject is disabled and PlayerListUIController is calling AnimateRemoveItem()
	/// The GameObject will be destroy at the end of the animation to prevent leakage. Prefer a Pooling solution if your number of players will be high.
	/// </summary>
	public void AnimateRemoveItem()
	{
		if (this.transform.parent.gameObject.activeInHierarchy)
		{
			StartCoroutine(AnimateRemoveItem_cr());
		}
	}

	IEnumerator AnimateRemoveItem_cr()
	{
		if (LayoutElement == null) Awake();
			
		animatedTargetHeight = 0;

		yield return new WaitForEndOfFrame();
			
		while(true)
		{
			LayoutElement.preferredHeight = Mathf.Lerp(LayoutElement.preferredHeight, animatedTargetHeight, 5f * Time.deltaTime);
			CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, 0, AnimationSpeed * Time.deltaTime);

			yield return new WaitForEndOfFrame();
				
			if (LayoutElement.preferredHeight <= animatedTargetHeight) break;
		}

		Destroy(this.gameObject);
	}

}
