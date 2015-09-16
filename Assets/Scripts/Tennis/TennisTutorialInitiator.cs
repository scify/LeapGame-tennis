using UnityEngine;
using System.Collections.Generic;

public class TennisTutorialInitiator : MonoBehaviour {
	
	public float offset_x = 0f;
	private TennisMovingObject ball;
	private bool positionalMovement = Settings.positionalMovement;
	private int ballTarget = 0;
	private bool handleDifficulty = false;
	private TextCanvasObject gui;
	
	void Start () {
		TennisStateRenderer renderer = new TennisStateRenderer();
		AudioEngine auEngine = new AudioEngine(0, "tennis", Settings.menu_sounds, Settings.game_sounds);
		
		List<Actor> actors = new List<Actor>();
		
		List<WorldObject> environment = new List<WorldObject>();
		ball = new TennisMovingObject("Prefabs/Tennis/Ball", new Vector3(0, -1, Settings.size_mod_z * 5.9f), true);
		environment.Add(ball);
		environment.Add(new TennisStaticObject("Prefabs/Tennis/MainCamera", new Vector3(0, 1, (Settings.size_mod_z * -6f) - 5.5f), false));
		environment.Add(new TennisStaticObject("Prefabs/Tennis/Light", new Vector3(0, 0, 0), false));
		environment.Add(new TennisStaticObject("Prefabs/Tennis/Field", new Vector3(0, -7, 0), false));
		environment.Add(new TennisStaticObject("Prefabs/Tennis/Wall", new Vector3(0, -5, Settings.size_mod_z * 6), false));
		environment.Add(new CanvasObject("Prefabs/Tennis/LeapLogo", true, new Vector3(0, 0, 0), false));
		
		List<Player> players = new List<Player>();
		players.Add(new Player("player0", "player0"));
		
		TennisRuleset rules = new TennisRuleset();
		rules.Add(new TennisRule("initialization", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
			AudioClip auClip = auEngine.getSoundForMenu("tutorial_intro");
			TennisSoundObject tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", auClip);
			state.environment.Add(tso);
			state.blockingSound = tso;
			state.speed = 0;
			state.timestamp = -1;
            state.target = 0;
			return false;
		}));
		
