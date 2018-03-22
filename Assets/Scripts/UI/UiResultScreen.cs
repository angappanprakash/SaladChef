using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameData;
using UnityEngine.SceneManagement;

public class UiResultScreen: MonoBehaviour 
{
	#region Variables
	[SerializeField]
	private Button                	m_BtnRestart;
	[SerializeField]
	private Button                	m_BtnMainMenu;

	[SerializeField]
	private Text               		m_TxtPlayer1_Score;
	[SerializeField]
	private Text               		m_TxtPlayer2_Score;

	#endregion

	#region Properties
	#endregion

	#region Monobehaviour functions
	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Start()
	{
		m_TxtPlayer1_Score.text = GameManager.Instance.pCurrentGameSession.GetPlayerData(PlayerIndex.PLAYER1)._score.ToString();
		m_TxtPlayer2_Score.text = GameManager.Instance.pCurrentGameSession.GetPlayerData(PlayerIndex.PLAYER2)._score.ToString();;
	}

	private void Update()
	{
	}
	#endregion

	#region Class specific functions
	public void OnClickRestart()
	{
		SceneManager.LoadScene("Game");
	}

	public void OnClickMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
	#endregion
}