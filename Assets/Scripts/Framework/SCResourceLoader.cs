using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SCResourceLoader : MonoBehaviour
{
	public static void LoadScene(string sceneName, bool waitForInput = false)
	{
		if (string.IsNullOrEmpty(sceneName))
			return;
		SceneManager.LoadScene("MainMenu");
	}
}