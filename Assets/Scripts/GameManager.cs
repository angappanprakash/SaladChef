using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
#region Variables
	public const int MAX_LOCAL_PLAYER_COUNT = 2;

	private static GameManager m_Instance;
#endregion

#region Properties
	public static GameManager Instance
	{
		get { return m_Instance; }
	}
#endregion

#region Monobehaviour functions
	private void Awake()
	{
		m_Instance = this;
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