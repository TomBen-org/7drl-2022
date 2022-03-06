using UnityEngine;

public class DoorBlocker : ButtonTarget {
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private Sprite _defaultSprite;
    public Sprite triggeredSprite;
    
    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _defaultSprite = _spriteRenderer.sprite;
    }

    public override void InteractWith(bool newState) {
        base.InteractWith(newState);
        _spriteRenderer.sprite = state? triggeredSprite: _defaultSprite;
        _collider.enabled = !state;
    }
}
