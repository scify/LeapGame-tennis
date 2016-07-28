
/**
 * Copyright 2016 , SciFY NPO - http://www.scify.org
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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