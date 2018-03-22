using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameData;

public class UiHUD : MonoBehaviour 
{
	[SerializeField]
	private Text			m_TxtRedItem_1;
	[SerializeField]
	private Text			m_TxtRedItem_2;

	[SerializeField]
	private Text			m_TxtBlueItem_1;
	[SerializeField]
	private Text			m_TxtBlueItem_2;

	[SerializeField]
	private Text			m_TxtRedChoppingItem_1;
	[SerializeField]
	private Text			m_TxtRedChoppingItem_2;
	[SerializeField]
	private Text			m_TxtRedChoppingItem_3;

	[SerializeField]
	private Text			m_TxtBlueChoppingItem_1;
	[SerializeField]
	private Text			m_TxtBlueChoppingItem_2;
	[SerializeField]
	private Text			m_TxtBlueChoppingItem_3;

	[SerializeField]
	private Image			m_ImgRedChoppingItem_1;
	[SerializeField]
	private Image			m_ImgRedChoppingItem_2;
	[SerializeField]
	private Image			m_ImgRedChoppingItem_3;

	[SerializeField]
	private Image			m_ImgBlueChoppingItem_1;
	[SerializeField]
	private Image			m_ImgBlueChoppingItem_2;
	[SerializeField]
	private Image			m_ImgBlueChoppingItem_3;

	[SerializeField]
	private Text			m_TxtRedScore;
	[SerializeField]
	private Text			m_TxtRedTimer;

	[SerializeField]
	private Text			m_TxtBlueScore;
	[SerializeField]
	private Text			m_TxtBlueTimer;

	[SerializeField]
	private Text 			m_TxtGameTimer;

	// Use this for initialization
	void Start () 
	{
		m_TxtRedItem_1.text = "";
		m_TxtRedItem_2.text = "";

		m_TxtBlueItem_1.text = "";
		m_TxtBlueItem_2.text = "";

		m_TxtRedChoppingItem_1.text = "";
		m_TxtRedChoppingItem_2.text = "";
		m_TxtRedChoppingItem_3.text = "";

		m_TxtBlueChoppingItem_1.text = "";
		m_TxtBlueChoppingItem_2.text = "";
		m_TxtBlueChoppingItem_3.text = "";

		m_TxtRedScore.text = "";
		m_TxtRedTimer.text = "";
		m_TxtBlueScore.text = "";
		m_TxtBlueTimer.text = "";

		m_TxtGameTimer.text = "";

		m_ImgRedChoppingItem_1.gameObject.SetActive(false);
		m_ImgRedChoppingItem_2.gameObject.SetActive(false);
		m_ImgRedChoppingItem_3.gameObject.SetActive(false);

		m_ImgBlueChoppingItem_1.gameObject.SetActive(false);
		m_ImgBlueChoppingItem_2.gameObject.SetActive(false);
		m_ImgBlueChoppingItem_3.gameObject.SetActive(false);
	}

	void OnEnable()
	{
		GameManager.Instance.pGameEventSystem.SubscribeEvent(GameEventsList.PlayerEvents.ON_COLLECT_VEGETABLE, OnCollectVegetable);
		GameManager.Instance.pGameEventSystem.SubscribeEvent(GameEventsList.PlayerEvents.ON_REMOVE_VEGETABLE, OnRemoveVegetable);
		GameManager.Instance.pGameEventSystem.SubscribeEvent(GameEventsList.PlayerEvents.ON_SALAD_SERVE_SUCCESS, OnSaladServeSuccess);
		GameManager.Instance.pGameEventSystem.SubscribeEvent(GameEventsList.PlayerEvents.ON_SALAD_SERVE_FAIL, OnSaladServeFail);
	}

