using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GameData;

public class PlayerController : MonoBehaviour
{
#region Variables
	public const int			MAX_VEGETABLES_CAN_CARRY = 2;
	[SerializeField]
	private Collider 			m_Collider = null;
	[SerializeField]
	public List<Vegetable>		m_Vegetables;

	private PlayerIndex m_PlayerIndex;
	private PlayerState m_CurrentState;
	private PlayerState m_PrevState;

	private float 		m_VerticalSpeed = 0;
	private Vector3 	m_MoveDirection = Vector3.zero;
	private float 		m_TurnRate = 1500.0f;
	private Rigidbody 	m_RigidBody;
	private PlayerData 	m_PlayerData;

	private float 		m_StateTimer = 0;
	private float 		m_MoveSpeed = 5;
	private bool		m_IsInsideTrigger;
	private GameObject 	m_CollidedObject;
	private Salad		m_SaladOnHand;
	[SerializeField]
	private int			m_Score;
	[SerializeField]
	private float 		m_Timer;
#endregion

#region Properties
	public PlayerState pCurrentState
	{
		get { return m_CurrentState; }
	}

	public PlayerIndex pPlayerIndex
	{
		get { return m_PlayerIndex; }
	}

	public Vector3 pMoveDirection
	{
		get { return m_MoveDirection; }
	}

	public Rigidbody pRigidBody
	{
		get { return m_RigidBody; }
	}

	public Collider pPlayerCollider
	{
		get	{ return m_Collider; }
	}

