using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomerState
{
	IDLE = 0,
	SPAWN,
	RECEIVED_SALAD,
	EATING,
	ANGRY,
	TIP_THE_PLAYER,
	LEFT,
	NONE = -1
}

public class Customer : MonoBehaviour 
{
	#region Variables
	[SerializeField]
	private GameObject					_hightlight;
	[SerializeField]
	private SaladType					m_SaladType;

	private bool						m_IsHighlighted;
	private PlayerController 			m_CollidedPlayer;

	private BoardState					m_CurrentState;
	private BoardState					m_PrevState;
	private float						m_StateTimer;

	private PlayerController			m_LastServedOwner;
	#endregion

	#region Properties
	public SaladType pSaladType
	{
		get { return m_SaladType; }
	}

	public BoardState pCurrentState
	{
		get { return m_CurrentState; }
	}
	#endregion

	#region Monobehaviour functions
	protected virtual void Awake()
	{
		_hightlight.gameObject.SetActive(false);
		m_IsHighlighted = false;
		m_LastServedOwner = null;
		m_CollidedPlayer = null;
	}

	protected virtual void Start () 
	{

	}

	protected virtual void Update () 
	{
		ProcessState();
	}

	protected virtual void OnTriggerEnter(Collider collider)
	{
		if(collider.tag == "Player")
		{
			m_CollidedPlayer = collider.GetComponentInParent<PlayerController>();
			if(!m_IsHighlighted)
				HighLight(true);
		}
	}

	protected virtual void OnTriggerExit(Collider collider)
	{
		if(collider.tag == "Player")
		{
			m_CollidedPlayer = collider.GetComponentInParent<PlayerController>();
			if(m_IsHighlighted)
				HighLight(false);
		}
	}
	#endregion

	#region Class specific functions
	public void OnSaladServed(SaladType saladType, PlayerController owner)
	{
		m_LastServedOwner = owner;
		if(saladType == m_SaladType)
			GameManager.Instance.pGameEventSystem.TriggerEvent(GameEventsList.PlayerEvents.ON_SALAD_SERVE_SUCCESS, new OnSaladServedEventArgs(m_SaladType, m_LastServedOwner, this.gameObject.GetComponent<Customer>()));
		else
			GameManager.Instance.pGameEventSystem.TriggerEvent(GameEventsList.PlayerEvents.ON_SALAD_SERVE_FAIL, new OnSaladServedEventArgs(m_SaladType, m_LastServedOwner, this.gameObject.GetComponent<Customer>()));			
	}

	public void SetState(BoardState state, object data = null)
	{
		if (m_CurrentState == state)
			return;

		m_PrevState = m_CurrentState;
		m_CurrentState = state;
		//m_RigidBody.isKinematic = (m_CurrentState == PlayerState.IDLE);

		switch (m_CurrentState)
		{
		case BoardState.IDLE:
			break;
		}
	}

	private void ProcessState()
	{
		m_StateTimer = Mathf.Max(0, m_StateTimer - Time.deltaTime);

		switch (m_CurrentState)
		{
		default:
			break;
		}
	}

	protected virtual void HighLight(bool flag)
	{
		_hightlight.gameObject.SetActive(flag);
		m_IsHighlighted = flag;
	}
	#endregion
}

