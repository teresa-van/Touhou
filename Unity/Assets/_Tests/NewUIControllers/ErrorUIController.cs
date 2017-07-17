using System;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

using ExitGames.Client.Photon;

namespace ExitGames.Demos.UI
{

	public class ErrorUIController: Photon.PunBehaviour {

		public Canvas ModalCanvas;
		public Text MessageText;

		public static ErrorUIController Instance;

		void Awake()
		{
			ModalCanvas.gameObject.SetActive(false);
		}


		public void ShowError(string message)
		{
			MessageText.text = message;
			ModalCanvas.gameObject.SetActive(true);

		}

		public void DismissError()
		{
			ModalCanvas.gameObject.SetActive(false);
			ConnectionManager.Instance.Start();
		}



		#region PHOTON CALLBACKS
		public override void OnFailedToConnectToPhoton(DisconnectCause cause) // object[] codeAndMsg)
		{
			ShowError("Failed to Conect to Photon: "+Environment.NewLine+cause);
		}

		public override void OnConnectionFail(DisconnectCause cause)
		{
			ShowError("Failed to Conect to Photon: "+Environment.NewLine+cause);
		}

		#endregion
	}
}