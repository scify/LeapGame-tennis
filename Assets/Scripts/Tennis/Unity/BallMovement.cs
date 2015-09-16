using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallMovement : Movement {

	private Rigidbody body;
	private float height;
    private float latest = 0f;

	void Start() {
		body = gameObject.GetComponent<Rigidbody>();
		height = -5;
	}

    public override void OnCollisionEnter(Collision collision) {
        float now = Time.realtimeSinceStartup;
        if (now - latest < 0.1f) {
            return;
        }
        latest = now;
        engine.postEvent(new GameEvent(collision.gameObject.tag, "bounce", "ball"));
        if (collision.gameObject.tag == "racket" || collision.gameObject.tag == "wall") {
            int directionMod = collision.gameObject.tag == "wall" ? 1 : -1;
			float x = body.transform.position.x > -1 && body.transform.position.x < 1 ? 0 : body.transform.position.x;
            int pos = body.transform.position.x > -1 && body.transform.position.x < 1 ? 0 : body.transform.position.x > 0 ? 1 : -1;
            body.transform.position = new Vector3(x, height, collision.gameObject.transform.position.z - directionMod * 0.8f);
			body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
			int min = -1 - directionMod * (int) pos;
            int max = 2 - directionMod * (int)pos;
            int xMod = Random.Range(min, max);
			int stateTarget = (engine.getState() as TennisGameState).target;
			if (stateTarget >= -1 && stateTarget <= 1) {
                xMod = stateTarget - directionMod * (int) pos;
			}
            body.AddForce(new Vector3(directionMod * xMod * 200, 280, -directionMod * Settings.size_mod_z * 400));
            engine.postEvent(new GameEvent((directionMod * (xMod - min - 1)).ToString(), "hit", "ball"));
		}
	}
}