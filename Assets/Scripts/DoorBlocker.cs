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
        if (newState && !state)
        {
            AudioManager.Instance.PlayAudio(AudioManager.GameSfx.doorOpen);
        } else if (!newState && state)
        {
            AudioManager.Instance.PlayAudio(AudioManager.GameSfx.doorClose);
        }
        base.InteractWith(newState);
        _spriteRenderer.sprite = state? triggeredSprite: _defaultSprite;
        _collider.enabled = !state;

    }
}
