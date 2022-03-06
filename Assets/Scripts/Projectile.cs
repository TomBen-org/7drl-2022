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
        Rigidbody2D movedRb = GetComponent<Rigidbody2D>();

        if (_mover != null) {
            _mover.Kill();
        }

        float dist = Vector2.Distance(movedRb.position, setting.target);
        Tween moveTween = movedRb.DOMove(setting.target, dist/moveSpeed*10f);
        moveTween.SetEase(Ease.Linear);
        moveTween.OnComplete(() => { Destroy(gameObject); });
        _mover = moveTween;
    }

    private void Update() {
        if (_director.currentPhase == updatePhase) {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("Projectile collides");
        int wallMask = LayerMask.NameToLayer("MoveTarget");
        int targetMask = LayerMask.NameToLayer(targetLayerName);
        
        if (col.gameObject.layer == wallMask) {
            Debug.Log("hit wall");
            DestroyProjectile();
        } else if (col.gameObject.layer == targetMask) {
            Debug.Log("hit enemy");
            Body victim = col.transform.GetComponent<Body>();
            if (victim != null) {
                ApplyEffect(victim);
                DestroyProjectile();
            }
        }
    }

    protected virtual void ApplyEffect(Body target) {
        Debug.Log(target.gameObject.name + " hit with generic projectile");
    }

    private void DestroyProjectile() {
        _mover.Kill();
        Destroy(gameObject);
    }
}
