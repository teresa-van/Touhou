using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ExitGames.Client.Photon;

/// <summary>
/// Player list UI manager.
/// This is a good starting point to handle efficient UI management for listing Photon Players.
/// It only updates when required
/// For long lists, be aware that items creation isn't optimal, as for every creation of GameObject, consider using pooling systems or dedicated framework for efficient long lists like https://www.assetstore.unity3d.com/en/#!/content/36378
/// 
/// Note that the typical setup should be that this controller sits on a GameObject that is never disabled, and the UI element can be turned off at will. This is important for efficient UI rendering to keep the minimum amount of UI on screen
/// </summary>
public class PlayerListUIManager: Photon.PunBehaviour {

	public GameObject PlayerPanel;

	public GameObject PlayerItemPrefab;

	Dictionary<int, PlayerItem> items = new Dictionary<int, PlayerItem>();


	/// <summary>
	/// Updates the UI listing, it creates the necessary items not yet listed, update existing items and remove unused entries
	/// </summary>
	public void UpdateUI()
	{
		List<int> processedIDS = new List<int>();

		// update existing items and add new items
		foreach(PhotonPlayer player in PhotonNetwork.playerList)
		{
			if (items.ContainsKey(player.ID)) // update
			{
				items[player.ID].RefreshData(player);
				processedIDS.Add(player.ID);
			}
            else
            {   // create new
				GameObject item =  (GameObject)Instantiate(PlayerItemPrefab);
				item.transform.SetParent(PlayerPanel.transform);

				PlayerItem playerItem = item.GetComponent<PlayerItem>();

                Debug.Log(playerItem);

                items.Add(player.ID, playerItem);
                playerItem.RefreshData(player);
				playerItem.AnimateRevealItem();
				processedIDS.Add(player.ID);
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
		items = new Dictionary<int, PlayerItem>();
		foreach(Transform child in PlayerPanel.transform) 
		{
			Destroy(child.gameObject);
		}
	}

	#region PHOTON CALLBACKS
	public override void OnJoinedRoom()
	{
		CleanUpList();
		UpdateUI();
	}
		
	public override void OnLeftRoom()
	{
		CleanUpList();
	}

	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		UpdateUI();
	}
		
	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		UpdateUI();
	}

	public override void OnMasterClientSwitched (PhotonPlayer newMasterClient)
	{
		UpdateUI();
	}

	#endregion
}
