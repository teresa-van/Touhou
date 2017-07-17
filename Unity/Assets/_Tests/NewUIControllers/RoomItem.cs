using System;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;

namespace ExitGames.Demos.UI
{
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

		LayoutElement _layoutElement;
		CanvasGroup _canvasGroup;

		float preferredHeight;
		float animatedTargetHeight;
		float animatedTargetAlpha;

		RoomInfo _roomCache;
		RoomListUIController Controller;

		void awake()
		{
			if (_layoutElement ==null)
			{
				_layoutElement = GetComponent<LayoutElement>();
				preferredHeight = _layoutElement.preferredHeight;

				_canvasGroup = GetComponent<CanvasGroup>();
			}
		}

		public void SetController(RoomListUIController _controller)
		{
			Controller = _controller;
		}

		public void RefreshData(RoomInfo room)
		{

			_roomCache = room;
			RoomNameText.text = room.name;
			PlayersCountText.text = room.playerCount+"/"+room.maxPlayers;

			ClosedPanel.SetActive(!room.open);
			JoinPanel.SetActive(room.open);
		}

		public void JoinRoomButtonClick()
		{
			Controller.JoinRoom(_roomCache);
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
			if (_layoutElement ==null) awake();
		
			animatedTargetHeight = preferredHeight;

			_layoutElement.preferredHeight = 0;
			_canvasGroup.alpha = 0f;

			yield return new WaitForEndOfFrame();

			while(true)
			{
				_layoutElement.preferredHeight = Mathf.Lerp(_layoutElement.preferredHeight,animatedTargetHeight,AnimationSpeed*Time.deltaTime);

				_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha,1,AnimationSpeed*Time.deltaTime);

				yield return new WaitForEndOfFrame();

				if (_layoutElement.preferredHeight>=animatedTargetHeight) break;
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
			if (_layoutElement ==null) awake();
			
			animatedTargetHeight = 0;

			yield return new WaitForEndOfFrame();
			
			while(true)
			{
				_layoutElement.preferredHeight = Mathf.Lerp(_layoutElement.preferredHeight,animatedTargetHeight,5f*Time.deltaTime);
				_canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha,0,AnimationSpeed*Time.deltaTime);

				yield return new WaitForEndOfFrame();
				
				if (_layoutElement.preferredHeight<=animatedTargetHeight) break;
			}

			Destroy(this.gameObject);
		}

	}
}