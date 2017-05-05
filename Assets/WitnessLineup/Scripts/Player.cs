using UnityEngine;
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

	private int _suspectIndex;
	private int _suspectCount = 5;

	private GameObject _soundButtonsParent;
	private Button _talkButton;
	private Button _laughButton;
	private Button _walkButton;

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
		if(_playerType == PlayerTypeEnum.Officer)
		{
			
		}
		else if(_playerType == PlayerTypeEnum.Witness)
		{
			
		}

		_soundButtonsParent = GameObject.FindWithTag("SoundButtons");

		foreach (Transform buttonTran in _soundButtonsParent.transform)
			buttonTran.gameObject.SetActive(true);

		_talkButton = GameObject.FindWithTag("TalkButton").GetComponent<Button>();
		_laughButton = GameObject.FindWithTag("LaughButton").GetComponent<Button>();
		_walkButton = GameObject.FindWithTag("WalkButton").GetComponent<Button>();

		_talkButton.onClick.AddListener(TalkButtonPressed);
		_laughButton.onClick.AddListener(LaughButtonPressed);
		_walkButton.onClick.AddListener(WalkButtonPressed);
	}

	private void TalkButtonPressed()
	{
		
	}

	private void LaughButtonPressed()
	{

	}

	private void WalkButtonPressed()
	{

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
