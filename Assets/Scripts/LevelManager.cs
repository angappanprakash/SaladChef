using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public enum GameState
{
	STARTING,
	IN_PROGRESS,
	PAUSED,
	ENDED
}

public class LevelManager : MonoBehaviour
{
#region Variables
	[SerializeField]
	private Transform[] 			m_SpawnMarkers;
	[SerializeField]
	private CameraControl           m_CameraControl;
	private List<PlayerController> 	m_PlayersList;
	private float					m_GameTimer;
	private GameState				m_CurrentGameState;
	private static LevelManager 	m_Instance;
	private CharacterCommonData		m_CharacterCommonData;
	private int                     m_NextSpawnMarkerIndex;
	private Transform               m_SpawnMarkersParent;

	private const string	DATA_ASSETS_PATH = "CharacterAssets/CharacterCommonData";
#endregion

#region Properties
	public static LevelManager Instance
	{
		get { return m_Instance; }
	}

	public List<PlayerController> PlayersList
	{
		get { return m_PlayersList; }
	}

	public float GameTimer
	{
		get { return m_GameTimer; }
	}

	public GameState CurrentGameState
	{
		get { return m_CurrentGameState; }
	}

	public CharacterCommonData CharacterCommonData
	{
		get { return m_CharacterCommonData;}
	}

	public Transform SpawnMarkersParent
	{
		get { return m_SpawnMarkersParent; }
	}
#endregion

#region Monobehaviour functions
	private void Awake()
	{
		m_Instance = this;
		m_NextSpawnMarkerIndex = -1;
		m_CharacterCommonData = (CharacterCommonData)  Resources.Load (DATA_ASSETS_PATH);

		GameObject markerParentObj = GameObject.Find("SpawnMarkers");
		if(markerParentObj != null)
			m_SpawnMarkersParent = markerParentObj.transform;
		StartGame();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if(m_CurrentGameState == GameState.IN_PROGRESS)
		{
			if(m_GameTimer > 0)
			{
				m_GameTimer = Mathf.Max(0, m_GameTimer - Time.deltaTime);
				if(m_GameTimer == 0)
					EndGame();
			}
		}
	}

	private void OnDestroy()
	{
		m_Instance = null;
	}
#endregion

#region Class specific functions
	private void StartGame()
	{
		Time.timeScale = 1;

		m_PlayersList = new List<PlayerController>();
		foreach (PlayerData playerData in GameManager.Instance.CurrentGameSession.ActivePlayers)
		{
			PlayerController playerController;
			playerData._characterName = CharacterDataLoader.pCharactersList[GetNextSpawnMarkerIndex()]._name;
			playerController = CreatePlayerInstance(playerData, m_SpawnMarkers[GetNextSpawnMarkerIndex()]);
			Debug.Log("spawn marker index:"+ GetNextSpawnMarkerIndex());

			m_PlayersList.Add(playerController);
		}

		SetCameraTargets();

		m_CurrentGameState = GameState.IN_PROGRESS;

		GameManager.Instance.GameEventSystem.TriggerEvent(GameEventsList.PlayerEvents.GAME_START, new GameStartEventArgs());
	}

	private int GetNextSpawnMarkerIndex()
	{
		return ((++m_NextSpawnMarkerIndex) % m_SpawnMarkers.Length);
	}

	private PlayerController CreatePlayerInstance(PlayerIndex playerIndex, Transform spawnMarker = null)
	{
		PlayerData playerData = GameManager.Instance.CurrentGameSession.GetPlayerData(playerIndex);
		return CreatePlayerInstance(playerData, spawnMarker);
	}

	private PlayerController CreatePlayerInstance(PlayerData playerData, Transform spawnMarker = null)
	{
		CharacterData characterData = CharacterDataLoader.GetData(playerData._characterName);
		GameObject playerObj = Instantiate(characterData._prefab) as GameObject;
		playerObj.name = playerData._name;
		if(spawnMarker != null)
		{
			playerObj.transform.position = spawnMarker.position;
			playerObj.transform.rotation = spawnMarker.rotation;
		}
		PlayerController playerController = playerObj.GetComponent<PlayerController>();
		playerController.Init(playerData._playerIndex);

		return playerController;
	}

	public PlayerController GetPlayer(PlayerIndex playerIndex)
	{
		return m_PlayersList.Find(x => x.PlayerIndex == playerIndex);
	}

	public void EndGame()
	{
		m_CurrentGameState = GameState.ENDED;
		GameManager.Instance.GameEventSystem.TriggerEvent(GameEventsList.PlayerEvents.GAME_END, new GameEndEventArgs());
	}

	private void SetCameraTargets()
	{
		List<Transform> targets = new List<Transform>();
		for (int i = 0; i < m_PlayersList.Count; i++)
		{
			targets.Add(m_PlayersList[i].transform);
		}

		m_CameraControl.Init(targets);
	}

	public void PauseGame(PlayerIndex playerIndex)
	{
		GameManager.Instance.GameEventSystem.TriggerEvent(GameEventsList.PlayerEvents.GAME_PAUSED, new GamePausedEventArgs());

		Time.timeScale = 0;
		m_CurrentGameState = GameState.PAUSED;
		ShowPauseMenu(playerIndex);
	}

	public void ResumeGame()
	{
		Time.timeScale = 1;
		m_CurrentGameState = GameState.IN_PROGRESS;

		GameManager.Instance.GameEventSystem.TriggerEvent(GameEventsList.PlayerEvents.GAME_RESUMED, new GameResumedEventArgs());
	}

	public void ResetCurrentLevel()
	{
		SceneManager.LoadScene("Game");
	}

	public void ShowPauseMenu(PlayerIndex playerIndex)
	{
	}

	public void SetGameDuration(float duration)
	{
		m_GameTimer = duration;
	}

	private IEnumerator ShowGameEndAfterDelay()
	{
		yield return new WaitForSeconds(2.5f);

		SceneManager.LoadScene("ResultScreen");
	}

	public void LoadMainMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("MainMenu");
	}
#endregion
}