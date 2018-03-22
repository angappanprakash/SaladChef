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
	private const string	DATA_ASSETS_PATH = "CharacterAssets/CharacterCommonData";
	private const string	VEGETABLE_ASSETS_PATH = "Vegetables";
	private const string	SALAD_ASSETS_PATH = "Salads";

	[SerializeField]
	private Transform[] 			m_PlayerSpawnMarkers;
	[SerializeField]
	private GameObject				m_PauseMenu;
	[SerializeField]
	private CameraControl           m_CameraControl;
	private List<PlayerController> 	m_PlayersList;
	private List<PowerUp>			m_PowerUpList;
	private float					m_GameTimer;
	private GameState				m_CurrentGameState;
	private static LevelManager 	m_Instance;
	private CharacterCommonData		m_CharacterCommonData;
	private int                     m_NextSpawnMarkerIndex;
	private Transform               m_SpawnMarkersParent;

#endregion

#region Properties
	public static LevelManager Instance
	{
		get { return m_Instance; }
	}

	public List<PlayerController> pPlayersList
	{
		get { return m_PlayersList; }
	}

	public List<PowerUp> pPowerUpsList
	{
		get { return m_PowerUpList; }
	}

	public float pGameTimer
	{
		get { return m_GameTimer; }
	}

	public GameState pCurrentGameState
	{
		get { return m_CurrentGameState; }
	}

	public CharacterCommonData pCharacterCommonData
	{
		get { return m_CharacterCommonData;}
	}

	public Transform pSpawnMarkersParent
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
		ShowPauseMenu(false);
		InvokeRepeating ("Countdown", 1.0f, 1.0f);
	}

	private void Update()
	{
		if(m_CurrentGameState == GameState.IN_PROGRESS)
		{
//			if(m_GameTimer > 0)
//			{
//				m_GameTimer = Mathf.Max(0, m_GameTimer - Time.deltaTime);
//				if(m_GameTimer == 0)
//					EndGame();
//			}

			if(m_GameTimer == 0)
				EndGame();
//
//			if(GameManager.Instance.pCurrentGameSession.GetPlayerData(PlayerIndex.PLAYER1)._timer == 0.0f && GameManager.Instance.pCurrentGameSession.GetPlayerData(PlayerIndex.PLAYER2)._timer == 0.0f)
//			{
//				EndGame();
//			}
		}
		if(Input.GetKeyDown(KeyCode.Escape) && !m_PauseMenu.gameObject.activeSelf)
		{
			ShowPauseMenu(true);
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
		SetGameDuration(GameManager.Instance.pGameTimer);

		m_PlayersList = new List<PlayerController>();
		foreach (PlayerData playerData in GameManager.Instance.pCurrentGameSession.pActivePlayers)
		{
			PlayerController playerController;
			playerController = CreatePlayerInstance(playerData, m_PlayerSpawnMarkers[GetNextSpawnMarkerIndex()]);
			//Debug.Log("spawn marker index:"+ m_SpawnMarkers[GetNextSpawnMarkerIndex()].name);

			m_PlayersList.Add(playerController);
		}

		SetCameraTargets();

		m_CurrentGameState = GameState.IN_PROGRESS;

		GameManager.Instance.pGameEventSystem.TriggerEvent(GameEventsList.PlayerEvents.GAME_START, new GameStartEventArgs());
	}

	private int GetNextSpawnMarkerIndex()
	{
		return ((++m_NextSpawnMarkerIndex) % m_PlayerSpawnMarkers.Length);
	}

	private PlayerController CreatePlayerInstance(PlayerData playerData, Transform spawnMarker = null)
	{
		CharacterData characterData = CharacterDataLoader.GetData(playerData._characterName);
		GameObject playerObj = Instantiate(characterData._prefab) as GameObject;
		playerObj.name = playerData._name;
		float timer = characterData._timer;

		if(spawnMarker != null)
		{
			playerObj.transform.position = spawnMarker.position;
			playerObj.transform.rotation = spawnMarker.rotation;
		}
		PlayerController playerController = playerObj.GetComponent<PlayerController>();
		playerController.pTimer = timer;
		playerController.Init(playerData);

		return playerController;
	}

	public PlayerController GetPlayer(PlayerIndex playerIndex)
	{
		return m_PlayersList.Find(x => x.pPlayerIndex == playerIndex);
	}

	public Vegetable CreateVegetable(VegetableDispenserType dispenserType)
	{
		GameObject go = (GameObject)Instantiate(Resources.Load(VEGETABLE_ASSETS_PATH + "/" + dispenserType.ToString()));
		return go.GetComponent<Vegetable>();
	}

	public Vegetable CreateVegetable(VegetableType veg)
	{
		GameObject go = (GameObject)Instantiate(Resources.Load(VEGETABLE_ASSETS_PATH + "/" + veg.ToString()));
		return go.GetComponent<Vegetable>();
	}

	public Salad CreateSalad(SaladType saladType)
	{
		GameObject go = (GameObject)Instantiate(Resources.Load(SALAD_ASSETS_PATH + "/" + saladType.ToString()));
		return go.GetComponent<Salad>();
	}

	public void EndGame()
	{
		m_CurrentGameState = GameState.ENDED;
		GameManager.Instance.pGameEventSystem.TriggerEvent(GameEventsList.PlayerEvents.GAME_END, new GameEndEventArgs());
		SceneManager.LoadScene("ResultScreen");
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

	public void ShowPauseMenu(bool flag)
	{
		if(flag)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
		m_PauseMenu.gameObject.SetActive(flag);
	}

	public void SetGameDuration(float duration)
	{
		m_GameTimer = duration;
	}

	public void LoadMainMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("MainMenu");
	}

	private void Countdown () 
	{
		//Debug.Log("timer count down:" + m_GameTimer);
		if (--m_GameTimer == 0)
			CancelInvoke ("Countdown");
	}

#endregion
}