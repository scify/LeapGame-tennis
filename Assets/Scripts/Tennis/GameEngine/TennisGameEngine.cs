using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TennisGameEngine : GameEngine {

    public new TennisStateRenderer renderer;
    public TennisRuleset rules;
    public TennisGameState state;
    public List<Actor> actors;
    public List<WorldObject> environment;
    public List<Player> players;
    public Queue<GameEvent> events;

    private bool initialized = false;

    public void initialize(TennisRuleset rules, List<Actor> actors, List<WorldObject> environment, List<Player> players, TennisStateRenderer renderer) {
        this.rules = rules;
        this.actors = actors;
        this.environment = environment;
        this.players = players;
        this.renderer = renderer;
        state = new TennisGameState(actors, environment, players);
        events = new Queue<GameEvent>();
        state.curPlayer = new System.Random().Next(players.Count);
        initialized = true;
    }

	public void Start() {
        run();
	}

    public override void run() {
    }

    public void Update() {
        loop();
    }

    public override void loop() {
        if (!initialized) {
            return;
        }
        while (events.Count != 0) {
            if (state.result.gameOver()) {
                cleanUp();
                return;
            }
            GameEvent curEvent = events.Dequeue();
            rules.applyTo(state, curEvent, this);
        }
        renderer.render(this);
        if (state.result.gameOver()) {
            cleanUp();
        }
	}

    public override void postEvent(GameEvent eve) {
        events.Enqueue(eve);
    }

    public override GameState getState() {
        return state;
    }

    public override StateRenderer getRenderer() {
        return renderer;
    }

    public override void cleanUp() {
        Application.LoadLevel(Settings.previousMenu);
	}

}