using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TennisGameState : GameState {

    public const int MAX_LEVEL = 20;
    public const int MAX_SCORE = 20;
    public const int LEVELS_PER_LIFE = 4;

    public TennisSoundObject blockingSound;
    public float speed;
    public int score;
    public int level;
    public int lives;
    public Boolean extra_life = false;
	public int target = -2;

    public TennisGameState(List<Actor> actors, List<WorldObject> environment, List<Player> players) {
        timestamp = 0;
        this.actors = actors;
        this.environment = environment;
        this.players = players;
        curPlayer = 0;
        result = new TennisGameResult(TennisGameResult.GameStatus.Ongoing, -1);
        blockingSound = null;
        speed = 0.6f;
        level = 0;
        lives = 3;
        score = 0;
    }

    public TennisGameState(TennisGameState previousState) {
        timestamp = previousState.timestamp;
        actors = new List<Actor>(previousState.actors);
        environment = new List<WorldObject>(previousState.environment);
        players = new List<Player>(previousState.players);
        curPlayer = previousState.curPlayer;
        blockingSound = previousState.blockingSound;
        speed = previousState.speed;
    }
}