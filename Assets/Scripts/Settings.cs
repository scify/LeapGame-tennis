
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
using System.Collections.Generic;
using UnityEngine;

public class Settings {

    public static bool just_started = true;
    public static bool first_game = true;
	public static bool positionalMovement = true;
    public static int levels_per_life = 4;
    public static int levels_max = 20;
    public static int score_per_level = 10;
    public static float starting_speed = 0.6f;
    public static float speed_per_level = 0.12f;
    public static float size_mod_z = 4f;
    public static string game_sounds = "default";
    public static string menu_sounds = "default";
    public static string previousMenu = "mainMenu";
    public static string default_soundset = "Θοδωρής";
    public static AudioSettingsDocument audio_settings_document;
    public static Dictionary<string, AudioClip> audioClips = new Dictionary<string,AudioClip>();
}