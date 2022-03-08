using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private const int MIN_AUDIO_SOURCES = 3;

	private const bool LOG_ACTIVE = false;

	public enum MusicType
	{
		gameplay, menu
	}
	private const int MUSIC_INDEX = 0;
	public enum InteractionSfx
    {
		interact, negativeInteract, positiveInteract
    }
	private const int INTERACT_INDEX = 1;

	public enum GameSfx
	{
		spawn, die, jumpInit, jumpLanding, doorOpen, doorClose, stunShoot, stunHit, canonShoot, canonHit
	}
	private const int GAME_INDEX_START = 2;

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
	[SerializeField]
	private AudioClip doorOpenClip;
	[SerializeField]
	private AudioClip doorCloseClip;
	[SerializeField]
	private AudioClip stunShootClip;
	[SerializeField]
	private AudioClip stunHitClip;
	[SerializeField]
	private AudioClip canonShootClip;
	[SerializeField]
	private AudioClip canonHitClip;

	[SerializeField]
	private AudioClip gameplayMusicClip;
	[SerializeField]
	private AudioClip menuMusicClip;

	private AudioSource interactionAudioSource;
	private AudioSource musicAudioSource;
	private AudioSource[] gameAudioSources;

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
        if (audioSources.Length < MIN_AUDIO_SOURCES)
        {
			Debug.LogWarning($"AudioManager expects at least {MIN_AUDIO_SOURCES} AudioSource Components, but only {audioSources.Length} were provided.");
        }
        else
        {
			gameAudioSources = new AudioSource[audioSources.Length - GAME_INDEX_START];
			Array.Copy(audioSources, GAME_INDEX_START, gameAudioSources, 0, audioSources.Length - GAME_INDEX_START);
        }
		interactionAudioSource = audioSources[0];
		musicAudioSource = audioSources[1];
	}

	public void PlayAudio(InteractionSfx interactionSound, bool looped = false)
    {
		Log($"Playing audio: {interactionSound}");
		switch (interactionSound)
        {
			case InteractionSfx.interact:
                {
					PlayClipAt(interactClip, looped, interactionAudioSource);
					return;
				}
			case InteractionSfx.positiveInteract:
				{
					PlayClipAt(positiveInteractClip, looped, interactionAudioSource);
					return;
				}
			case InteractionSfx.negativeInteract:
				{
					PlayClipAt(negativeInteractClip, looped, interactionAudioSource);
					return; 
				}
		}
    }

	public void PlayAudio(GameSfx gameSound, bool looped = false)
	{
		Log($"Playing audio: {gameSound}");
		switch (gameSound)
		{
			case GameSfx.spawn:
				{
					PlayClipAt(spawnClip, looped, gameAudioSources);
					return;
				}
			case GameSfx.die:
				{
					PlayClipAt(dieClip, looped, gameAudioSources);
					return;
				}
			case GameSfx.jumpInit:
				{
					PlayClipAt(jumpInitClip, looped, gameAudioSources);
					return;
				}
			case GameSfx.jumpLanding:
				{
					PlayClipAt(jumpLandingClip, looped, gameAudioSources);
					return;
				}
			case GameSfx.doorOpen:
				{
					PlayClipAt(doorOpenClip, looped, gameAudioSources);
					return;
				}
			case GameSfx.doorClose:
				{
					PlayClipAt(doorCloseClip, looped, gameAudioSources);
					return;
				}
			case GameSfx.stunShoot:
				{
					PlayClipAt(stunShootClip, looped, gameAudioSources);
					return;
				}
			case GameSfx.stunHit:
				{
					PlayClipAt(stunHitClip, looped, gameAudioSources);
					return;
				}
			case GameSfx.canonShoot:
				{
					PlayClipAt(canonShootClip, looped, gameAudioSources);
					return;
				}
			case GameSfx.canonHit:
				{
					PlayClipAt(canonHitClip, looped, gameAudioSources);
					return;
				}
		}
	}

	public void PlayAudio(MusicType music, bool looped = true)
	{
		Log($"Playing music audio: {music}");
		switch (music)
		{
			case MusicType.gameplay:
				{
					PlayClipAt(gameplayMusicClip, looped, musicAudioSource);
					return;
				}
			case MusicType.menu:
				{
					PlayClipAt(menuMusicClip, looped, musicAudioSource);
					break;
				}
		}
	}

	private void PlayClipAt(AudioClip clip, bool looped, AudioSource[] sources)
    {
		PlayClipAt(clip, looped, GetMostProgressedSource(sources));
    }
	private AudioSource GetMostProgressedSource(AudioSource[] sources)
	{
		if (sources.Length < 1)
		{
			Debug.LogWarning("Passed an array with less than 1 audio source");
			return null;
		}
		int maxIndex = 0;
		float maxTime = -1;
		for (int i = 0; i < sources.Length; i++)
		{
			if (!sources[i].isPlaying)
			{
				return sources[i];
			}
			if (sources[i].time > maxTime)
			{
				maxTime = sources[i].time;
				maxIndex = i;
			}
		}
		return sources[maxIndex];
	}

	private void PlayClipAt(AudioClip clip, bool looped, AudioSource source)
	{
		source.clip = clip;
		source.loop = looped;
		source.Play();
	}

	void OnDestroy()
	{
		Instance = null;
	}

	/// <summary>
	/// Logs depending on LOG_ACTIVE
	/// </summary>
	/// <param name="output"></param>
	private void Log(string output)
    {
        if (LOG_ACTIVE)
        {
#pragma warning disable CS0162 //ignore unreachable code warning
            Debug.Log(output);
#pragma warning restore CS0162 
        }
    }
}