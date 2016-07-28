﻿
/**
 * Copyright 2016 , SciFY NPO - http://www.scify.org
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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