		rules.Add(new TennisRule("action", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
			if (eve.payload.Equals("any")) {
				switch(state.timestamp) {
				case 0:
					AudioClip auClip = auEngine.getSoundForMenu("tutorial_01");
					TennisSoundObject tso = new TennisSoundObject("Prefabs/Tennis/AudioSource", auClip);
					state.environment.Add(tso);
					state.blockingSound = tso;
					state.timestamp++;
					return false;
				case 10:
					(state.result as TennisGameResult).status = TennisGameResult.GameStatus.Over;
					return false;
				default:
					return false;
				}
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
				switch(state.timestamp) {
				case -1:
					state.timestamp++;
					break;
				case 44:
					break;
				case 55:
					break;
				case 66:
					break;
				case 77:
					break;
				case 78:
					break;
				case 88:
					break;
				case 89:
					break;
                case 76:
                    state.timestamp++;
                    break;
                case 6:
                    state.blockingSound = new TennisSoundObject("Prefabs/Tennis/AudioSource", auEngine.getSoundForMenu("tutorial_0" + state.timestamp));
                    state.environment.Add(state.blockingSound);
                    state.timestamp = 76;
                    break;
				case 7:
					TennisSoundObject stso = new TennisSoundObject("Prefabs/Tennis/AudioSource", auEngine.getSoundForMenu("tutorial_outro"));
					state.environment.Add(stso);
					state.blockingSound = stso;
					state.timestamp = 9;
					break;
				case 9:
					state.timestamp = 10;
					break;
				default:
					state.timestamp++;
					state.speed = 1f;
					break;
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

		rules.Add(new TennisRule("action", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
			if (eve.payload.Equals("pause") && state.blockingSound != null) {
				state.blockingSound.hidden = true;
				return false;
			}
			return true;
		}));

		rules.Add(new TennisRule("action", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
			if (eve.payload.Equals("enter") && state.timestamp > 10 && state.blockingSound == null && state.timestamp < 66) {
				state.timestamp /= 11;
				TennisSoundObject ltso = new TennisSoundObject("Prefabs/Tennis/AudioSource", auEngine.getSoundForMenu("tutorial_0" + state.timestamp));
				state.environment.Add(ltso);
				state.blockingSound = ltso;
				state.target = state.timestamp == 4 ? -1 : 0;
				state.timestamp = (state.timestamp + 1) * 11;
				return false;
			}
			return true;
		}));
		
		rules.Add(new TennisRule("ALL", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
			return !eve.initiator.StartsWith("player") || (eve.initiator.Equals("player" + state.curPlayer) && state.blockingSound == null);
		}));

        rules.Add(new TennisRule("position", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
            if (state.timestamp < 67) {
                return false;
            }
            if (state.speed == 0f || !positionalMovement) {
                return false;
            }
            switch (state.timestamp) {
                case 67:
                    if (!eve.payload.Equals("right")) {
                        state.timestamp = 66;
                        actors.Clear();
                        AudioClip auClip = auEngine.getSoundForMenu("tutorial_55");
                        state.blockingSound = new TennisSoundObject("Prefabs/Tennis/AudioSource", auClip);
                        state.environment.Add(state.blockingSound);
                        state.speed = 0;
                    }
                    break;
                case 78:
                    if (!eve.payload.Equals("left")) {
                        state.timestamp = 77;
                        actors.Clear();
                        AudioClip auClip = auEngine.getSoundForMenu("tutorial_66");
                        state.blockingSound = new TennisSoundObject("Prefabs/Tennis/AudioSource", auClip);
                        state.environment.Add(state.blockingSound);
                        state.speed = 0;
                    }
                    break;
                case 68:
                    break;
                case 79:
                    break;
                default:
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
		
		rules.Add(new TennisRule("move", (TennisGameState state, GameEvent eve, TennisGameEngine engine) => {
			switch (state.timestamp) {
			case 44:
				if (eve.payload.Equals("right")) {
                    state.speed = 1f;
				}
                break;
			case 55:
				if (eve.payload.Equals("left")) {
                    state.speed = 1f;
				}
                break;
			case 66:
				actors.Add(new TennisActor("racket", "Prefabs/Tennis/Racket", new Vector3(0, -5, Settings.size_mod_z * - 6), false, null));
				if (eve.payload.Equals("right")) {
					state.target = 1;
					state.speed = 1f;
                    state.timestamp = 67;
				}
                break;
            case 77:
                actors.Add(new TennisActor("racket", "Prefabs/Tennis/Racket", new Vector3(0, -5, Settings.size_mod_z * -6), false, null));
                if (eve.payload.Equals("left")) {
                    state.target = -1;
                    state.speed = 1f;
                    state.timestamp = 78;
                }
                break;
			default:
                break;
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
				auClip = auEngine.getSoundForPlayer("player_racket_hit");
                state.timestamp++;
                break;
			case "wall":
                if (state.timestamp == 68 || state.timestamp == 79) {
                    environment.Remove(ball);
                    ball = new TennisMovingObject("Prefabs/Tennis/Ball", new Vector3(0, -1, Settings.size_mod_z * 5.9f), true);
                    environment.Add(ball);
                    state.speed = 0f;
                    if (state.timestamp == 68) {
                        state.blockingSound = new TennisSoundObject("Prefabs/Tennis/AudioSource", auEngine.getSoundForPlayer("opponent_racket_hit"));
                    } else {
                        state.blockingSound = new TennisSoundObject("Prefabs/Tennis/AudioSource", auEngine.getSoundForPlayer("win_" + Random.Range(1, 13)));
                    }
                    environment.Add(state.blockingSound);
                    state.timestamp = (state.timestamp - 2) / 11;
                    return false;
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
				environment.Remove(ball);
				ball = new TennisMovingObject("Prefabs/Tennis/Ball", new Vector3(0, -1, Settings.size_mod_z * 5.9f), true);
				environment.Add(ball);
				state.speed = 0f;
				if (state.timestamp < 10) {
					TennisSoundObject rtso = new TennisSoundObject("Prefabs/Tennis/AudioSource", auEngine.getSoundForMenu("tutorial_0" + state.timestamp));
					state.environment.Add(rtso);
					state.blockingSound = rtso;
					switch(state.timestamp) {
                    case 2:
                        state.target = 0;
                        break;
					case 3:
						state.target = 1;
						state.timestamp = 44;
						break;
					case 44:
						break;
					case 55:
						break;
					case 66:
						break;
					default:
						state.target = -2;
						break;
					}
				}
				return false;
			default:
				return false;
			}
			TennisSoundObject tso = new TennisSoundObject("Prefabs/Tennis/GameAudio", auClip, position);
			engine.state.environment.Add(tso);
			return true;
		}));
		
		gameObject.AddComponent<TennisGameEngine>();
		gameObject.AddComponent<TennisUserInterface>();
		gameObject.GetComponent<TennisGameEngine>().initialize(rules, actors, environment, players, renderer);
		gameObject.GetComponent<TennisUserInterface>().initialize(gameObject.GetComponent<TennisGameEngine>());
		gameObject.GetComponent<TennisGameEngine>().postEvent(new GameEvent("", "initialization", "unity"));
	}
}