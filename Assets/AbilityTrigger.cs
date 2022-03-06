using UnityEngine;

public class AbilityTrigger : MonoBehaviour {
    private Collider2D _playerCollider;
    protected AbilitySetting _setting;

    void Awake() {
        _playerCollider = FindObjectOfType<TurnDirector>().GetComponent<Collider2D>();
    }

    public void Setup(AbilitySetting setting) {
        _setting = setting;
    }
    
    private void OnTriggerEnter2D(Collider2D col) {
        if (col == _playerCollider) {
            TriggerMe();    
        }
    }

    protected virtual void TriggerMe() {
        Debug.Log("Triggered Generic ability");
        Destroy(gameObject);
    }
}
