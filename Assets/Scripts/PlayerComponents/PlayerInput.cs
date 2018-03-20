using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayerInputAction
{
	MOVE,
    PAUSE
}

public class PlayerInputEventData
{
	public PlayerInputAction _type;
	public object			 _data;
}

public enum ControlMap
{
	NONE = -1,
	ASWD_KEYS,
	ARROW_KEYS
}

public class PlayerInput : MonoBehaviour
{
#region Variables
	protected PlayerController 	m_Controller;
	private ControlMap 			m_ControlMap;
#endregion

#region Properties
#endregion

#region Monobehaviour functions
	protected virtual  void Start()
	{
		m_Controller = GetComponent<PlayerController>();

		SetDefaultControls();
	}

	protected virtual void Update()
	{
		if(m_Controller != null)
		{
			
			float horizontalComp = Input.GetAxis(GetPlayerControl(m_ControlMap)+"Horizontal");
			float verticalComp = Input.GetAxis(GetPlayerControl(m_ControlMap)+"Vertical");
//			if(Mathf.Abs(horizontalComp) > 0.01f || Mathf.Abs(verticalComp) > 0.01f)
			{
				PlayerInputEventData eventData = new PlayerInputEventData();
				eventData._type = PlayerInputAction.MOVE;
				Dictionary<string, float> values = new Dictionary<string, float>();
				values.Add("horizontal", horizontalComp);
				values.Add("vertical", verticalComp);
				eventData._data = values;
				SendMessage("OnPlayerInput", eventData, SendMessageOptions.DontRequireReceiver);
			}
        }
	}
#endregion

#region Class specific functions
	private void SetDefaultControls()
	{
		if(m_Controller.PlayerIndex == GameData.PlayerIndex.PLAYER1)
			m_ControlMap = ControlMap.ASWD_KEYS;
		else
			m_ControlMap = ControlMap.ARROW_KEYS;
	}

	private string GetPlayerControl(ControlMap map)
	{
		switch(map)
		{
		case ControlMap.ASWD_KEYS:
			return "P1_";
		case ControlMap.ARROW_KEYS:
			return "P2_";
			default:
			return "";
		}
	}
#endregion
}
