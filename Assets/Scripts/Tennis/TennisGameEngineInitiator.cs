
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

public class TennisGameEngineInitiator : MonoBehaviour {

    public float offset_x = 0f;
	private float latestSpeed = 0f;
	private bool positionalMovement = Settings.positionalMovement;
    private int ballTarget = 0;
    private TennisMovingObject ball;
    private bool handleDifficulty = false;
    private TextCanvasObject gui;

	void Start () {
        TennisStateRenderer renderer = new TennisStateRenderer();
        AudioEngine auEngine = new AudioEngine(0, "tennis", Settings.menu_sounds, Settings.game_sounds);

        List<Actor> actors = new List<Actor>();
        actors.Add(new TennisActor("racket", "Prefabs/Tennis/Racket", new Vector3(0, -5, Settings.size_mod_z * - 6), false, null));

        List<WorldObject> environment = new List<WorldObject>();
        ball = new TennisMovingObject("Prefabs/Tennis/Ball", new Vector3(0, -1, Settings.size_mod_z * 5.9f), false);
        environment.Add(ball);
        environment.Add(new TennisStaticObject("Prefabs/Tennis/MainCamera", new Vector3(0, 1, (Settings.size_mod_z * -6f) - 5.5f), false));
        environment.Add(new TennisStaticObject("Prefabs/Tennis/Light", new Vector3(0, 0, 0), false));
        environment.Add(new TennisStaticObject("Prefabs/Tennis/Field", new Vector3(0, -7, 0), false));
        environment.Add(new TennisStaticObject("Prefabs/Tennis/Wall", new Vector3(0, -5, Settings.size_mod_z * 6), false));
		environment.Add(new CanvasObject("Prefabs/Tennis/LeapLogo", true, new Vector3(0, 0, 0), false));
        gui = new TextCanvasObject("Prefabs/Tennis/GUI_Element", true, "Score: 0\nLevel: 0\nLives: 0", new Vector3(0, 0, 0), false);
        environment.Add(gui);

        List<Player> players = new List<Player>();
        players.Add(new Player("player0", "player0"));

        TennisRuleset rules = new TennisRuleset();
        rules.Add(new TennisRule("initialization", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
			AudioClip auClip = auEngine.getSoundForMenu("new_game_intro");
            TennisSoundObject tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", auClip);
            state.environment.Add(tso);
            state.blockingSound = tso;
            state.speed = 0;
			state.timestamp = 0;
			state.level = 1;
            return false;
        }));

