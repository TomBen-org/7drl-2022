using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public enum InteractionSfx
    {
		interact, negativeInteract, positiveInteract
    }

	public enum GameSfx
	{
		spawn, die, jumpInit, jumpLanding
	}
	public enum MusicType
	{
		gameplay, menu
	}

	public static AudioManager Instance;

	[SerializeField]
	private AudioClip interactClip;
	[SerializeField]
	private AudioClip negativeInteractClip;
	[SerializeField]
	private AudioClip positiveInteractClip;

	[SerializeField]
	private AudioClip dieClip;
	[SerializeField]
	private AudioClip spawnClip;
	[SerializeField]
	private AudioClip jumpInitClip;
	[SerializeField]
	private AudioClip jumpLandingClip;


	private AudioSource interactionAudioSource;
	private AudioSource gameAudioSource;
	private AudioSource musicAudioSource;


	void Awake()
	{
		SetInstance();
		Init();
	}

	private void SetInstance()
	{
		if (Instance != null)
		{

			Debug.LogWarning($"There is more than one {this.GetType()} in this scene.");
		}
		Instance = this;
	}

	private void Init()
    {
		AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length < 3)
        {
			Debug.LogWarning($"AudioManager expects 3 AudioSource Components, but only {audioSources.Length} were provided.");
        }

		this.interactionAudioSource = audioSources[0];
		this.gameAudioSource = audioSources[1];
		this.musicAudioSource = audioSources[2];
	}

	public void PlayAudio(InteractionSfx interactionSound, bool looped = false)
    {
        switch (interactionSound)
        {
			case InteractionSfx.interact:
                {
					interactionAudioSource.clip = interactClip;
					break;
				}
			case InteractionSfx.positiveInteract:
				{
					interactionAudioSource.clip = positiveInteractClip;
					break;
				}
			case InteractionSfx.negativeInteract:
				{
					interactionAudioSource.clip = negativeInteractClip;
					break;
				}
		}
		interactionAudioSource.loop = looped;
		interactionAudioSource.Play();
    }

	public void PlayAudio(GameSfx gameSound, bool looped = false)
	{
		switch (gameSound)
		{
			case GameSfx.spawn:
				{
					gameAudioSource.clip = spawnClip;
					break;
				}
			case GameSfx.die:
				{
					gameAudioSource.clip = dieClip;
					break;
				}
			case GameSfx.jumpInit:
				{
					gameAudioSource.clip = jumpInitClip;
					break;
				}
			case GameSfx.jumpLanding:
				{
					gameAudioSource.clip = jumpLandingClip;
					break;
				}
		}
		gameAudioSource.loop = looped;
		gameAudioSource.Play();
	}


	public void PlayAudio(MusicType music, bool looped = true)
	{
		Debug.LogWarning("no music exists yet");
		switch (music)
		{
			case MusicType.menu:
				{
					//musicAudioSource.clip = menuMusicClip;
					break;
				}
			case MusicType.gameplay:
				{
					//musicAudioSource.clip = gameplayMusicClip;
					break;
				}
		}
		musicAudioSource.loop = looped;
		musicAudioSource.Play();
	}


	void OnDestroy()
	{
		Instance = null;
	}
}
