using UnityEngine;
using System.Collections;

namespace Utilities
{
	public class PersistentObject : MonoBehaviour
	{
		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
	}
}