using System;

public class TennisMenuResult : GameResult {

    public enum GameStatus {
        Paused = -1,
        Choosing = 0,
        NewGame = 1,
        Tutorial = 2,
        Exit = 3
    }

    public GameStatus status;

    public TennisMenuResult(GameStatus status) {
        this.status = status;
	}

    public override bool gameOver() {
        return status > GameStatus.Choosing;
    }

    public override int getWinner() {
        return -1;
    }
}