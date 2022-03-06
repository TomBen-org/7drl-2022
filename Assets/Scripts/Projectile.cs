using DG.Tweening;
using UnityEngine;

public class Projectile: MonoBehaviour {

    public float moveSpeed = 10f;
    public TurnDirector.Phase updatePhase;
    public string targetLayerName;
    private TurnDirector _director;
    private Tween _mover;
    
    private void Awake() {
        _director = FindObjectOfType<TurnDirector>();
    }

    public void Setup(AbilitySetting setting) {
        Transform movedTransform = transform;

        if (_mover != null) {
            _mover.Kill();
        }

        float dist = Vector2.Distance(movedTransform.position, setting.target);
        Tween moveTween = movedTransform.DOMove(setting.target, dist/moveSpeed*10f);
        moveTween.SetEase(Ease.Linear);
        moveTween.OnComplete(() => { Destroy(gameObject); });
        _mover = moveTween;
    }

    private void Update() {
        if (_director.currentPhase == updatePhase) {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.layer == LayerMask.GetMask("MoveTarget")) {
            Destroy(gameObject);
        } else if (col.gameObject.layer == LayerMask.GetMask(targetLayerName)) {
            Body victim = col.transform.GetComponent<Body>();
            if (victim != null) {
                ApplyEffect(victim);
            }
        }
    }

    protected virtual void ApplyEffect(Body target) {
        Debug.Log(target.gameObject.name + " hit with generic projectile");
    }
}
