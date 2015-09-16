using UnityEngine;
using System.Collections.Generic;
using System;

public class TennisRuleset : Ruleset<TennisRule> {

    public override void applyTo(GameState state, GameEvent eve, GameEngine engine) {
        throw new ArgumentException("Invalid game state type! Expected a TennisGameState, got " + state.GetType().ToString(), "state");
    }

    public void applyTo(TennisMenuState state, GameEvent eve, TennisMenuEngine engine) {
        List<TennisRule> rules = this.FindAll(delegate(TennisRule rule) {
            return rule.category.Equals(eve.type) || rule.category.Equals("ALL");
        });
        foreach (TennisRule rule in rules) {
            if (!rule.applyTo(state, eve, engine)) {
                return;
            }
        };
    }
    public void applyTo(TennisGameState state, GameEvent eve, TennisGameEngine engine) {
        List<TennisRule> rules = this.FindAll(delegate(TennisRule rule) {
            return rule.category.Equals(eve.type) || rule.category.Equals("ALL");
        });
        foreach (TennisRule rule in rules) {
            if (!rule.applyTo(state, eve, engine)) {
                return;
            }
        };
    }

}