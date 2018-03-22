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
	IDLE = 0,
	COLLECTED_VEG,
	CHOPPING,
	COMPLETED,
	MAKE_SALAD,
	CLEARED,
	NONE = -1
}

public class ChoppingBoard : MonoBehaviour 
{
#region Variables

	public const int					MAX_VEGETABLES = 3;
	[SerializeField]
	protected ChoppingBoardType 		_type;
	[SerializeField]
	private GameObject					_hightlight;

	private bool						m_IsHighlighted;
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
			SetState(BoardState.COLLECTED_VEG);
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
		case BoardState.IDLE:
			break;
		case BoardState.COLLECTED_VEG:
			m_StateTimer = m_CurrentVegOnBoard.pChoppingDuration;
			SetState(BoardState.CHOPPING);
			break;
		case BoardState.CHOPPING:
			break;
		case BoardState.COMPLETED:
			m_ChoppedList.Add(m_CurrentVegOnBoard);
			if(m_ChoppedList.Count == 3)
				SetState(BoardState.MAKE_SALAD);
			else
				SetState(BoardState.IDLE);
			break;
		case BoardState.MAKE_SALAD:
			if(m_ChoppedList.Count == 3)
			{
				MakeSalad(m_ChoppedList);
			}
			SetState(BoardState.IDLE);
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
		//Debug.Log("Make salad");
		List<VegetableType> vegetableTypes = new List<VegetableType>();
		for(int i = 0; i< ChoppedList.Count; i++)
		{
			vegetableTypes.Add(m_ChoppedList[i].pVegetableType);
		}

		Salad salad = null;
		if(vegetableTypes.Contains(VegetableType.ARTICHOKE) && vegetableTypes.Contains(VegetableType.BROCCOLLI) && vegetableTypes.Contains(VegetableType.ARTICHOKE))
			salad = LevelManager.Instance.MakeSalad(SaladType.SALAD_ABC);
		if(vegetableTypes.Contains(VegetableType.DASHEEN) && vegetableTypes.Contains(VegetableType.EGGPLANT) && vegetableTypes.Contains(VegetableType.FENNEL))
			salad = LevelManager.Instance.MakeSalad(SaladType.SALAD_DEF);
		if(vegetableTypes.Contains(VegetableType.BROCCOLLI) && vegetableTypes.Contains(VegetableType.CUCUMBER) && vegetableTypes.Contains(VegetableType.DASHEEN))
			salad = LevelManager.Instance.MakeSalad(SaladType.SALAD_BCD);
		if(vegetableTypes.Contains(VegetableType.CUCUMBER) && vegetableTypes.Contains(VegetableType.DASHEEN) && vegetableTypes.Contains(VegetableType.EGGPLANT))
			salad = LevelManager.Instance.MakeSalad(SaladType.SALAD_CDE);
		if(vegetableTypes.Contains(VegetableType.EGGPLANT) && vegetableTypes.Contains(VegetableType.FENNEL) && vegetableTypes.Contains(VegetableType.ARTICHOKE))
			salad = LevelManager.Instance.MakeSalad(SaladType.SALAD_EFA);

		for(int j = 0; j < m_ChoppedList.Count; j++)
		{
			GameObject go = Utilities.Util.FindChildObject(gameObject, m_ChoppedList[j].gameObject.name);
			m_ChoppedList[j].transform.SetParent(null);
			Destroy(go);
		}
		m_ChoppedList.Clear();
		salad.gameObject.transform.parent = this.gameObject.transform;
		Utilities.Util.SetDefaultLocalTransform(salad.gameObject);
	}

	public bool CanAdd()
	{
		if( m_ChoppedList.Count != MAX_VEGETABLES && m_CurrentState != BoardState.CHOPPING)
			return true;
		return false;
	}
#endregion
}

