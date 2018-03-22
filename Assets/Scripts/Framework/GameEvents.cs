using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsList
{
	public enum PlayerEvents
	{
		GAME_START,
		GAME_RUNNING,
		GAME_PAUSED,
		GAME_RESUMED,
		GAME_END,

		ON_COLLECT_VEGETABLE,
		ON_REMOVE_VEGETABLE,
		ON_SALAD_SERVE_SUCCESS,
		ON_SALAD_SERVE_FAIL,

		ON_SCORE_UPDATE,
		ON_SPEED_UPDATE,
		ON_TIMER_UPDATE
	}
}
