using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChoppingBoardType
{
	RED_TEAM = 0,
	BLUE_TEAM,
	NONE= -1
}

public enum ChoppingBoardState
{
	IDLE = 0,
	COLLECTED_VEG,
	CHOPPING,
	COMPLETED_CHOPPING,
	MAKE_SALAD,
	CLEARED,
	NONE = -1
}

public class ChoppingBoard : MonoBehaviour 
{
#region Variables
	private const string				SALAD_ASSETS_PATH = "Salads";
	public const int					MAX_VEGETABLES = 3;
	[SerializeField]
	protected ChoppingBoardType 		m_Type;
	[SerializeField]
	private GameObject					m_Hightlight;
	[SerializeField]
	private GameObject					m_ParentObj;

	private bool						m_IsHighlighted;
	private PlayerController 			m_CollidedPlayer;
	[SerializeField]
	private List<Vegetable> 			m_ChoppedList;
	private float						m_ChoppingTime;
	private ChoppingBoardState					m_CurrentState;
	private ChoppingBoardState					m_PrevState;
	private float						m_StateTimer;
	private Vegetable					m_CurrentVegOnBoard;
	private Salad						m_SaladOnBoard;
#endregion

#region Properties
	public ChoppingBoardType pChoppingBoardType
	{
		get { return m_Type; }
	}

	public ChoppingBoardState pCurrentState
	{
		get { return m_CurrentState; }
	}

	public Salad pSaldOnBoard
	{
		get { return m_SaladOnBoard; }
	}
#endregion

#region Monobehaviour functions
	protected virtual void Awake()
	{
		m_Hightlight.gameObject.SetActive(false);
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
			veg.transform.SetParent(this.transform);
			veg.pOwner = this.gameObject.GetComponent<PlayerController>();
			m_CurrentVegOnBoard = veg;
			Utilities.Util.SetDefaultLocalTransform(veg.gameObject);
			SetState(ChoppingBoardState.COLLECTED_VEG);
		}
	}

	public void SetState(ChoppingBoardState state, object data = null)
	{
		if (m_CurrentState == state)
			return;

		m_PrevState = m_CurrentState;
		m_CurrentState = state;
		//m_RigidBody.isKinematic = (m_CurrentState == PlayerState.IDLE);

		switch (m_CurrentState)
		{
		case ChoppingBoardState.IDLE:
			break;
		case ChoppingBoardState.COLLECTED_VEG:
			m_StateTimer = m_CurrentVegOnBoard.pChoppingDuration;
			SetState(ChoppingBoardState.CHOPPING);
			break;
		case ChoppingBoardState.CHOPPING:
			break;
		case ChoppingBoardState.COMPLETED_CHOPPING:
			m_ChoppedList.Add(m_CurrentVegOnBoard);
			if(m_ChoppedList.Count == 3)
				SetState(ChoppingBoardState.MAKE_SALAD);
			else
				SetState(ChoppingBoardState.IDLE);
			break;
		case ChoppingBoardState.MAKE_SALAD:
			if(m_ChoppedList.Count == 3)
			{
				MakeSalad(m_ChoppedList);
			}
			break;
		case ChoppingBoardState.CLEARED:
			SetState(ChoppingBoardState.IDLE);
			break;
		}
	}

	private void ProcessState()
	{
		m_StateTimer = Mathf.Max(0, m_StateTimer - Time.deltaTime);

		switch (m_CurrentState)
		{
		case ChoppingBoardState.CHOPPING:
			if(m_StateTimer == 0)
				SetState(ChoppingBoardState.COMPLETED_CHOPPING);
			break;
		default:
			break;
		}
	}

	protected virtual void HighLight(bool flag)
	{
		m_Hightlight.gameObject.SetActive(flag);
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

		Salad saladObj = null;
		if(vegetableTypes.Contains(VegetableType.ARTICHOKE) && vegetableTypes.Contains(VegetableType.BROCCOLLI) && vegetableTypes.Contains(VegetableType.CUCUMBER))
			saladObj = LevelManager.Instance.CreateSalad(SaladType.SALAD_ABC);
		if(vegetableTypes.Contains(VegetableType.DASHEEN) && vegetableTypes.Contains(VegetableType.EGGPLANT) && vegetableTypes.Contains(VegetableType.FENNEL))
			saladObj = LevelManager.Instance.CreateSalad(SaladType.SALAD_DEF);
		if(vegetableTypes.Contains(VegetableType.BROCCOLLI) && vegetableTypes.Contains(VegetableType.CUCUMBER) && vegetableTypes.Contains(VegetableType.DASHEEN))
			saladObj = LevelManager.Instance.CreateSalad(SaladType.SALAD_BCD);
		if(vegetableTypes.Contains(VegetableType.CUCUMBER) && vegetableTypes.Contains(VegetableType.DASHEEN) && vegetableTypes.Contains(VegetableType.EGGPLANT))
			saladObj = LevelManager.Instance.CreateSalad(SaladType.SALAD_CDE);
		if(vegetableTypes.Contains(VegetableType.EGGPLANT) && vegetableTypes.Contains(VegetableType.FENNEL) && vegetableTypes.Contains(VegetableType.ARTICHOKE))
			saladObj = LevelManager.Instance.CreateSalad(SaladType.SALAD_EFA);

		for(int j = 0; j < m_ChoppedList.Count; j++)
		{
			GameObject go = Utilities.Util.FindChildObject(gameObject, m_ChoppedList[j].gameObject.name);
			m_ChoppedList[j].transform.SetParent(null);
			Destroy(go);
		}
		m_ChoppedList.Clear();
		saladObj.gameObject.transform.SetParent(m_ParentObj.transform);
		Utilities.Util.SetDefaultLocalTransform(saladObj.gameObject);
		Vector3 tempPos = saladObj.transform.localPosition;
		float offSetZ = 0.0f;
		if(m_Type == ChoppingBoardType.BLUE_TEAM)
			offSetZ = - 3.0f;
		else if(m_Type == ChoppingBoardType.RED_TEAM)
			offSetZ = 0.5f;
			
		saladObj.transform.localPosition = new Vector3(tempPos.x, tempPos.y, tempPos.z +offSetZ);
		m_SaladOnBoard = saladObj;
	}

	public bool CanAdd()
	{
		if( m_ChoppedList.Count != MAX_VEGETABLES && m_CurrentState != ChoppingBoardState.CHOPPING)
			return true;
		return false;
	}

	public Salad CreateSalad(SaladType saladType)
	{
		GameObject go = (GameObject)Instantiate(Resources.Load(SALAD_ASSETS_PATH + "/" + saladType.ToString()));
		return go.GetComponent<Salad>();
	}
#endregion
}

