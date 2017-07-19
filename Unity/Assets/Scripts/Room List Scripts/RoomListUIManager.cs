using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ExitGames.Client.Photon;

/// <summary>
/// Room list UI manager.
/// This is a good starting point to handle efficient UI management for listing Rooms in lobby.
/// It only updates when required
/// For long lists, be aware that items creation isn't optimal, as for every creation of GameObject, consider using pooling systems or dedicated framework for efficient long lists like https://www.assetstore.unity3d.com/en/#!/content/36378
/// 
/// Note that the typical setup should be that this controller sits on a GameObject that is never disabled, and the UI element can be turned off at will. This is important for efficient UI rendering to keep the minimum amount of UI on screen
/// </summary>
public class RoomListUIManager: Photon.PunBehaviour {

	public GameObject RoomPanel;
	public GameObject RoomItemPrefab;

	Dictionary<string, RoomItem> items = new Dictionary<string, RoomItem>();


	/// <summary>
	/// Updates the UI listing, it creates the necessary items not yet listed, update existing items and remove unused entries
	/// </summary>
	public void UpdateUI()
	{
		List<string> processedIDS = new List<string>();

		// update existing items and add new items
		foreach(RoomInfo info in PhotonNetwork.GetRoomList())
		{
			Debug.Log(info.ToStringFull());

			if (items.ContainsKey(info.Name)) // update
			{
				items[info.Name].RefreshData(info);
				processedIDS.Add(info.Name);

			}
            else
            {   // create new
				GameObject item =  (GameObject)Instantiate(RoomItemPrefab);
				item.transform.SetParent(RoomPanel.transform);

				RoomItem roomItem = item.GetComponent<RoomItem>();
				items.Add(info.Name,roomItem);
				roomItem.SetController(this);
				roomItem.RefreshData(info);

				roomItem.AnimateRevealItem();

				processedIDS.Add(info.Name);
			}
		}

		// now deal with items that needs to be deleted.
		// work in reverse so that we can delete entries without worries.
		foreach(var item in items.Reverse())
		{
			if (!processedIDS.Contains(item.Key))
			{
				items[item.Key].AnimateRemoveItem();
				items.Remove(item.Key);
			}
		}

	}

	/// <summary>
	/// Cleans up list to prevent memory leak.
	/// </summary>
	public void CleanUpList()
	{
		items = new Dictionary<string, RoomItem>();
		foreach(Transform child in RoomPanel.transform) 
		{
			Destroy(child.gameObject);
		}
	}


	public void JoinRoom(RoomInfo room)
	{
		ServerManager.Instance.JoinRoom(room);
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
