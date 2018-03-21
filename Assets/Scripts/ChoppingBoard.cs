using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChoppingBoardType
{
	RED_TEAM = 0,
	BLUE_TEAM,
	NONE= -1
}

public enum BoardState
{
	COLLECTED_VEG = 0,
	CHOPPING,
	COMPLETED,
	MAKE_SALAD,
	CLEARED,
	NONE = -1
}

public class ChoppingBoardDelegates
{
	public delegate void OnChoppingBegin(ChoppingBoard boss);
	public delegate void OnChoppingEnd(ChoppingBoard boss);
	public delegate void OnUpdateChoppedList(ChoppingBoard boss);
}

public class ChoppingBoard : MonoBehaviour 
{
#region Variables
	public static event ChoppingBoardDelegates.OnChoppingBegin 		_onChoppingBegin;
	public static event ChoppingBoardDelegates.OnChoppingEnd		_onChoppingEnd;
	public static event ChoppingBoardDelegates.OnUpdateChoppedList	_onnUpdateChoppedList;

	public const int					MAX_VEGETABLES = 3;
	[SerializeField]
	protected ChoppingBoardType 		_type;
	[SerializeField]
	private GameObject					_hightlight;

	private bool						m_IsHighlighted;
	private bool						m_IsTriggerStay;
	private PlayerController 			m_CollidedPlayer;
	[SerializeField]
	private List<Vegetable> 			m_ChoppedList;
	private float						m_ChoppingTime;
	private BoardState					m_CurrentState;
	private BoardState					m_PrevState;
	private float						m_StateTimer;
	private Vegetable					m_CurrentVegOnBoard;
#endregion

#region Properties
	public ChoppingBoardType pChoppingBoardType
	{
		get { return _type; }
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
		m_IsTriggerStay = false;
		m_CollidedPlayer = null;
		m_ChoppedList = new List<Vegetable>();
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
	public void AddVegetable(VegetableType vegType)
	{
		if(m_ChoppedList != null)
		{
			if(m_ChoppedList.Count == MAX_VEGETABLES)
				return;
			Vegetable veg = LevelManager.Instance.CreateVegetable(vegType);
			veg.transform.parent = this.transform;
			veg.pOwner = this.gameObject.GetComponent<PlayerController>();
			m_CurrentVegOnBoard = veg;
			Utilities.Util.SetDefaultLocalTransform(veg.gameObject);
			SetState(BoardState.CHOPPING);
		}
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
		case BoardState.CHOPPING:
			m_StateTimer = m_CurrentVegOnBoard.pChoppingDuration;
			break;
		case BoardState.COMPLETED:
			m_ChoppedList.Add(m_CurrentVegOnBoard);
			SetState(BoardState.MAKE_SALAD);
			break;
		case BoardState.MAKE_SALAD:
			if(m_ChoppedList.Count == 3)
				MakeSalad(m_ChoppedList);
			break;
		}
	}

	private void ProcessState()
	{
		m_StateTimer = Mathf.Max(0, m_StateTimer - Time.deltaTime);

		switch (m_CurrentState)
		{
		case BoardState.CHOPPING:
			if(m_StateTimer == 0)
				SetState(BoardState.COMPLETED);
			break;
		default:
			break;
		}
	}

	protected virtual void HighLight(bool flag)
	{
		_hightlight.gameObject.SetActive(flag);
		m_IsHighlighted = flag;
	}

	private void MakeSalad(List<Vegetable> ChoppedList)
	{
		Debug.Log("Make salad");
	}

	public bool CanAdd()
	{
		return m_ChoppedList.Count == MAX_VEGETABLES ? false : true;
	}
#endregion
}

