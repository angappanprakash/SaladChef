using System;
//using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities
{
	public class Util
	{
		public static T FindUiWidget<T>(string widgetName, Transform parent)
		{
			List<UIBehaviour> childWidgets = new List<UIBehaviour>(parent.GetComponentsInChildren<UIBehaviour>());
			UIBehaviour widget = childWidgets.Find(x => (x.name == widgetName && x.GetType() == typeof(T)));
			T t = (T)Convert.ChangeType(widget, typeof(T));
			return t;
		}

		public static void SetDefaultLocalTransform(GameObject obj)
		{
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
			obj.transform.localScale = Vector3.one;
		}

		public static GameObject FindChildObject(GameObject go, string nodeName)
		{
			Transform[] ts = go.GetComponentsInChildren<Transform>();
			foreach (Transform transform in ts)
			{
				if (transform.gameObject.name == nodeName)
				{
					return transform.gameObject;
				}
			}
			return null;
		}

		public static void ToggleRenderer(Transform parent, bool on)
		{
			foreach (Renderer renderer in parent.GetComponentsInChildren<Renderer>())
				renderer.enabled = on;
		}

		public static string GetFormattedTimeHHMMSS(float timeInSeconds)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
			return string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}

		public static string GetFormattedTimeMMSS(float timeInSeconds)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
			return string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
		}

		public static string GetCurrentBuildVersion()
		{
			string currentVersion = "1.0.0(0)";
			string versionFilePath = "version";
			TextAsset textAsset = Resources.Load(versionFilePath) as TextAsset;
			if(textAsset != null)
			{
				currentVersion = textAsset.text;
				currentVersion = currentVersion.Trim();
			}

			return currentVersion;
		}

		public static void SetLayerRecursively(GameObject go, int layerId)
		{
			go.layer = layerId;
			Transform[] ts = go.GetComponentsInChildren<Transform>();
			foreach (Transform transform in ts)
			{
				transform.gameObject.layer = layerId;
			}
		}
	}
}
