
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

public class TennisExitSceneInitiator : MonoBehaviour {

    public float offset_x;
    public float offset_y;

    void Start() {
        TennisStateRenderer renderer = new TennisStateRenderer();
        AudioEngine auEngine = new AudioEngine(0, "tennis", Settings.menu_sounds, Settings.game_sounds);

        List<WorldObject> environment = new List<WorldObject>();
        environment.Add(new TennisStaticObject("Prefabs/Tennis/Camera_Default", new Vector3(0, 10, 0), false));
        environment.Add(new TennisStaticObject("Prefabs/Tennis/Light_Default", new Vector3(0, 10, 0), false));
        environment.Add(new CanvasObject("Prefabs/Tennis/OutroLogo", true, new Vector3(0, 0, 0), false));

        TennisRuleset rules = new TennisRuleset();
        rules.Add(new TennisRule("initialization", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            TennisSoundObject tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", auEngine.getSoundForMenu("outro"), Vector3.zero);
            state.environment.Add(tso);
            state.stoppableSounds.Add(tso);
            return false;
        }));

        rules.Add(new TennisRule("soundOver", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            Application.Quit();
            return false;
        }));

        gameObject.AddComponent<TennisMenuEngine>();
        gameObject.AddComponent<TennisMenuUserInterface>();
        gameObject.GetComponent<TennisMenuEngine>().initialize(rules, environment, renderer);
        gameObject.GetComponent<TennisMenuUserInterface>().initialize(gameObject.GetComponent<TennisMenuEngine>());
        gameObject.GetComponent<TennisMenuEngine>().postEvent(new GameEvent("", "initialization", "unity"));
    }
}
