using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameData
{
	public class PlayerData
	{
		public PlayerIndex	_playerIndex;
		public string		_name;
		public string 		_characterName;
		public PlayerColor	_playerColor;
	}

	public enum PlayerIndex
	{
		PLAYER1 = 0,
		PLAYER2,
		INVALID = -1
	}

	[System.Serializable]
	public enum PlayerColor
	{
        BLUE = 0,
        RED,
		NONE = -1
	}

	public enum PlayerState
	{
		SPAWN = 0,
		IDLE,
		RUNNING,
		INTERACTION,
		CHOPPING,
		FRUIT_EFFECT,
		NONE = -1
    }
}
