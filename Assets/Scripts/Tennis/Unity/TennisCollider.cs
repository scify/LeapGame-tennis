using UnityEngine;
using System.Collections;

public class TennisCollider : MonoBehaviour {

    public GameEngine engine;

    void OnTriggerEnter(Collider other) {
        engine.postEvent(new GameEvent(gameObject.tag, "bounce", other.gameObject.tag));
    }
}
