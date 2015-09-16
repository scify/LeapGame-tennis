using UnityEngine;
using System.Collections.Generic;

public class TennisMainMenuInitiator : MonoBehaviour {

    public float offset_x;
    public float offset_y;

    void Start() {
        TennisStateRenderer renderer = new TennisStateRenderer();
        AudioEngine auEngine = new AudioEngine(0, "tennis", Settings.menu_sounds, Settings.game_sounds);

        List<WorldObject> environment = new List<WorldObject>();
        environment.Add(new TennisStaticObject("Prefabs/Tennis/Camera_Default", new Vector3(0, 10, 0), false));
		environment.Add(new TennisStaticObject("Prefabs/Tennis/Light_Default", new Vector3(0, 10, 0), false));
		environment.Add(new CanvasObject("Prefabs/Tennis/Logos", true, new Vector3(10000, 0, 0), false));
        environment.Add(new TennisMenuItem("Prefabs/Tennis/ButtonSelected", "Οδηγίες", "tutorial", "tutorials", auEngine.getSoundForMenu("tutorials"), new Vector3(0, 0, -offset_y), false, true));
        environment.Add(new TennisMenuItem("Prefabs/Tennis/ButtonDefault", "Νέο Παιχνίδι", "newGame", "new_game", auEngine.getSoundForMenu("new_game"), new Vector3(0, 0, 0), false));        
        environment.Add(new TennisMenuItem("Prefabs/Tennis/ButtonDefault", "Έξοδος", "exitScene", "exit", auEngine.getSoundForMenu("exit"), new Vector3(0, 0, offset_y), false));

        TennisRuleset rules = new TennisRuleset();
        rules.Add(new TennisRule("initialization", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            AudioClip audioClip;
            if (Settings.just_started) {
                Settings.just_started = false;
                audioClip = auEngine.getSoundForMenu("game_intro");
                state.timestamp = 0;
            } else {
                audioClip = auEngine.getSoundForMenu("tutorials");
                state.timestamp = 1;
            }
            TennisSoundObject tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", audioClip, Vector3.zero);
            state.environment.Add(tso);
            state.stoppableSounds.Add(tso);
            Settings.previousMenu = "mainMenu";
            return false;
        }));

        rules.Add(new TennisRule("soundSettings", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            Settings.menu_sounds = eve.payload;
            auEngine = new AudioEngine(0, "Tic-Tac-Toe", Settings.menu_sounds, Settings.game_sounds);
            foreach (WorldObject wo in state.environment) {
                if (wo is TennisMenuItem) {
                    (wo as TennisMenuItem).audioMessage = auEngine.getSoundForMenu((wo as TennisMenuItem).audioMessageCode);
                }
            }
            return false;
        }));

        rules.Add(new TennisRule("soundOver", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            if (state.timestamp == 0) {
                AudioClip audioClip = auEngine.getSoundForMenu("tutorials");
                TennisSoundObject tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", audioClip, Vector3.zero);
                state.environment.Add(tso);
                state.stoppableSounds.Add(tso);
                state.timestamp = 1;
            }
            return false;
        }));

        rules.Add(new TennisRule("action", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            if (eve.payload.Equals("escape")) {
                Application.LoadLevel("mainMenu");
                return false;
            }
            return true;
        }));

        rules.Add(new TennisRule("action", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            if (eve.payload.Equals("enter")) {
                foreach (WorldObject obj in state.environment) {
                    if (obj is TennisMenuItem) {
                        if ((obj as TennisMenuItem).selected) {
                            Application.LoadLevel((obj as TennisMenuItem).target);
                            return false;
                        }
                    }
                }
            }
            return true;
        }));

        rules.Add(new TennisRule("move", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            state.timestamp++;
            foreach (TennisSoundObject Tennisso in state.stoppableSounds) {
                state.environment.Remove(Tennisso);
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
                            tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", previous.audioMessage, Vector3.zero);
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
                        tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", temp.audioMessage, Vector3.zero);
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
            return true;
        }));

        gameObject.AddComponent<TennisMenuEngine>();
        gameObject.AddComponent<TennisMenuUserInterface>();
        gameObject.GetComponent<TennisMenuEngine>().initialize(rules, environment, renderer);
        gameObject.GetComponent<TennisMenuUserInterface>().initialize(gameObject.GetComponent<TennisMenuEngine>());
        gameObject.GetComponent<TennisMenuEngine>().postEvent(new GameEvent("", "initialization", "unity"));
    }
}
