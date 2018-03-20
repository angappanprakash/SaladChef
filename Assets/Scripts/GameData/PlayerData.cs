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
		public TeamType		_teamType;
		public PlayerColor	_playerColor;
	}

    public enum TeamType
    {
        NONE,
        RED,
        BLUE
    }
	public enum PlayerIndex
	{
		INVALID = -1,
		PLAYER1,
		PLAYER2
	}

	[System.Serializable]
	public enum PlayerColor
	{
        NONE = -1,
        BLUE,
        RED
	}

	public enum PlayerState
	{
		NONE = -1,
		SPAWN,
		IDLE,
		RUNNING,
		FRUIT_EFFECT,
    }
}
