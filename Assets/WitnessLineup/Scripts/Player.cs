﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum PlayerTypeEnum
{
	Officer,
	Witness
}

public class Player : CaptainsMessPlayer {

	public Image PlayerImage;
	public Text NameField;
	public Text ReadyField;

	PlayerTypeEnum _playerType;

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();

		if (playerIndex == 0)
			_playerType = PlayerTypeEnum.Officer;
		else
		{
			Camera.main.orthographicSize /= 5;
			_playerType = PlayerTypeEnum.Witness;
		}
	}

	public override void OnClientEnterLobby()
	{
		base.OnClientEnterLobby();

		// Brief delay to let SyncVars propagate
		Invoke("ShowPlayer", 0.5f);
	}

	public override void OnClientReady(bool readyState)
	{
		if (readyState)
		{
			ReadyField.text = "READY!";
			ReadyField.color = Color.green;
		}
		else
		{
			ReadyField.text = "not ready";
			ReadyField.color = Color.red;
		}
	}

	void ShowPlayer()
	{
		//transform.SetParent(GameObject.Find("Canvas/PlayerContainer").transform, false);

		NameField.text = deviceName;
		ReadyField.gameObject.SetActive(true);

		OnClientReady(IsReady());
	}

	public void Update()
	{

	}

	[ClientRpc]
	public void RpcOnStartedGame()
	{
		ReadyField.gameObject.SetActive(false);
		Invoke("SetupGame", 0.5f);
	}

	private void SetupGame()
	{
		Transform soundButtonTran = GameObject.FindWithTag("SoundButtons").transform;

		foreach (Transform tran in soundButtonTran)
			tran.gameObject.SetActive(true);

		if(_playerType == PlayerTypeEnum.Officer)
		{
			
		}
		else if(_playerType == PlayerTypeEnum.Witness)
		{
			
		}
	}

	void OnGUI()
	{
		if (isLocalPlayer)
		{
			GUILayout.BeginArea(new Rect(0, Screen.height * 0.8f, Screen.width, 100));
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			GameSession gameSession = GameSession.instance;
			if (gameSession)
			{
				if (gameSession.gameState == GameStateEnum.Lobby ||
					gameSession.gameState == GameStateEnum.Countdown)
				{
					if (GUILayout.Button(IsReady() ? "Not ready" : "Ready", GUILayout.Width(Screen.width * 0.3f), GUILayout.Height(100)))
					{
						if (IsReady())
						{
							SendNotReadyToBeginMessage();
						}
						else
						{
							SendReadyToBeginMessage();
						}
					}
				}
				//else if (gameSession.gameState == GameState.GameOver)
				//{
				//	if (isServer)
				//	{
				//		if (GUILayout.Button("Play Again", GUILayout.Width(Screen.width * 0.3f), GUILayout.Height(100)))
				//		{
				//			CmdPlayAgain();
				//		}
				//	}
				//}
			}

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}
}