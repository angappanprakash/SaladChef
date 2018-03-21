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

public enum VegetableState
{
	IDLE = 0,
	CHOPPED,
	READY_TO_CHOP,
	NONE = -1
}

public class Vegetable : MonoBehaviour 
{
#region Variables
	[SerializeField]
	protected VegetableType 	_vegetableType;
	[SerializeField]
	protected float				m_ChoppingDuration = 5.0f;
	private PlayerController 	m_Owner;

	protected VegetableState	m_CurrentState;
	protected VegetableState 	m_PreviousState;

	private float           	mStateTimer;
#endregion

#region Properties
	public VegetableType pVegetableType
	{
		get { return _vegetableType; }
	}

	public VegetableState pCurrentState
	{
		get { return m_CurrentState; }
	}

	public float pChoppingDuration
	{
		get { return m_ChoppingDuration; }
	}

	public PlayerController pOwner
	{
		get { return m_Owner;	}
		set { m_Owner = value;	}
	}
#endregion

#region Monobehaviour functions
	protected virtual void Awake()
	{
		m_Owner = null;
	}

	protected virtual void Start () 
	{
		
	}
	
	protected virtual void Update () 
	{
		
	}

	public void SetOwner(PlayerController owner)
	{
		m_Owner = owner;
	}
#endregion
}
