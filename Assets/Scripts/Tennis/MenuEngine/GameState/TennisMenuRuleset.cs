using UnityEngine;
using System.Collections.Generic;
using System;

public class TennisMenuRuleset : Ruleset<TennisMenuRule> {

    public override void applyTo(GameState state, GameEvent eve, GameEngine engine) {
        throw new ArgumentException("Invalid game state type! Expected a TennisGameState, got " + state.GetType().ToString(), "state");
    }

    public void applyTo(TennisMenuState state, GameEvent eve, TennisMenuEngine engine) {
        List<TennisMenuRule> rules = this.FindAll(delegate(TennisMenuRule rule) {
            return rule.category.Equals(eve.type) || rule.category.Equals("ALL");
        });
        foreach (TennisMenuRule rule in rules) {
            if (!rule.applyTo(state, eve, engine)) {
                return;
            }
        };
    }

}