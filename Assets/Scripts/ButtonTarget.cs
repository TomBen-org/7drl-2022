using UnityEngine;

public class ButtonTarget: MonoBehaviour {
    public bool state;
    
    public virtual void InteractWith(bool newState) {
        this.state = newState;
    }
}
