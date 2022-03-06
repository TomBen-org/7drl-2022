using UnityEngine;

public class Body: MonoBehaviour {
    public bool isStunned => stunnedTurns > 0;
    public int stunnedTurns;
    
    public void UpdateStates() {
        if (stunnedTurns > 0) {
            stunnedTurns -= 1;
        }
    }
    
    public void Stun(int turns) {
        stunnedTurns += turns;
    }
}
