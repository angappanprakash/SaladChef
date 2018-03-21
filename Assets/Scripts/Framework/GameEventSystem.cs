using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class EventIds
{
	public GameEventsList.PlayerEvents _eventType;
	public List<int> _ids = new List<int>();
}

public class GameEventSystem : MonoBehaviour
{
#region Variables
	public System.Action<PlayerEventParams> GameEventHandlers;
	[SerializeField]
	private List<EventIds> m_EventIdMapper = new List<EventIds>();
#endregion

#region Class specific functions
	public void SubscribeEvent(GameEventsList.PlayerEvents EventType, System.Action<PlayerEventParams> eventHandler)
	{
		GameEventHandlers += eventHandler;
		EventIds eventIds = m_EventIdMapper.Find(x => x._eventType == EventType);
		if (eventIds == null)
		{
			eventIds = new EventIds();
			eventIds._eventType = EventType;
			eventIds._ids.Add(eventHandler.GetHashCode());
			m_EventIdMapper.Add(eventIds);
		}
		else
			eventIds._ids.Add(eventHandler.GetHashCode());
	}

	public void UnsubscribeEvent(GameEventsList.PlayerEvents EventType, System.Action<PlayerEventParams> eventHandler)
	{
		if (eventHandler == null)
			return;

		GameEventHandlers -= eventHandler;

		EventIds eventIds = m_EventIdMapper.Find(x => x._eventType == EventType);
		if (eventIds != null)
			eventIds._ids.Remove(eventHandler.GetHashCode());
	}

	public void TriggerEvent(GameEventsList.PlayerEvents EventType, PlayerEventParams playerEventParams)
	{
		EventIds eventIds = m_EventIdMapper.Find(x => x._eventType == EventType);
		if (eventIds != null)
		{
			InvokeRelevantListeners(eventIds._ids, GameEventHandlers.GetInvocationList(), playerEventParams);
		}
		else
		{
			//Debug.Log("Key Not Found!");
		}
	}

	private void InvokeRelevantListeners(List<int> ids, System.Delegate[] eventhandlers, PlayerEventParams playerEventParams)
	{
		foreach (var eventId in ids)
		{
			foreach (var eventHandler in eventhandlers)
			{
				if (eventHandler.GetHashCode() == eventId)
				{
					eventHandler.DynamicInvoke(playerEventParams);
				}
			}
		}
	}
#endregion
}
