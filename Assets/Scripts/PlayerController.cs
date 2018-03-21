using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameData;

public class PlayerController : MonoBehaviour
{
#region Variables
	[SerializeField]
	private Collider 			m_Collider = null;
	[SerializeField]
	private List<GameObject>	m_Vegetables;
		

	private PlayerIndex m_PlayerIndex;
	private PlayerState m_CurrentState;
	private PlayerState m_PrevState;

	private float 		m_VerticalSpeed = 0;
	private Vector3 	m_MoveDirection = Vector3.zero;
	private float 		m_TurnRate = 1000.0f;
	private Rigidbody 	m_RigidBody;
	private PlayerData 	m_PlayerData;

	private float 		m_StateTimer = 0;
	private float 		m_MoveSpeed = 5;
#endregion

#region Properties
	public PlayerState CurrentState
	{
		get { return m_CurrentState; }
	}

	public PlayerIndex PlayerIndex
	{
		get { return m_PlayerIndex; }
	}

	public Vector3 MoveDirection
	{
		get { return m_MoveDirection; }
	}

	public Rigidbody RigidBody
	{
		get { return m_RigidBody; }
	}

	public Collider PlayerCollider
	{
		get	{ return m_Collider; }
	}

	public PlayerData PlayerData
	{
		get { return m_PlayerData; }
	}
#endregion

#region Monobehaviour functions
	private void Awake()
	{
		m_RigidBody = GetComponent<Rigidbody>();
	}

	private void OnEnable()
	{
		m_CurrentState = PlayerState.SPAWN;
		m_PrevState = PlayerState.NONE;
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
		ProcessState();
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Vegetable")
		{
			Debug.Log("collided with:"+ collider.name);
		}
	}

	private void OnDestroy()
	{
	}
	#endregion

	#region Class specific functions
	public void Init(PlayerIndex playerIndex)
	{
		m_PlayerIndex = playerIndex;
		m_MoveDirection = transform.forward;
	}

	private void OnPlayerInput(PlayerInputEventData eventData)
	{
		switch (eventData._type)
		{
		case PlayerInputAction.MOVE:
			{
				if (CanProcessState(PlayerState.RUNNING))
				{
					Dictionary<string, float> values = (Dictionary<string, float>)eventData._data;
					float horizontalComp = values["horizontal"];
					float verticalComp = values["vertical"];
					if (Mathf.Abs(horizontalComp) > 0.01f || Mathf.Abs(verticalComp) > 0.01f)
					{
						Vector3 targetDirection = horizontalComp * Vector3.right + verticalComp * Vector3.forward;
						m_MoveDirection = Vector3.RotateTowards(m_MoveDirection, targetDirection, m_TurnRate * Mathf.Deg2Rad * Time.deltaTime, 1000);
						m_MoveDirection = m_MoveDirection.normalized;
						if (Mathf.Abs(horizontalComp) > 0.1f || Mathf.Abs(verticalComp) > 0.1f)
						{
							Vector3 movement = m_MoveDirection * m_MoveSpeed * Time.deltaTime + new Vector3(0, m_VerticalSpeed, 0);
							//transform.position += movement;
							m_RigidBody.MovePosition(transform.position + movement);
						}
						//transform.rotation = Quaternion.LookRotation(m_MoveDirection);
						m_RigidBody.MoveRotation(Quaternion.LookRotation(m_MoveDirection));
					}
					else if (m_CurrentState == PlayerState.RUNNING)
						SetState(PlayerState.IDLE);
				}
				break;
			}

		case PlayerInputAction.PAUSE:
			{
				break;
			}
		}
	}

	public void SetState(PlayerState state, object data = null)
	{
		if (m_CurrentState == state)
			return;

		m_PrevState = m_CurrentState;
		m_CurrentState = state;
		//m_RigidBody.isKinematic = (m_CurrentState == PlayerState.IDLE);

		switch (m_CurrentState)
		{
		case PlayerState.IDLE:
			m_RigidBody.velocity = Vector3.zero;
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

	public bool CanProcessState(PlayerState state)
	{
		bool canProcess = false;
		switch (state)
		{
		case PlayerState.RUNNING:
			canProcess = (m_CurrentState == PlayerState.SPAWN || m_CurrentState == PlayerState.IDLE || m_CurrentState == PlayerState.RUNNING);
			break;
		}

		return canProcess;
	}

	private string GetPlayerID(PlayerIndex playerIndex)
	{
		switch(PlayerIndex)
		{
		case PlayerIndex.PLAYER1:
			return "P1";
		case PlayerIndex.PLAYER2:
			return "P2";
		case PlayerIndex.INVALID:
			return "P-INVALID";
		default:
			return "P-INVALID";
		}
	}
	#endregion
}