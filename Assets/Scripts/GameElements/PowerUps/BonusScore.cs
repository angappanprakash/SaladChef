using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScore : PowerUp
{
	[SerializeField]
	private int m_BonusScore;

	protected override void Start () 
	{
		base.Start();
	}

	protected override void Update () 
	{
		base.Update();
	}

	protected override void OnTriggerEnter(Collider collider)
	{
		base.OnTriggerEnter(collider);
		if(collider.tag == "Player")
		{
			PlayerController m_CollidedPlayer = collider.GetComponentInParent<PlayerController>();
			GameManager.Instance.pGameEventSystem.TriggerEvent(GameEventsList.PlayerEvents.ON_SCORE_UPDATE, new ScoreUpdateEventArgs(m_CollidedPlayer, m_BonusScore));
			DestroyObject(gameObject);
		}
	}
}
