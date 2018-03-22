using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GameData;

public class GameManager : MonoBehaviour
{
#region Variables
	public const int 			MAX_LOCAL_PLAYER_COUNT = 2;

	private static GameManager 	m_Instance;
	private GameSession 		m_CurrentGameSession;
	[SerializeField]
	private GameEventSystem 	m_GameEventSystem;
	private string 				m_CurrentLevelName;
	[SerializeField]
	private AudioManager        m_AudioManager;
	[SerializeField]
	private float				m_GameTimer;
#endregion

#region Properties
	public static GameManager Instance
	{
		get { return m_Instance; }
	}

	public GameSession pCurrentGameSession
	{
		get { return m_CurrentGameSession; }
	}

	public GameEventSystem pGameEventSystem
	{
		get { return m_GameEventSystem; }
	}

	public float pGameTimer
	{
		get { return m_GameTimer; }
	}
#endregion

#region Monobehaviour functions
	private void Awake()
	{
		m_Instance = this;
		m_AudioManager.Init();
		m_CurrentGameSession = new GameSession();
		m_AudioManager.PlayInGameBGM();
		CharacterDataLoader.Init();
		SceneManager.LoadScene("MainMenu");
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
		m_Instance = null;
	}
#endregion
}