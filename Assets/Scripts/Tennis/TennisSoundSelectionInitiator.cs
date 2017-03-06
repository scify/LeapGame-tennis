
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
using System.Collections.Generic;

public class TennisSoundSelectionInitiator : MonoBehaviour {

    public float offset_y;
    private TennisStaticObject movingCamera = new TennisStaticObject("Prefabs/Tennis/Camera_Default", new Vector3(0, 10, 0), false);

    void Start() {
        TennisStateRenderer renderer = new TennisStateRenderer();
        AudioEngine auEngine = new AudioEngine(0, "tennis", Settings.menu_sounds, Settings.game_sounds);

        List<WorldObject> environment = new List<WorldObject>();
        environment.Add(movingCamera);
		environment.Add(new TennisStaticObject("Prefabs/Tennis/Light_Default", new Vector3(0, 10, 0), false));
        environment.Add(new CanvasObject("Prefabs/Tennis/Logos", true, new Vector3(10000, 0, 0), false));
        int i = 0;

        foreach (string s in auEngine.getSettingsAudioForMenu()) {
            if (s == Settings.default_soundset) {
                environment.Add(new TennisMenuItem("Prefabs/Tennis/ButtonSelected", Settings.default_soundset_btn_name, s, s + "_", null, new Vector3(0, 0, -offset_y), false, true));
            } else {
                //environment.Add(new TennisMenuItem("Prefabs/Tennis/ButtonDefault", s, s, s + "_", null, new Vector3(0, 0, i++ * offset_y), false, false));
            }
        }

        TennisRuleset rules = new TennisRuleset();
        rules.Add(new TennisRule("initialization", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            AudioClip audioClip;
            audioClip = auEngine.getSoundForMenu("voice_selection");
            state.timestamp = 0;
            TennisSoundObject tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", audioClip, Vector3.zero);
            state.environment.Add(tso);
            state.stoppableSounds.Add(tso);
            Settings.previousMenu = "mainMenu";
            return false;
        }));

        rules.Add(new TennisRule("action", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            if (eve.payload.Equals("enter")) {
                foreach (WorldObject obj in state.environment) {
                    if (obj is TennisMenuItem) {
                        TennisMenuItem temp = obj as TennisMenuItem;
                        if (temp.selected) {
                            Settings.menu_sounds = temp.target;
                            Settings.game_sounds = temp.target;
                        }
                    }
                }
                Application.LoadLevel("mainMenu");
            }
            return true;
        }));

        rules.Add(new TennisRule("move", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            state.timestamp++;
            foreach (TennisSoundObject tennisSo in state.stoppableSounds) {
                state.environment.Remove(tennisSo);
            }
            state.stoppableSounds.Clear();
            TennisMenuItem previous = null;
            bool change = false;
            AudioClip audioClip;
            TennisSoundObject tso;
            foreach (WorldObject obj in state.environment) {
                if (obj is TennisMenuItem) {
                    TennisMenuItem temp = obj as TennisMenuItem;
                    if (temp.selected) {
                        if (eve.payload == "_up" || eve.payload == "left") {
                            if (previous == null) {
                                audioClip = auEngine.getSoundForPlayer("boundary", Vector3.up);
                                tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", audioClip, Vector3.zero);
                                state.environment.Add(tso);
                                state.stoppableSounds.Add(tso);
                                break;
                            }
                            temp.selected = false;
                            temp.prefab = temp.prefab.Replace("Selected", "Default");
                            previous.selected = true;
                            previous.prefab = previous.prefab.Replace("Default", "Selected");
                            tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", auEngine.getSoundForPlayer(previous.audioMessageCode + Random.Range(1, 5)), Vector3.zero);
                            state.environment.Add(tso);
                            state.stoppableSounds.Add(tso);
                            break;
                        } else {
                            change = true;
                        }
                    } else if (change) {
                        temp.selected = true;
                        temp.prefab = temp.prefab.Replace("Default", "Selected");
                        previous.prefab = previous.prefab.Replace("Selected", "Default");
                        previous.selected = false;
                        change = false;
                        tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", auEngine.getSoundForPlayer(temp.audioMessageCode + Random.Range(1, 5)), Vector3.zero);
                        state.environment.Add(tso);
                        state.stoppableSounds.Add(tso);
                        break;
                    }
                    previous = temp;
                }
            }
            if (change) {
                audioClip = auEngine.getSoundForPlayer("boundary", Vector3.down);
                tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", audioClip, Vector3.zero);
                state.environment.Add(tso);
                state.stoppableSounds.Add(tso);
            }
            foreach (WorldObject obj in state.environment) {
                if (obj is TennisMenuItem) {
                    TennisMenuItem temp = obj as TennisMenuItem;
                    if (temp.selected) {
                        movingCamera.position = new Vector3(0, 10, Mathf.Clamp(temp.position.z, 6 * offset_y, 0));
                        break;
                    }
                }
            }
            return true;
        }));

        gameObject.AddComponent<TennisMenuEngine>();
        gameObject.AddComponent<TennisMenuUserInterface>();
        gameObject.GetComponent<TennisMenuEngine>().initialize(rules, environment, renderer);
        gameObject.GetComponent<TennisMenuUserInterface>().initialize(gameObject.GetComponent<TennisMenuEngine>());
        gameObject.GetComponent<TennisMenuEngine>().postEvent(new GameEvent("", "initialization", "unity"));
    }
}
