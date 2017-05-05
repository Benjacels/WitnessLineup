using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Listener : CaptainsMessListener {

	public enum NetworkState
	{
		Init,
		Offline,
		Connecting,
		Connected,
		Disrupted
	};
	[HideInInspector]
	public NetworkState CurrentNetworkState = NetworkState.Init;
	public Text NetworkStateField;

	public GameObject GameSessionPrefab;
	public GameSession CurrentGameSession;

	public void Start()
	{
		CurrentNetworkState = NetworkState.Offline;

		ClientScene.RegisterPrefab(GameSessionPrefab);
	}

	public override void OnStartConnecting()
	{
		CurrentNetworkState = NetworkState.Connecting;
	}

	public override void OnStopConnecting()
	{
		CurrentNetworkState = NetworkState.Offline;
	}

	public override void OnServerCreated()
	{
		// Create game session
		GameSession oldSession = FindObjectOfType<GameSession>();
		if (oldSession == null)
		{
			GameObject serverSession = Instantiate(GameSessionPrefab);
			NetworkServer.Spawn(serverSession);
		}
		else
		{
			Debug.LogError("GameSession already exists!");
		}
	}

	public override void OnJoinedLobby()
	{
		CurrentNetworkState = NetworkState.Connected;

		CurrentGameSession = FindObjectOfType<GameSession>();
		if (CurrentGameSession)
		{
			CurrentGameSession.OnJoinedLobby();
		}
	}

	public override void OnLeftLobby()
	{
		CurrentNetworkState = NetworkState.Offline;

		CurrentGameSession.OnLeftLobby();
	}

	public override void OnCountdownStarted()
	{
		CurrentGameSession.OnCountdownStarted();
	}

	public override void OnCountdownCancelled()
	{
		CurrentGameSession.OnCountdownCancelled();
	}

	public override void OnStartGame(List<CaptainsMessPlayer> aStartingPlayers)
	{
		Debug.Log("GO!");
		CurrentGameSession.OnStartGame(aStartingPlayers);
	}

	public override void OnAbortGame()
	{
		Debug.Log("ABORT!");
		CurrentGameSession.OnAbortGame();
	}

	void Update()
	{
		NetworkStateField.text = CurrentNetworkState.ToString();
	}
}