		rules.Add(new TennisRule("action", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
			if (eve.payload.Equals("any")) {
				if (state.timestamp == 1) {
					state.speed = latestSpeed == 0 ? Settings.starting_speed : latestSpeed;
					state.timestamp = 2;
					return false;
				}
				return false;
			}
			return true;
		}));

		rules.Add(new TennisRule("action", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
			if (eve.payload.Equals("pause") && state.blockingSound != null) {
				state.blockingSound.hidden = true;
				return true;
			}
			return true;
		}));

        rules.Add(new TennisRule("action", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            if (eve.payload.Equals("escape")) {
                Application.LoadLevel("mainMenu");
                return false;
            }
            return true;
        }));

        rules.Add(new TennisRule("soundOver", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            int id = int.Parse(eve.payload);
            if (state.blockingSound != null && id == state.blockingSound.clip.GetInstanceID()) {
                state.environment.Remove(state.blockingSound);
                state.blockingSound = null;
				if (state.timestamp == 0) {
					state.timestamp = 1;
					return false;
				}
				if (state.timestamp == 12) {
                    state.blockingSound = new TennisSoundObject("Prefabs/Tennis/GameAudio", auEngine.getSoundForMenu("new_game_prize_" + (state.level - 1)));
					engine.state.environment.Add(state.blockingSound);
					state.timestamp = 2;
					return false;
				}
				if (state.timestamp == 14) {
                    state.blockingSound = new TennisSoundObject("Prefabs/Tennis/GameAudio", auEngine.getSoundForMenu("new_game_extra_life"));
					engine.state.environment.Add(state.blockingSound);
					state.timestamp = 15;
					return false;
				}
				if (state.timestamp == 15) {
                    state.blockingSound = new TennisSoundObject("Prefabs/Tennis/GameAudio", auEngine.getSoundForMenu("new_game_lives_left_" + state.lives));
					engine.state.environment.Add(state.blockingSound);
					state.timestamp = 2;
					return false;
                }
                if (state.timestamp == 16) {
                    state.blockingSound = new TennisSoundObject("Prefabs/Tennis/GameAudio", auEngine.getSoundForMenu("new_game_outro_a"));
                    engine.state.environment.Add(state.blockingSound);
                    state.timestamp = 17;
                    return false;
                }
				if (state.timestamp == 17) {
                    state.blockingSound = new TennisSoundObject("Prefabs/Tennis/GameAudio", auEngine.getSoundForMenu("new_game_outro_b" + (state.level - 1)));
					engine.state.environment.Add(state.blockingSound);
					state.timestamp = 18;
					return false;
				}
				if (state.timestamp == 18) {
                    state.blockingSound = new TennisSoundObject("Prefabs/Tennis/GameAudio", auEngine.getSoundForMenu("new_game_outro_c"));
					engine.state.environment.Add(state.blockingSound);
					state.timestamp = 19;
					return false;
				}
				if (state.timestamp == 19) {
					state.timestamp = 10;
					return false;
				}
                if (state.level > Settings.levels_max) {
					environment.Remove(ball);
                    state.blockingSound = new TennisSoundObject("Prefabs/Tennis/GameAudio", auEngine.getSoundForMenu("new_game_outro_a"));
					engine.state.environment.Add(state.blockingSound);
					state.speed = 0f;
					state.timestamp = 17;
					return false;
				}
				if (state.extra_life) {
					state.extra_life = false;
					state.lives++;
                    AudioClip auClip = auEngine.getSoundForMenu("new_game_stage_" + state.level / Settings.levels_per_life);
					state.blockingSound = new TennisSoundObject("Prefabs/Tennis/GameAudio", auClip);
					engine.state.environment.Add(state.blockingSound);
					state.timestamp = 14;
					return false;
				}
				if (state.speed == 0f) {
					state.speed = latestSpeed == 0 ? Settings.starting_speed : latestSpeed;
				}
            } else {
                WorldObject toRemove = null;
                foreach (WorldObject go in state.environment) {
                    if (go is TennisSoundObject && (go as TennisSoundObject).clip.GetInstanceID() == id) {
                        toRemove = go;
                        break;
                    }
                }
                if (toRemove != null) {
                    state.environment.Remove(toRemove);
                }
            }
            return false;
        }));

        rules.Add(new TennisRule("ALL", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            return !eve.initiator.StartsWith("player") || (eve.initiator.Equals("player" + state.curPlayer) && state.blockingSound == null);
        }));

        rules.Add(new TennisRule("ALL", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            gui.text = "Score: " + state.score + "\nLevel: " + state.level + "\nLives: " + state.lives;
            return true;
        }));

        rules.Add(new TennisRule("action", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            if (state.timestamp == 10) {
                (state.result as TennisGameResult).status = TennisGameResult.GameStatus.Over;
                return false;
            }
            return true;
        }));

        rules.Add(new TennisRule("action", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            if (eve.payload.Equals("pause")) {
                state.speed += latestSpeed;
                latestSpeed = state.speed - latestSpeed;
                state.speed -= latestSpeed;
            }
            return false;
        }));

        rules.Add(new TennisRule("settings", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            switch (eve.payload) {
                case "movement":
                    positionalMovement = !positionalMovement;
                    break;
                case "difficulty":
                    handleDifficulty = !handleDifficulty;
                    break;
                default:
                    return false;
            }
            return true;
        }));

        rules.Add(new TennisRule("move", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            if (state.speed == 0f || positionalMovement) {
                return false;
            }
            int dx = 0;
            switch (eve.payload) {
                case "left":
                    dx = -1;
                    break;
                case "right":
                    dx = 1;
                    break;
                default:
                    return false;
            }
            foreach (Actor actor in state.actors) {
                int x = (int)(actor.position.x / offset_x) + dx;
                if (x < -1 || x > 1) {
                    return false;
                }
                actor.position = new Vector3(offset_x * x, actor.position.y, actor.position.z);
                engine.state.environment.Add(new TennisSoundObject("Prefabs/Tennis/GameAudio", auEngine.getSoundForPlayer("just moved", new Vector3(x, 0, 0)), actor.position));
            }
            return false;
        }));

        rules.Add(new TennisRule("position", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            if (state.speed == 0f || !positionalMovement) {
                return false;
            }
            int dx = 0;
            switch (eve.payload) {
                case "left":
                    dx = -1;
                    break;
                case "right":
                    dx = 1;
                    break;
            }
            foreach (Actor actor in state.actors) {
                Vector3 newPos = new Vector3(offset_x * dx, actor.position.y, actor.position.z);
                if (actor.position == newPos) {
                    return false;
                }
                actor.position = newPos;
                engine.state.environment.Add(new TennisSoundObject("Prefabs/Tennis/GameAudio", auEngine.getSoundForPlayer("just moved", new Vector3(dx, 0, 0)), actor.position));
            }
            return false;
        }));

        rules.Add(new TennisRule("hit", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            switch (eve.payload) {
                case "-1":
                    ballTarget = -1;
                    break;
                case "0":
                    ballTarget = 0;
                    break;
                case "1":
                    ballTarget = 1;
                    break;
            }
            return false;
        }));

        rules.Add(new TennisRule("bounce", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            AudioClip auClip;
            Vector3 position = new Vector3(ballTarget * offset_x, ball.position.y, ball.position.z);
            switch (eve.payload) {
            case "racket":
                state.score++;
                auClip = auEngine.getSoundForPlayer("player_racket_hit");
                break;
            case "wall":
                if (state.score == Settings.score_per_level) {
                    state.score = 0;
                    if (state.level % Settings.levels_per_life == 0) {
                        state.extra_life = true;
                    }
                    state.timestamp = 12;
                    engine.state.environment.Add(new TennisSoundObject("Prefabs/Tennis/GameAudio", auEngine.getSoundForPlayer("claps_" + (state.level % 2 + 1)), position));
                    auClip = auEngine.getSoundForPlayer("win_" + Random.Range(1, 13));
                    environment.Remove(ball);
                    ball = new TennisMovingObject("Prefabs/Tennis/Ball", new Vector3(0, -2, Settings.size_mod_z * 5.9f), false);
                    environment.Add(ball);
                    state.level++;
                    latestSpeed = Settings.starting_speed + state.level * Settings.speed_per_level;
                    state.speed = 0;
                    state.blockingSound = new TennisSoundObject("Prefabs/Tennis/GameAudio", auClip, position);
                    engine.state.environment.Add(state.blockingSound);
                    return true;
                }
                auClip = auEngine.getSoundForPlayer("opponent_racket_hit");
                break;
            case "floor":
                auClip = auEngine.getSoundForPlayer("floor_hit");
                break;
            case "net":
                auClip = auEngine.getSoundForPlayer("net_pass");
                position.Scale(new Vector3(1f, 1f, 1f));
                break;
            case "boundary":
                state.lives--;
                if (state.lives == 0) {
					state.speed = 0;
					if (state.level == 1) {
						state.timestamp = 18;
                        auClip = auEngine.getSoundForMenu("new_game_outro_start");
						state.blockingSound = new TennisSoundObject("Prefabs/Tennis/GameAudio", auClip, position);
						engine.state.environment.Add(state.blockingSound);
						return false;
					}
                    state.timestamp = 16;
                    auClip = auEngine.getSoundForMenu("new_game_outro_start");
					state.blockingSound = new TennisSoundObject("Prefabs/Tennis/GameAudio", auClip, position);
					engine.state.environment.Add(state.blockingSound);
					return false;
				}
                auClip = auEngine.getSoundForMenu("new_game_miss");
                environment.Remove(ball);
                ball = new TennisMovingObject("Prefabs/Tennis/Ball", new Vector3(0, -2, Settings.size_mod_z * 5.9f), false);
                environment.Add(ball);
                latestSpeed = state.speed;
                state.speed = 0;
                state.blockingSound = new TennisSoundObject("Prefabs/Tennis/GameAudio", auClip, position);
                engine.state.environment.Add(state.blockingSound);
				state.timestamp = 15;
                return true;
            default:
                return false;
            }
            TennisSoundObject tso = new TennisSoundObject("Prefabs/Tennis/GameAudio", auClip, position);
            engine.state.environment.Add(tso);
            return true;
        }));

        rules.Add(new TennisRule("difficulty", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            if (!handleDifficulty) {
                return false;
            }
            float df;
            switch (eve.payload) {
                case "up":
                    df = 0.01f;
                    break;
                case "down":
                    df = -0.01f;
                    break;
                default:
                    return false;
            }
            state.speed += df;
            if (state.speed >= 4f || state.speed <= 0.5) {
                state.speed -= df;
            }
            return false;
        }));

        gameObject.AddComponent<TennisGameEngine>();
        gameObject.AddComponent<TennisUserInterface>();
        gameObject.GetComponent<TennisGameEngine>().initialize(rules, actors, environment, players, renderer);
        gameObject.GetComponent<TennisUserInterface>().initialize(gameObject.GetComponent<TennisGameEngine>());
        gameObject.GetComponent<TennisGameEngine>().postEvent(new GameEvent("", "initialization", "unity"));
	}
}