	public PlayerData pPlayerData
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
		GameManager.Instance.pGameEventSystem.SubscribeEvent(GameEventsList.PlayerEvents.ON_SALAD_SERVE_SUCCESS, OnSaladServeSuccess);
		GameManager.Instance.pGameEventSystem.SubscribeEvent(GameEventsList.PlayerEvents.ON_SALAD_SERVE_FAIL, OnSaladServeFail);
	}

	private void OnDisable()
	{
		GameManager.Instance.pGameEventSystem.UnsubscribeEvent(GameEventsList.PlayerEvents.ON_SALAD_SERVE_SUCCESS, OnSaladServeSuccess);
		GameManager.Instance.pGameEventSystem.UnsubscribeEvent(GameEventsList.PlayerEvents.ON_SALAD_SERVE_FAIL, OnSaladServeFail);
	}

	private void Update()
	{
		ProcessState();
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "VegetableDispenser" || collider.tag == "ChoppingBoard" || collider.tag == "NextToChop" || collider.tag == "CustomerSlot" || collider.tag == "Salad")
		{
			m_CollidedObject = collider.gameObject;
			m_IsInsideTrigger = true;
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "VegetableDispenser" || collider.tag == "ChoppingBoard" || collider.tag == "NextToChop" || collider.tag == "CustomerSlot" || collider.tag == "Salad")
		{
			m_CollidedObject = collider.gameObject;
			m_IsInsideTrigger = false;
		}
	}

	private void OnDestroy()
	{
	}
	#endregion

	#region Class specific functions
	private void OnSaladServeSuccess(PlayerEventParams eventArgs)
	{
		OnSaladServedEventArgs eventParams = (OnSaladServedEventArgs)eventArgs;
		PlayerController playerController = eventParams.playerController;
		if(m_PlayerIndex == playerController.m_PlayerIndex)
			m_Score += 10;
	}

	private void OnSaladServeFail(PlayerEventParams eventArgs)
	{
		
	}

	public void Init(PlayerData playerData)
	{
		m_PlayerData = playerData;
		m_Timer = playerData._timer;
		m_PlayerIndex = m_PlayerData._playerIndex;
		m_MoveDirection = transform.forward;
		m_Score = 0;
	}

	private void OnPlayerInput(PlayerInputEventData eventData)
	{
		switch (eventData._type)
		{
		case PlayerInputAction.MOVE:
			{
				if (CanProcessState(PlayerState.RUNNING)  && m_CurrentState != PlayerState.CHOPPING)
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
		case PlayerInputAction.INTERACTION:
			{
				if(m_IsInsideTrigger)
					SetState(PlayerState.INTERACTION);
			}
			break;

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
		case PlayerState.INTERACTION:
				ProcessInteraction();
			break;
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

	public bool IsAlreadyExists(VegetableDispenserType dispenserType)
	{
		for(int i=0; i < m_Vegetables.Count; i++)
		{
			if(m_Vegetables[i].pVegetableType.ToString() == dispenserType.ToString())
				return true;
		}
		return false;
	}

	private void ProcessInteraction()
	{
		switch(m_CollidedObject.tag)
		{
		case "VegetableDispenser":
			VegetableDispenserType dispenserType = m_CollidedObject.GetComponent<VegetableDispenser>().pDispenserType;
			if(!IsAlreadyExists(dispenserType))
				AddVegetable(dispenserType);
			break;
		case "ChoppingBoard":
			if(m_Vegetables.Count > 0)
			{
				ChoppingBoard choppingBoard = m_CollidedObject.GetComponent<ChoppingBoard>();

				if(choppingBoard.CanAdd())
				{
					Vegetable veg = m_Vegetables[0];
					choppingBoard.AddVegetable(veg.pVegetableType);
					RemoveVegetable(veg);
					GameObject go = Utilities.Util.FindChildObject(gameObject, veg.name);
					veg.transform.SetParent(null);
					Destroy(go);
				}
			}
			break;
		case "NextToChop":
			break;
		case "Salad":
			{
				Salad salad = m_CollidedObject.GetComponent<Salad>();
				salad.transform.parent = this.gameObject.transform;
				Utilities.Util.SetDefaultLocalTransform(salad.gameObject);
			}
			break;
		case "Customer":
			{
				Customer customer = m_CollidedObject.GetComponent<Customer>();
				Salad salad = m_SaladOnHand;
				customer.OnSaladServed(salad.pSaladType, this.gameObject.GetComponent<PlayerController>());
				GameObject saladObj = Utilities.Util.FindChildObject(gameObject, salad.gameObject.name);
				saladObj.gameObject.transform.SetParent(null);
				Destroy(saladObj);
			}
			break;
		}

		SetState(PlayerState.IDLE);
	}

	private string GetPlayerID(PlayerIndex playerIndex)
	{
		switch(pPlayerIndex)
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

	private Vegetable GetNextVegOnList()
	{
		return m_Vegetables[0];
	}

	private void AddVegetable(VegetableDispenserType DispenserType)
	{
		if(m_Vegetables != null)
		{
			if(m_Vegetables.Count == MAX_VEGETABLES_CAN_CARRY)
				return;
			Vegetable veg = LevelManager.Instance.CreateVegetable(DispenserType);
			veg.transform.parent = this.transform;
			veg.pOwner = this.gameObject.GetComponent<PlayerController>();
			Utilities.Util.SetDefaultLocalTransform(veg.gameObject);
			m_Vegetables.Add(veg);
			GameManager.Instance.pGameEventSystem.TriggerEvent(GameEventsList.PlayerEvents.ON_COLLECT_VEGETABLE, new OnCollectVegetableEventArgs(this.gameObject.GetComponent<PlayerController>(), veg));
		}
	}

	private void RemoveVegetable(Vegetable vegetable)
	{
		if(m_Vegetables != null)
		{
			if(m_Vegetables.Count == 0)
				return;
			GameManager.Instance.pGameEventSystem.TriggerEvent(GameEventsList.PlayerEvents.ON_REMOVE_VEGETABLE, new OnRemoveVegetableEventArgs(this.gameObject.GetComponent<PlayerController>(), vegetable));
			m_Vegetables.Remove(vegetable);
		}
	}

	public static void SetDefaultLocalTransform(GameObject obj)
	{
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localScale = Vector3.one;
	}

	#endregion
}