using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
	BONUSSCORE = 0,
	SPEED_BOOST,
	NONE = -1
}

public class PowerUp : MonoBehaviour 
{
	[SerializeField]
	protected float		m_LifeTime;

	protected virtual void Start () 
	{
	}
	
	protected virtual void Update () 
	{
		if(m_LifeTime > 0)
		{
			m_LifeTime = Mathf.Max(0, m_LifeTime - Time.deltaTime);
			if(m_LifeTime == 0)
				Destroy(gameObject);
		}
	}

	protected virtual void OnTriggerEnter(Collider collider)
	{
		
	}
}
