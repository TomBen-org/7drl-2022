using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class WallJumper: WallPosition {
    public float jumpTime = 1.5f;
    private Tween _mover;
    private UnityEvent _moveCompleteEvent;
    private TurnDirector _director;
    
    private void Awake() {
        _director = GetComponent<TurnDirector>();
    }
    
    public override void Start() {
        if (_moveCompleteEvent == null) {
            _moveCompleteEvent = new UnityEvent();
        }
        _moveCompleteEvent.AddListener(delegate { _director.NextPhase(); });
    }

    public void StartTweenMove(Vector2 position) {
        Transform movedTransform = transform;

        if (_mover != null) {
            _mover.Kill();
        }

        Tween moveTween = movedTransform.DOMove(position, jumpTime);
        moveTween.OnComplete(()=> {
            _director.playerFinishedMoving = true;
        });
        _mover = moveTween;
    }
}
