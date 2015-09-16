using UnityEngine;
using System.Collections;
using System;

public class TennisMenuRule : Rule {

    public delegate bool Applicator(TennisMenuState state, GameEvent eve, TennisMenuEngine engine);
    public Applicator apllicator;

    public TennisMenuRule(string category, Applicator applier) : base(category) {
        apllicator = applier;
    }

    public override bool applyTo(GameState state, GameEvent eve, GameEngine engine) {
        throw new ArgumentException("Invalid game state type! Expected a TennisMenuGameState, got " + state.GetType().ToString());
    }

    public bool applyTo(TennisMenuState state, GameEvent eve, TennisMenuEngine engine) {
        return apllicator(state, eve, engine);
    }
	
}