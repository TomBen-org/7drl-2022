using UnityEngine;

public class BodySpriteStatus: Body {
    private Sprite _defaultSprite;
    public SpriteRenderer statusRenderer;
    
    public Sprite stunnedSprite;

    void Awake() {
        _defaultSprite = statusRenderer.sprite;
    }

    public override void UpdateStates() {
        base.UpdateStates();
        UpdateStatus();
    }

    public override void Stun(int turns) {
        base.Stun(turns);
        UpdateStatus();
    }

    private void UpdateStatus() {
        if (isStunned) {
            statusRenderer.sprite = stunnedSprite;
        }
        else {
            statusRenderer.sprite = _defaultSprite;
        }
    }
    
}
