using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
#region Variables
	[SerializeField]
	private AudioSource			m_BgMusicChannel;
	[SerializeField]
	private AudioClip           m_BgInGame;
	[SerializeField]
	private List<AudioSource>   m_SfxChannels;

	private static AudioManager	mInstance;
	#endregion

	#region Properties
	public static AudioManager pInstance
	{
		get { return mInstance; }
	}
	#endregion

	#region Monobehaviour functions
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	private void OnDestroy()
	{
		mInstance = null;
	}
	#endregion

	#region Class specific functions
	public void Init()
	{
		mInstance = this;
	}

	public void PlayInGameBGM()
	{
		m_BgMusicChannel.clip = m_BgInGame;

		m_BgMusicChannel.Play();
	}

	private void Update()
	{
	}

	public void PlaySound(AudioClip clip, bool isLoop = false)
	{
		for (int i = 0; i < m_SfxChannels.Count; i++)
		{
			if (!m_SfxChannels[i].isPlaying)
			{
				m_SfxChannels[i].clip = clip;
				m_SfxChannels[i].loop = isLoop;
				m_SfxChannels[i].Play();
			}
		}
	}

	public void StopChannel(AudioSource audioSource)
	{
		if (audioSource != null)
		{
			audioSource.Stop();
		}
	}

	public void ResetAudio()
	{
		if(m_BgMusicChannel.isPlaying)
			m_BgMusicChannel.clip = null;
		for (int i = 0; i < m_SfxChannels.Count; i++)
		{
			if(m_SfxChannels[i].isPlaying)
				m_SfxChannels[i].clip = null;
		}
	}
	#endregion
}
