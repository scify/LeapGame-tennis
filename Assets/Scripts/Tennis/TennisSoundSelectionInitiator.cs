using UnityEngine;
using System.Collections.Generic;

public class TennisSoundSelectionInitiator : MonoBehaviour {

    public float offset_x;
    public float offset_y;

    void Start() {
        TennisStateRenderer renderer = new TennisStateRenderer();
        AudioEngine auEngine = new AudioEngine(0, "tennis", Settings.menu_sounds, Settings.game_sounds);

        List<WorldObject> environment = new List<WorldObject>();
        environment.Add(new TennisStaticObject("Prefabs/Tennis/Camera_Default", new Vector3(0, 10, 0), false));
		environment.Add(new TennisStaticObject("Prefabs/Tennis/Light_Default", new Vector3(0, 10, 0), false));
		environment.Add(new CanvasObject("Prefabs/Tennis/Logos", true, new Vector3(10000, 0, 0), false));
        environment.Add(new TennisMenuItem("Prefabs/Tennis/ButtonSelected", "Θοδωρής", "thodoris_", null, null, new Vector3(0, 0, -offset_y), false, true));
        environment.Add(new TennisMenuItem("Prefabs/Tennis/ButtonDefault", "Ηλέκτρα", "electra_", null, null, new Vector3(0, 0, offset_y), false));

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
                Application.LoadLevel("mainMenu");
            }
            return true;
        }));

        rules.Add(new TennisRule("move", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            foreach (TennisSoundObject Tennisso in state.stoppableSounds) {
                state.environment.Remove(Tennisso);
            }
            state.stoppableSounds.Clear();
            AudioClip audioClip = null;
            foreach (WorldObject obj in state.environment) {
                if (obj is TennisMenuItem) {
                    TennisMenuItem temp = obj as TennisMenuItem;
                    if (temp.selected) {
                        temp.selected = false;
                        temp.prefab = temp.prefab.Replace("Selected", "Default");
                    } else {
                        temp.selected = true;
                        audioClip = auEngine.getSoundForPlayer(temp.target + Random.Range(1, 5));
                        switch (temp.target) {
                            case "thodoris_":
                                Settings.menu_sounds = "default";
                                break;
                            case "electra_":
                                Settings.menu_sounds = "electra";
                                break;
                        }
                        temp.prefab = temp.prefab.Replace("Default", "Selected");
                    }
                }
            }
            TennisSoundObject tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", audioClip, Vector3.zero);
            state.environment.Add(tso);
            state.stoppableSounds.Add(tso);
            return false;
        }));

        gameObject.AddComponent<TennisMenuEngine>();
        gameObject.AddComponent<TennisMenuUserInterface>();
        gameObject.GetComponent<TennisMenuEngine>().initialize(rules, environment, renderer);
        gameObject.GetComponent<TennisMenuUserInterface>().initialize(gameObject.GetComponent<TennisMenuEngine>());
        gameObject.GetComponent<TennisMenuEngine>().postEvent(new GameEvent("", "initialization", "unity"));
    }
}