	void OnDisable()
	{
		GameManager.Instance.pGameEventSystem.UnsubscribeEvent(GameEventsList.PlayerEvents.ON_COLLECT_VEGETABLE, OnCollectVegetable);
		GameManager.Instance.pGameEventSystem.UnsubscribeEvent(GameEventsList.PlayerEvents.ON_REMOVE_VEGETABLE, OnRemoveVegetable);
		GameManager.Instance.pGameEventSystem.UnsubscribeEvent(GameEventsList.PlayerEvents.ON_SALAD_SERVE_SUCCESS, OnSaladServeSuccess);
		GameManager.Instance.pGameEventSystem.UnsubscribeEvent(GameEventsList.PlayerEvents.ON_SALAD_SERVE_FAIL, OnSaladServeFail);
	}

	// Update is called once per frame
	void Update ()
	{
		m_TxtGameTimer.text = LevelManager.Instance.pGameTimer.ToString();
	}

	private void OnSaladServeSuccess(PlayerEventParams eventArgs)
	{
		OnSaladServedEventArgs eventParams = (OnSaladServedEventArgs)eventArgs;
		PlayerController playerController = eventParams.playerController;
		playerController.pPlayerData._score += 10;
		if(playerController.pPlayerIndex == PlayerIndex.PLAYER1)
		{
			m_TxtBlueScore.text = playerController.pPlayerData._score.ToString();
		}
		else if(playerController.pPlayerIndex == PlayerIndex.PLAYER2)
		{
			m_TxtRedScore.text = playerController.pPlayerData._score.ToString();
		}
	}

	private void OnSaladServeFail(PlayerEventParams eventArgs)
	{
	}

	void OnCollectVegetable(PlayerEventParams eventArgs)
	{
		OnCollectVegetableEventArgs eventParams = (OnCollectVegetableEventArgs)eventArgs;
		PlayerController playerController = eventParams.playerController;

		if(playerController.pPlayerData._playerColor == PlayerColor.BLUE)
		{
			if(playerController.m_Vegetables.Count == 1)
				m_TxtBlueItem_1.text = GetVegName(eventParams.vegetable.pVegetableType);
			else if(playerController.m_Vegetables.Count == 2)
				m_TxtBlueItem_2.text = GetVegName(eventParams.vegetable.pVegetableType);
		}
		else if(playerController.pPlayerData._playerColor == PlayerColor.RED)
		{
			if(playerController.m_Vegetables.Count == 1)
				m_TxtRedItem_1.text = GetVegName(eventParams.vegetable.pVegetableType);
			else if(playerController.m_Vegetables.Count == 2)
				m_TxtRedItem_2.text = GetVegName(eventParams.vegetable.pVegetableType);
		}
	}

	void OnRemoveVegetable(PlayerEventParams eventArgs)
	{
//		OnCollectVegetableEventArgs eventParams = (OnCollectVegetableEventArgs)eventArgs;
//		PlayerController playerController = eventParams.playerController;
//
//		if(playerController.pPlayerData._playerColor == PlayerColor.BLUE)
//		{
//			if(playerController.m_Vegetables.Count == 1)
//				m_TxtBlueItem_1.text = GetVegName(eventParams.vegetable.pVegetableType);
//			else if(playerController.m_Vegetables.Count == 2)
//				m_TxtBlueItem_2.text = GetVegName(eventParams.vegetable.pVegetableType);
//		}
//		else if(playerController.pPlayerData._playerColor == PlayerColor.RED)
//		{
//			if(playerController.m_Vegetables.Count == 1)
//				m_TxtRedItem_1.text = GetVegName(eventParams.vegetable.pVegetableType);
//			else if(playerController.m_Vegetables.Count == 2)
//				m_TxtRedItem_2.text = GetVegName(eventParams.vegetable.pVegetableType);
//		}
	}

	string GetVegName(VegetableType vegType)
	{
		switch(vegType)
		{
		case VegetableType.ARTICHOKE:
			return "ARTICHOKE";
		case VegetableType.BROCCOLLI:
			return "BROCCOLLI";
		case VegetableType.CUCUMBER:
			return "CUCUMBER";
		case VegetableType.DASHEEN:
			return "DASHEEN";
		case VegetableType.EGGPLANT:
			return "EGGPLANT";
		case VegetableType.FENNEL:
			return "FENNEL";
		}
		return "INVALID";
	}
}
