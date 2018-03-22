using UnityEngine;
using System.Collections.Generic;
using GameData;

public class CameraControl : MonoBehaviour
{
#region Variables
	private const float CHARACTER_TARGET_Y_OFFSET = 0.5f;

	[SerializeField]
	private float           m_DampTime;// = 0.2f;
	[SerializeField]
	private float           m_ScreenEdgeBuffer;// = 4f;
	[SerializeField]
	private float           m_MinSize;// = 6.5f;

	[SerializeField]
	private float           m_XOffSet;
	[SerializeField]
	private float           m_YOffSet;
	[SerializeField]
	private float           m_ZOffSet;
	[SerializeField]
	private float           m_MinX;
	[SerializeField]
	private float           m_MinY;
	[SerializeField]
	private float           m_MinZ;
	[SerializeField]
	private float           m_MaxX;
	[SerializeField]
	private float           m_MaxY;
	[SerializeField]
	private float           m_MaxZ;
	[SerializeField]
	private float           m_MaxOffSetZ;
	[SerializeField]
	private float           m_FOVMax;

	private Camera          m_Camera;
	private float           m_ZoomSpeed;
	private Vector3         m_MoveVelocity;
	private Vector3         m_DesiredPosition;
	private List<Transform> m_Targets;
	private Vector3         m_DefaultPosition;
	private float           m_DefaultFov;
#endregion

#region Monobehaviour functions
	private void Awake()
	{
		m_Camera = GetComponentInChildren<Camera>();
		m_DefaultPosition = transform.position;
		m_DefaultFov = m_Camera.fieldOfView;
	}

	private void Start()
	{
		//FindAveragePosition();
		//transform.position = m_Camera.transform.position;
		//m_Camera.fieldOfView = FindRequiredFOV();
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void FixedUpdate()
	{
		//FindAveragePosition();
		FindMidPoint();

		transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
		m_Camera.fieldOfView = Mathf.SmoothDamp(m_Camera.fieldOfView, FindRequiredFOV(), ref m_ZoomSpeed, m_DampTime);
	}
#endregion

#region Class specific functions
	public void Init(List<Transform> targets)
	{
		m_Targets = targets;
	}

	private void OnPlayerSpawn(PlayerEventParams playerEventparams)
	{
		//Add players as targets here
	}

	private void FindAveragePosition()
	{
		Vector3 averagePos = new Vector3();
		int numTargets = 0;

		for (int i = 0; i < m_Targets.Count; i++)
		{
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			averagePos += m_Targets[i].position;
			numTargets++;
		}

		if (numTargets > 0)
			averagePos /= numTargets;

		//averagePos.y = transform.position.y;
		if(averagePos.z > m_MaxOffSetZ)
			averagePos.z += m_ZOffSet;

		m_DesiredPosition = new Vector3(averagePos.x + m_XOffSet, averagePos.y + m_YOffSet, averagePos.z + m_ZOffSet);
	}

	private void FindMidPoint()
	{
		if(m_Targets.Count == 0)
		{
			m_DesiredPosition = m_DefaultPosition;
			return;
		}

		Vector3 midPoint = Vector3.zero;
		float maxX = Mathf.NegativeInfinity;
		float minX = Mathf.Infinity;
		float maxZ = Mathf.NegativeInfinity;
		float minZ = Mathf.Infinity;

		foreach(Transform target in m_Targets)
		{
			if (target.position.x < minX)
				minX = target.position.x;
			if (target.position.x > maxX)
				maxX = target.position.x;
			if (target.position.z < minZ)
				minZ = target.position.z;
			if (target.position.z > maxZ)
				maxZ = target.position.z;
		}

		float midX = (minX + maxX) / 2;
		float midZ = (minZ + maxZ) / 2;
		float midY = m_Targets[0].position.y;

		midX = midX + m_XOffSet;
		midY = midY + m_YOffSet;
		midZ = midZ + m_ZOffSet;

		if((midX > m_MinX && midX < m_MaxX) && (midY > m_MinY && midY < m_MaxY) && (midZ > m_MinZ && midZ < m_MaxZ))
			m_DesiredPosition = new Vector3(midX, midY, midZ);
	}

	private float FindRequiredFOV()
	{
		if(m_Targets.Count == 0)
			return m_DefaultFov;

		Vector3 desiredLocalPos = m_Camera.transform.InverseTransformPoint(m_DesiredPosition);  
		float fov = 0f;
		for (int i = 0; i < m_Targets.Count; i++)
		{
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			Vector3 targetLocalPos = m_Camera.transform.InverseTransformPoint(m_Targets[i].position);
			Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
			fov = Mathf.Max (fov, Mathf.Abs (desiredPosToTarget.z));
			fov = Mathf.Max (fov, Mathf.Abs (desiredPosToTarget.x)/m_Camera.aspect);
		}

		fov += m_ScreenEdgeBuffer;
		fov = Mathf.Max(fov, m_MinSize);
		if (fov <= m_FOVMax)
			return fov;
		else
			return m_FOVMax; 
	}
#endregion
}