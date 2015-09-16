using UnityEngine;
using System.Collections;

public class TennisUserInterface : MonoBehaviour {

    public TennisGameEngine engine;

    private bool initialized;

    public void initialize(TennisGameEngine engine) {
        this.engine = engine;
        initialized = true;
    }

	public void Update() {
        if (!initialized) {
            return;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            engine.postEvent(new UIEvent("up", "difficulty", "player0"));
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            engine.postEvent(new UIEvent("down", "difficulty", "player0"));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            engine.postEvent(new UIEvent("left", "move", "player0"));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            engine.postEvent(new UIEvent("right", "move", "player0"));
		}
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			engine.postEvent(new UIEvent("down", "move", "player0"));
		}
        if (Input.GetKeyDown(KeyCode.Q)) {
            engine.postEvent(new UIEvent("movement", "settings", "player0"));
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            engine.postEvent(new UIEvent("difficulty", "settings", "player0"));
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            engine.postEvent(new UIEvent("left", "position", "player0"));
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            engine.postEvent(new UIEvent("right", "position", "player0"));
        }
        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)) {
            engine.postEvent(new UIEvent("center", "position", "player0"));
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            engine.postEvent(new UIEvent("escape", "action", "player0"));
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            engine.postEvent(new UIEvent("pause", "action", "player0"));
		}
		if (Input.GetKeyDown(KeyCode.Return)) {
			engine.postEvent(new UIEvent("enter", "action", "player0"));
		}
		if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
			engine.postEvent(new UIEvent("enter", "action", "player0"));
		}
		if (Input.anyKeyDown) {
			engine.postEvent(new UIEvent("any", "action", "player0"));
		}
	}
	
}