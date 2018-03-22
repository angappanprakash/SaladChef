using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SaladType
{
	SALAD_ABC = 0,
	SALAD_DEF,
	SALAD_BCD,
	SALAD_CDE,
	SALAD_EFA,
	NONE = -1
}

public enum SaladState
{
	IDLE = 0,
	SERVED_SUCCESSFULLY,
	THROWN_AWAY,
	NONE = -1
}

public class Salad : MonoBehaviour 
{
#region Variables
	[SerializeField]
	protected SaladType 		m_SaladType;
	private PlayerController 	m_Owner;

	protected SaladState		m_CurrentState;
	protected SaladState 		m_PreviousState;

	private float           	mStateTimer;
#endregion

#region Properties
	public SaladType pSaladType
	{
		get { return m_SaladType; }
	}

	public SaladState pCurrentState
	{
		get { return m_CurrentState; }
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
#endregion
}
