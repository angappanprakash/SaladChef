using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VegetableDispenserType
{
	ARTICHOKE = 0,
	BROCCOLLI,
	CUCUMBER,
	DASHEEN,
	EGGPLANT,
	FENNEL,
	NONE= -1
}

public class VegetableDispenser : MonoBehaviour 
{
#region Variables
	[SerializeField]
	protected VegetableDispenserType 	_type;
	[SerializeField]
	private GameObject					_hightlight;

	private bool						m_IsHighlighted;
	private bool						m_IsTriggerStay;
	private PlayerController 			m_CollidedPlayer;
#endregion

#region Properties
	public VegetableDispenserType pDispenserType
	{
		get { return _type; }
	}
#endregion

#region Monobehaviour functions
	protected virtual void Awake()
	{
		_hightlight.gameObject.SetActive(false);
		m_IsHighlighted = false;
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
	protected virtual void HighLight(bool flag)
	{
		_hightlight.gameObject.SetActive(flag);
		m_IsHighlighted = flag;
	}
#endregion
}

