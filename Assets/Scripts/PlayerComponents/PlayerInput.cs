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

public class PlayerInput : MonoBehaviour
{
#region Variables
	protected PlayerController 	m_Controller;
#endregion

#region Properties
#endregion

#region Monobehaviour functions
	protected virtual  void Start()
	{
		m_Controller = GetComponent<PlayerController>();
	}

	protected virtual void Update()
	{
		if(m_Controller != null)
		{
			float horizontalComp = Input.GetAxis("Horizontal");
			float verticalComp = Input.GetAxis("Vertical");
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
#endregion
}
