﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class TennisLoadingInitiator : MonoBehaviour {

    private TextCanvasObject gui;
    private string basepath;
    private int players;
    private int total = 0;
    private int done = 0;
    private int bypass = 0;

    void Start() {
        TennisStateRenderer renderer = new TennisStateRenderer();

        List<WorldObject> environment = new List<WorldObject>();
        environment.Add(new TennisStaticObject("Prefabs/Tennis/Camera_Default", new Vector3(0, 10, 0), false));
        environment.Add(new TennisStaticObject("Prefabs/Tennis/Light_Default", new Vector3(0, 10, 0), false));
        environment.Add(new CanvasObject("Prefabs/Tennis/OutroLogo", true, new Vector3(0, 0, 0), false));
        gui = new TextCanvasObject("Prefabs/Tennis/GUI_Element", true, "Reading soundsets", new Vector3(0, 0, 0), false);
        environment.Add(gui);

        TennisRuleset rules = new TennisRuleset();
        rules.Add(new TennisRule("initialization", (TennisMenuState state, GameEvent eve, TennisMenuEngine engine) => {
            readSoundsets();
            return false;
        }));

        gameObject.AddComponent<TennisMenuEngine>();
        gameObject.AddComponent<TennisMenuUserInterface>();
        gameObject.GetComponent<TennisMenuEngine>().initialize(rules, environment, renderer);
        gameObject.GetComponent<TennisMenuUserInterface>().initialize(gameObject.GetComponent<TennisMenuEngine>());
        gameObject.GetComponent<TennisMenuEngine>().postEvent(new GameEvent("", "initialization", "unity"));
    }

    void Update() {
        if (bypass == 1 || (total != 0 && done == total)) {
            Application.LoadLevel(1);
        }
    }

    void readSoundsets() {
        try {
            Settings.audio_settings_document = new AudioSettingsDocument();
            Settings.audio_settings_document.LoadSettingsXML("Tennis");
        } catch (Exception e) {
            Debug.Log(e);
            Application.Quit();
        }
        players = Settings.audio_settings_document.MaximumPlayers;
        basepath = Application.dataPath + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar;
        basepath = basepath.Replace("_Data", "_Soundsets");
        DirectoryInfo dir = new DirectoryInfo(basepath);
        if (dir.Exists) {
            foreach (DirectoryInfo d in dir.GetDirectories()) {
                string name = null;
                if (d.GetFiles("info").Length != 0) {
                    FileInfo infoFile = d.GetFiles("info")[0];
                    string[] data = File.ReadAllLines(infoFile.FullName);
                    foreach (string s in data) {
                        if (s.Contains("name=")) {
                            name = s.Replace("name=", "");
                        }
                    }
                    Settings.audio_settings_document.addAudioSetting(name);
                    gui.text = "Loading " + name + "\n";
                    loadSoundset(basepath + d.Name + Path.DirectorySeparatorChar, name);
                }
            }
            if (total == 0) {
                bypass = 1;
            }
        } else {
            bypass = 1;
        }
    }

    void loadSoundset(string path, string name) {
        DirectoryInfo menu = new DirectoryInfo(path + "menu_sounds");
        Settings.audio_settings_document.addFiles(name, menu.GetFiles());
        for (int i = 1; i <= Settings.audio_settings_document.MaximumPlayers; i++) {
            DirectoryInfo player = new DirectoryInfo(path + "player" + i);
            Settings.audio_settings_document.addPlayerFiles(name, i, player.GetFiles());
        }
        foreach (FileInfo file in new DirectoryInfo(path).GetFiles("*.wav")) {
            Settings.audio_settings_document.addPlayerFile("default", name, file);
        }
        foreach (string key in Settings.audioClips.Keys) {
            if (Settings.audioClips[key] == null) {
                total++;
                StartCoroutine(loadClip(key));
            }
        }
    }

    IEnumerator<System.Object> loadClip(string path) {
        FileInfo f = new FileInfo(path);
        List<string> tokens = new List<string>();
        tokens.Add(f.Name);
        DirectoryInfo dir = f.Directory;
        while (dir != null) {
            tokens.Insert(0, dir.Name);
            dir = dir.Parent;
        }
        tokens[0] = tokens[0].Replace("\\", "").Replace("/", "");
        string url = "file:/";
        if (Application.platform.ToString().Contains("Windows")) {
            url += "/";
        }
        foreach (string s in tokens) {
            url += "/" + s;
        }
        WWW www = new WWW(url);
        yield return www;
        Settings.audioClips[path] = www.audioClip;
        done++;
    }

}