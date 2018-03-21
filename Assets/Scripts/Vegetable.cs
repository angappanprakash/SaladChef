using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VegetableType
{
	ARTICHOKE = 0,
	BROCCOLLI,
	CUCUMBER,
	DASHEEN,
	EGGPLANT,
	FENNEL,
	NONE= -1
}

public enum VegatableState
{
	IDLE = 0,
	CHOPPED,
	NONE = -1
}

public class Vegetable : MonoBehaviour 
{
	[SerializeField]
	protected VegetableType _vegetableType;
	private bool			m_IsTriggerStay;
	private PlayerController m_CollidedPlayer;

	public VegetableType VegetableType
	{
		get { return _vegetableType; }
	}

	protected virtual void Awake()
	{
		m_IsTriggerStay = false;
		m_CollidedPlayer = null;
	}

	protected virtual void Start () 
	{
		
	}
	
	protected virtual void Update () 
	{
		
	}

	protected virtual void OnTriggerEnter(Collider collider)
	{
		if(collider.tag == "Player")
		{
			m_CollidedPlayer = collider.GetComponentInParent<PlayerController>();
		}
	}

	protected virtual void OnTriggerExit(Collider collider)
	{
		if(collider.tag == "Player")
		{
			m_CollidedPlayer = collider.GetComponentInParent<PlayerController>();
		}
	}

	protected virtual void HighLight(bool flag)
	{
	}
}
