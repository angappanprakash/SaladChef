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
		public float			_timer;
	}

	public class CharacterDataLoader
	{
#region Variables
		private const string	DATA_ASSETS_PATH = "CharacterAssets/Data/";

		private	static List<CharacterData> m_CharactersList;

#endregion

#region Properties
		public static List<CharacterData> pCharactersList
		{
			get { return m_CharactersList; }
		}
#endregion

#region Functions
		public static void Init()
		{
			m_CharactersList = new List<CharacterData>();

			Object[] loadedObjects = Resources.LoadAll(DATA_ASSETS_PATH);
			foreach(Object obj in loadedObjects)
				m_CharactersList.Add((CharacterData)obj);
		}

		public static CharacterData GetData(string characterName)
		{
			return m_CharactersList.Find(x => x._name == characterName);
		}
#endregion
	}
}