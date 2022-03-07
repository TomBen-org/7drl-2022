using UnityEngine;

public class Explosion: MonoBehaviour {
    private ParticleSystem _particleSystem;

    void Awake() {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    void FixedUpdate() {
        if (_particleSystem && !_particleSystem.IsAlive()) {
            Destroy(gameObject);
        }
    }
}
