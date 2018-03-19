using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameData
{
    [CreateAssetMenu(menuName = "GameAssets/Character")]
	public class CharacterData : ScriptableObject
	{

		public string			_name;
		public GameObject		_prefab;
	}
}