using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class PlayerEventParams
{
	public PlayerIndex PlayerIndex;
	public PlayerController Player;
}

public class GameStartEventArgs : PlayerEventParams
{
	public GameStartEventArgs()
	{

	}
}

public class GamePausedEventArgs : PlayerEventParams
{
	public GamePausedEventArgs()
	{

	}
}

public class GameResumedEventArgs : PlayerEventParams
{
	public GameResumedEventArgs()
	{

	}
}

public class GameEndEventArgs : PlayerEventParams
{
	public GameEndEventArgs()
	{

	}
}

public class ScoreUpdateEventArgs : PlayerEventParams
{
	public int NewScore;
	public ScoreUpdateEventArgs(PlayerIndex playerIndex, int newScore)
	{
		PlayerIndex = playerIndex;
		NewScore = newScore;
	}
}

public class OnCollectVegetableEventArgs : PlayerEventParams
{
	public Vegetable vegetable;
	public PlayerController playerController;

	public OnCollectVegetableEventArgs(PlayerController playerCtrl, Vegetable veg)
	{
		playerController = playerCtrl;
		vegetable = veg;
	}
}

public class OnRemoveVegetableEventArgs : PlayerEventParams
{
	public Vegetable vegetable;
	public PlayerController playerController;

	public OnRemoveVegetableEventArgs(PlayerController playerCtrl, Vegetable veg)
	{
		playerController = playerCtrl;
		vegetable = veg;
	}
}
