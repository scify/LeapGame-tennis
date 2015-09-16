using System;

public class TennisGameResult : GameResult {

    public enum GameStatus {
        Ongoing = 0,
        Won = 1,
        Draw = 2,
        Over = 3,
        Replay = 4
    }

    public GameStatus status;
    public int winner;

    public TennisGameResult(GameStatus status, int winner) {
        this.status = status;
        this.winner = winner;
	}

    public override bool gameOver() {
        return status == GameStatus.Over || status == GameStatus.Replay;
    }

    public override int getWinner() {
        return winner;
    }
}