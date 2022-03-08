using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private const int N_AUDIO_SOURCES = 4;

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
		spawn, die, jumpInit, jumpLanding, doorOpen, stunShoot, stunHit, canonShoot, canonHit
	}
	private const int GAME_INDEX1 = 2;
	private const int GAME_INDEX2 = 3;

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


	private AudioSource[] audioSources = new AudioSource[N_AUDIO_SOURCES];

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
		audioSources = GetComponents<AudioSource>();
        if (audioSources.Length < 4)
        {
			Debug.LogWarning($"AudioManager expects 4 AudioSource Components, but only {audioSources.Length} were provided.");
        }
	}

	public void PlayAudio(InteractionSfx interactionSound, bool looped = false)
    {

        switch (interactionSound)
        {
			case InteractionSfx.interact:
                {
					PlayClipAt(interactClip, looped, audioSources[INTERACT_INDEX]);
					return;
				}
			case InteractionSfx.positiveInteract:
				{
					PlayClipAt(positiveInteractClip, looped, audioSources[INTERACT_INDEX]);
					return;
				}
			case InteractionSfx.negativeInteract:
				{
					PlayClipAt(negativeInteractClip, looped, audioSources[INTERACT_INDEX]);
					return; 
				}
		}
    }

	public void PlayAudio(GameSfx gameSound, bool looped = false)
	{
		switch (gameSound)
		{
			case GameSfx.spawn:
				{
					PlayClipAt(spawnClip, looped, audioSources[GAME_INDEX1], audioSources[GAME_INDEX2]);
					return;
				}
			case GameSfx.die:
				{
					PlayClipAt(dieClip, looped, audioSources[GAME_INDEX1], audioSources[GAME_INDEX2]);
					return;

				}
			case GameSfx.jumpInit:
				{
					PlayClipAt(jumpInitClip, looped, audioSources[GAME_INDEX1], audioSources[GAME_INDEX2]);
					return;
				}
			case GameSfx.jumpLanding:
				{
					PlayClipAt(jumpLandingClip, looped, audioSources[GAME_INDEX1], audioSources[GAME_INDEX2]);
					return;
				}
			case GameSfx.doorOpen:
				{
					PlayClipAt(doorOpenClip, looped, audioSources[GAME_INDEX1], audioSources[GAME_INDEX2]);
					return;
				}
			case GameSfx.stunShoot:
				{
					PlayClipAt(stunShootClip, looped, audioSources[GAME_INDEX1], audioSources[GAME_INDEX2]);
					return;
				}
			case GameSfx.stunHit:
				{
					PlayClipAt(stunHitClip, looped, audioSources[GAME_INDEX1], audioSources[GAME_INDEX2]);
					return;
				}
			case GameSfx.canonShoot:
				{
					PlayClipAt(canonShootClip, looped, audioSources[GAME_INDEX1], audioSources[GAME_INDEX2]);
					return;
				}
			case GameSfx.canonHit:
				{
					PlayClipAt(canonHitClip, looped, audioSources[GAME_INDEX1], audioSources[GAME_INDEX2]);
					return;
				}
		}
	}


	public void PlayAudio(MusicType music, bool looped = true)
	{
		Debug.LogWarning("no music exists yet");
		switch (music)
		{
			case MusicType.gameplay:
				{
					PlayClipAt(gameplayMusicClip, looped, audioSources[MUSIC_INDEX]);
					return;
				}
			case MusicType.menu:
				{
					PlayClipAt(menuMusicClip, looped, audioSources[MUSIC_INDEX]);
					break;
				}
		}
	}

	private void PlayClipAt(AudioClip clip, bool looped, AudioSource source, AudioSource altSource = null)
    {
		if(altSource is not null && source.isPlaying && !altSource.isPlaying)
        {
			PlayClipAt(clip, looped, altSource, null);
        }
		source.clip = clip;
		source.loop = looped;
		source.Play();
    }

	void OnDestroy()
	{
		Instance = null;
	}
}
