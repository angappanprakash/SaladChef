using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PowerUpSpawner : MonoBehaviour
{
	#region Variables
	private const string	POWERUPS_ASSETS_PATH = "PowerUps";

	[SerializeField]
	private Transform[] 			m_PowerUpSpawnMarkers;

	private List<PowerUp>			m_PowerUpList;
	private static PowerUpSpawner 	m_Instance;
	private int                     m_NextSpawnMarkerIndex;
	private Transform               m_SpawnMarkersParent;
	[SerializeField]
	private float					m_SpawnInterval;
#endregion

	#region Properties
	public static PowerUpSpawner Instance
	{
		get { return m_Instance; }
	}

	public List<PowerUp> pPowerUpsList
	{
		get { return m_PowerUpList; }
	}

	public Transform pSpawnMarkersParent
	{
		get { return m_SpawnMarkersParent; }
	}
#endregion

	#region Monobehaviour functions
	private void Awake()
	{
		m_Instance = this;
		m_NextSpawnMarkerIndex = -1;
		GameObject markerParentObj = GameObject.Find("SpawnMarkers");
		if(markerParentObj != null)
			m_SpawnMarkersParent = markerParentObj.transform;
	}

	private void Start()
	{
		InvokeRepeating ("SpawnPowerUp", m_SpawnInterval, m_SpawnInterval);
	}

	private void Update()
	{
	}

	private void OnDestroy()
	{
		m_Instance = null;
	}
	#endregion

	#region Class specific functions
	private int GetNextSpawnMarkerIndex()
	{
		return ((++m_NextSpawnMarkerIndex) % m_PowerUpSpawnMarkers.Length);
	}

	private PowerUp CreatePowerUpInstance(PowerUpType powerType, Transform spawnMarker = null)
	{
		GameObject go = (GameObject)Instantiate(Resources.Load(POWERUPS_ASSETS_PATH + "/" + powerType.ToString()));
		//Debug.Log("path: " +POWERUPS_ASSETS_PATH + "/" + powerType.ToString());
		if(spawnMarker != null)
		{
			go.transform.position = spawnMarker.position;
			go.transform.rotation = spawnMarker.rotation;
		}
		return go.GetComponent<PowerUp>();
	}

	private void SpawnPowerUp() 
	{
		CreatePowerUpInstance(PowerUpType.BONUSSCORE, m_PowerUpSpawnMarkers[GetNextSpawnMarkerIndex()]);
	}

	#endregion
}