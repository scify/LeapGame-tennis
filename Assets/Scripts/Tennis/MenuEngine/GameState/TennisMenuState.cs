
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TennisMenuState : GameState {

    public List<TennisSoundObject> stoppableSounds = new List<TennisSoundObject>();

    public TennisMenuState(List<WorldObject> environment) {
        timestamp = 0;
        this.actors = new List<Actor>();
        this.environment = environment;
        this.players = new List<Player>();
        curPlayer = 0;
        result = new TennisMenuResult(TennisMenuResult.GameStatus.Choosing);
    }

    public TennisMenuState(TennisMenuState previousState) {
        timestamp = previousState.timestamp;
        actors = new List<Actor>();
        players = new List<Player>();
        environment = new List<WorldObject>(previousState.environment);
        curPlayer = previousState.curPlayer;
    }
}