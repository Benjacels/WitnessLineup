using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;

public enum GameStateEnum
{
	Offline,
	Connecting,
	Lobby,
	Countdown,
	Playing
}

public class GameSession : NetworkBehaviour {

	public Text gameStateField;
	public Text gameRulesField;

	public static GameSession instance;

	Listener networkListener;
	List<Player> players;
	string specialMessage = "";

	[SyncVar]
	public GameStateEnum gameState;

	[SyncVar]
	public string message = "";

	[Server]
	public override void OnStartServer()
	{
		networkListener = FindObjectOfType<Listener>();
		gameState = GameStateEnum.Connecting;
	}

	[Server]
	public void OnStartGame(List<CaptainsMessPlayer> aStartingPlayers)
	{
		players = aStartingPlayers.Select(p => p as Player).ToList();

		RpcOnStartedGame();
		foreach (Player p in players)
		{
			p.RpcOnStartedGame();
		}

		StartCoroutine(RunGame());
	}

	[Server]
	IEnumerator RunGame()
	{
		yield return null;
	}

	[Server]
	public void OnAbortGame()
	{
		RpcOnAbortedGame();
	}

	[Client]
	public override void OnStartClient()
	{
		if (instance)
		{
			Debug.LogError("ERROR: Another GameSession!");
		}
		instance = this;

		networkListener = FindObjectOfType<Listener>();
		networkListener.CurrentGameSession = this;

		if (gameState != GameStateEnum.Lobby)
		{
			gameState = GameStateEnum.Lobby;
		}
	}

	public void OnJoinedLobby()
	{
		gameState = GameStateEnum.Lobby;
	}

	public void OnLeftLobby()
	{
		gameState = GameStateEnum.Offline;
	}

	public void OnCountdownStarted()
	{
		gameState = GameStateEnum.Countdown;
	}

	public void OnCountdownCancelled()
	{
		gameState = GameStateEnum.Lobby;
	}

	void Update()
	{
		if (isServer)
		{
			if (gameState == GameStateEnum.Countdown)
			{
				message = "Game Starting in " + Mathf.Ceil(networkListener.mess.CountdownTimer()) + "...";
			}
			else if (specialMessage != "")
			{
				message = specialMessage;
			}
			else
			{
				message = gameState.ToString();
			}
		}

		gameStateField.text = message;
	}

	// Client RPCs

	[ClientRpc]
	public void RpcOnStartedGame()
	{
		
	}

	[ClientRpc]
	public void RpcOnAbortedGame()
	{
		
	}

}
