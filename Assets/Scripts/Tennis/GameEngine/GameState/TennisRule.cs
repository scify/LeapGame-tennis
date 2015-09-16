using UnityEngine;
using System.Collections;
using System;

public class TennisRule : Rule {

    public delegate bool Applicator(TennisGameState state, GameEvent eve, TennisGameEngine engine);
    public delegate bool MenuApplicator(TennisMenuState state, GameEvent eve, TennisMenuEngine engine);
    public Applicator apllicator;
    public MenuApplicator menuApllicator;

    public TennisRule(string category, Applicator applier) : base(category) {
        apllicator = applier;
    }
    public TennisRule(string category, MenuApplicator applier)
        : base(category) {
            menuApllicator = applier;
    }

    public override bool applyTo(GameState state, GameEvent eve, GameEngine engine) {
        throw new ArgumentException("Invalid game state type! Expected a TennisGameState, got " + state.GetType().ToString());
    }

    public bool applyTo(TennisGameState state, GameEvent eve, TennisGameEngine engine) {
        return apllicator(state, eve, engine);
    }
    public bool applyTo(TennisMenuState state, GameEvent eve, TennisMenuEngine engine) {
        return menuApllicator(state, eve, engine);
    }
	
}