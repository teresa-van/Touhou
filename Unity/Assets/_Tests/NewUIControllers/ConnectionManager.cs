using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.Collections;

namespace ExitGames.Demos.UI
{
	/// <summary>
	/// Simple Connection Manager.
	/// Deals with toggle UI menu and player list based on photon status.
	/// </summary>
	public class ConnectionManager : MonoBehaviour {

		public GameObject MenuUI;
		public GameObject RoomUI;
		public GameObject LobbyUI;

		public Text ConnectionStatusText;

		ClientState _clientStateCache;

		public static ConnectionManager Instance;

		public string PlayerName
		{
			get
			{
				return PhotonNetwork.playerName;
			}
			set
			{
				PhotonNetwork.playerName = value;
			}
		}

		void Awake()
		{
			Instance = this;
		}

		public void Start()
		{
			PhotonNetwork.autoJoinLobby = true;
			MenuUI.SetActive(true);
			RoomUI.SetActive(false);
			LobbyUI.SetActive(false);
		}

		void Update()
		{
			if (_clientStateCache != PhotonNetwork.connectionStateDetailed)
			{
				_clientStateCache = PhotonNetwork.connectionStateDetailed;
				ConnectionStatusText.text = _clientStateCache.ToString();
			}
		}


		public void JoinLobby () {
			
			// Unity UI hack to catch TextField submition. 
			// the player name TextField OnEndEdit calls Connect(), but pressing Esc also means ending the edit, so we catch the esc key and don't proceed.
			if( Input.GetKeyDown( KeyCode.Escape ) ) 
			{
				return;
			}
			
			PhotonNetwork.ConnectUsingSettings("1.0");
			
			MenuUI.SetActive(false);
		}

		public void JoinRoom(RoomInfo room)
		{
			PhotonNetwork.JoinRoom(room.name);
		}

		public void JoinRandomRoom()
		{
			PhotonNetwork.JoinRandomRoom();
		}


		public void CreateRoom()
		{
			PhotonNetwork.CreateRoom(PhotonNetwork.playerName+"'s Room", new RoomOptions() { MaxPlayers = 4 }, null);
		}

		public void LeaveRoom(){
			PhotonNetwork.LeaveRoom();
		}

		public virtual void OnLeftRoom()
		{
			MenuUI.SetActive(false);
			RoomUI.SetActive(false);
		}

		public virtual void OnJoinedRoom()
		{
			MenuUI.SetActive(false);
			RoomUI.SetActive(true);

		}

		public virtual void OnJoinedLobby()
		{
			LobbyUI.SetActive(true);
		}
		
		public virtual void OnLeftLobby()
		{
			LobbyUI.SetActive(false);
		}
		
		public virtual void OnPhotonRandomJoinFailed()
		{
			PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
		}
		
		// the following methods are implemented to give you some context. re-implement them as needed.
		
		public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
		{
			Debug.LogError("Cause: " + cause);
		}



	}
}


