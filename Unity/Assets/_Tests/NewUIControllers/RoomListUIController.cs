using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ExitGames.Client.Photon;

namespace ExitGames.Demos.UI
{
	/// <summary>
	/// Room list UI manager.
	/// This is a good starting point to handle efficient UI management for listing Rooms in lobby.
	/// It only updates when required
	/// For long lists, be aware that items creation isn't optimal, as for every creation of GameObject, consider using pooling systems or dedicated framework for efficient long lists like https://www.assetstore.unity3d.com/en/#!/content/36378
	/// 
	/// Note that the typical setup should be that this controller sits on a GameObject that is never disabled, and the UI element can be turned off at will. This is important for efficient UI rendering to keep the minimum amount of UI on screen
	/// </summary>
	public class RoomListUIController: Photon.PunBehaviour {

		public GameObject UIList;

		public GameObject RoomItemPrefab;

		Dictionary<string,RoomItem> _items = new Dictionary<string, RoomItem>();


		/// <summary>
		/// Updates the UI listing, it creates the necessary items not yet listed, update existing items and remove unused entries
		/// </summary>
		public void UpdateUI()
		{
			List<string> processedIDS = new List<string>();

			// update existing items and add new items
			foreach(RoomInfo _info in PhotonNetwork.GetRoomList())
			{
				Debug.Log(_info.ToStringFull());

				if (_items.ContainsKey(_info.name)) // update
				{
					_items[_info.name].RefreshData(_info);
					processedIDS.Add(_info.name);

				}else{ // create new
					GameObject _item =  (GameObject)Instantiate(RoomItemPrefab);
					_item.transform.SetParent(UIList.transform);

					 RoomItem _roomItem = _item.GetComponent<RoomItem>();
					_items.Add(_info.name,_roomItem);
					_roomItem.SetController(this);
					_roomItem.RefreshData(_info);

					_roomItem.AnimateRevealItem();

					processedIDS.Add(_info.name);
				}
			}

			// now deal with items that needs to be deleted.
			// work in reverse so that we can delete entries without worries.
			foreach(var _item in _items.Reverse())
			{
				if (!processedIDS.Contains(_item.Key))
				{
					_items[_item.Key].AnimateRemoveItem();
					_items.Remove(_item.Key);
				}
			}

		}

		/// <summary>
		/// Cleans up list to prevent memory leak.
		/// </summary>
		public void CleanUpList()
		{
			_items = new Dictionary<string, RoomItem>();
			foreach(Transform child in UIList.transform) 
			{
				Destroy(child.gameObject);
			}
		}


		public void JoinRoom(RoomInfo room)
		{
			ConnectionManager.Instance.JoinRoom(room);
		}

		#region PHOTON CALLBACKS
		public override void OnJoinedLobby()
		{
			Debug.Log("OnJoinedLobby");
			CleanUpList();
			UpdateUI();
		}
		
		public override void OnLeftLobby()
		{
			Debug.Log("OnLeftLobby");
			CleanUpList();
		}

		public override void OnReceivedRoomListUpdate()
		{
			Debug.Log("OnReceivedRoomListUpdate");
			UpdateUI();
		}

		#endregion
	}
}