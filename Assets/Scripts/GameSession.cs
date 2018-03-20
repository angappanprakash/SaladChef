﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class GameSession
{
#region Variables
	public const int				MAX_LOCAL_PLAYER_COUNT = 2;

	private List<PlayerData> 		m_ActivePlayers;
#endregion

#region Properties
	public List<PlayerData>	ActivePlayers
	{
		get { return m_ActivePlayers; }
	}

	public void ClearActivePlayers()
	{
		m_ActivePlayers.Clear ();
	}

	#endregion

	#region Class specific functions
	public GameSession()
	{
		m_ActivePlayers = new List<PlayerData>();

		for(int index = 0;index <= 1; index++)
		{
			CreatePlayerData(index);
		}
	}

	public void CreatePlayerData(int index)
	{
		if(m_ActivePlayers.Count == MAX_LOCAL_PLAYER_COUNT)
			return;

		PlayerIndex playerIndex;

		//TEMP FIX
		if(index == 1)
			playerIndex = PlayerIndex.PLAYER1;
		else
			playerIndex = PlayerIndex.PLAYER2;
		
		PlayerData newPlayer = new PlayerData();
		newPlayer._playerIndex = playerIndex;
		newPlayer._name = playerIndex.ToString();

		m_ActivePlayers.Add(newPlayer);
	}

	public PlayerData GetPlayerData(PlayerIndex playerIndex)
	{
		return m_ActivePlayers.Find(x => x._playerIndex == playerIndex);
	}

	#endregion
}
