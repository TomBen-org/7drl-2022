using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class WallJumper: WallPosition {
    public float jumpTime = 1.5f;
    public GameObject landingExplosionPrefab;
    private Tween _mover;
    private TurnDirector _director;
    private ParticleSystem _particles;
    private SpriteRenderer _spriteRenderer;

    private Vector2 _destination;
    
    private void Awake() {
        _director = GetComponent<TurnDirector>();
        _particles = GetComponentInChildren<ParticleSystem>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _particles.Stop();
    }
    
    public void Update() {
        if (_director.currentPhase == TurnDirector.Phase.Moving) {
            
        }
    }

    public void StartTweenMove(Vector2 destination) {
        _destination = destination;
        Transform movedTransform = transform;
        AudioManager.Instance.PlayAudio(AudioManager.GameSfx.jumpInit);

        if (_mover != null) {
            _mover.Kill();
        }

        Tween moveTween = movedTransform.DOMove(destination, jumpTime);
        moveTween.OnComplete(()=> {
            SetFacing();
            _particles.Stop();
            _spriteRenderer.enabled = true;
            Instantiate(landingExplosionPrefab, _spriteRenderer.transform.position, Quaternion.identity);
            _director.playerFinishedMoving = true;
            AudioManager.Instance.PlayAudio(AudioManager.GameSfx.jumpLanding);
        });
        Instantiate(landingExplosionPrefab, _spriteRenderer.transform.position, Quaternion.identity);
        _spriteRenderer.enabled = false;
        _particles.Play();
        _mover = moveTween;
    }
}
