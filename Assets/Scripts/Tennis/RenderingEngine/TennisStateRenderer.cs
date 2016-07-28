
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
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class TennisStateRenderer : StateRenderer {

	public TennisStateRenderer() {
	}

    public override void render(GameEngine engine) {
        throw new System.NotImplementedException("This engine can not be used for this game");
    }

    public void render(TennisMenuEngine engine) {
        TennisMenuState state = engine.state;
        List<WorldObject> currentRenderedObjects = new List<WorldObject>();
        render(state.environment, currentRenderedObjects, engine);
        List<WorldObject> toRemove = new List<WorldObject>();
        foreach (WorldObject so in rendered.Keys) {
            if (!currentRenderedObjects.Contains(so)) {
                UnityEngine.Object.Destroy(rendered[so]);
                toRemove.Add(so);
            }
        }
        foreach (WorldObject so in toRemove) {
            rendered.Remove(so);
            prefabs.Remove(so);
        }
    }

    public void render(TennisGameEngine engine) {
        Time.timeScale = engine.state.speed;
        TennisGameState state = engine.state;
        List<WorldObject> currentRenderedObjects = new List<WorldObject>();
        render(state.environment, currentRenderedObjects, engine);
        render(state.actors, currentRenderedObjects, engine);
        List<WorldObject> toRemove = new List<WorldObject>();
        foreach (WorldObject so in rendered.Keys) {
            if (!currentRenderedObjects.Contains(so)) {
                UnityEngine.Object.Destroy(rendered[so]);
                toRemove.Add(so);
            }
        }
        foreach (WorldObject so in toRemove) {
            rendered.Remove(so);
        }
    }

    private void render<T>(List<T> set, List<WorldObject> currentRenderedObjects, GameEngine engine) where T : WorldObject {
        foreach (T so in set) {
            if (so is IUnityRenderable) {
                if (!rendered.ContainsKey(so)) {
                    GameObject go = (GameObject) GameObject.Instantiate(Resources.Load((so as IUnityRenderable).getPrefab()));
                    go.transform.position = so.position;
                    rendered.Add(so, go);
                    prefabs.Add(so, (so as IUnityRenderable).getPrefab());
                    if (so is CanvasObject) {
                        go.GetComponent<Canvas>().worldCamera = Camera.main;
                    }
                    if (so is TennisMenuItem) {
                        go.GetComponentInChildren<TextMesh>().text = (so as TennisMenuItem).message;
                    }
                    if (so is TennisSoundObject) {
                        go.GetComponent<AudioSource>().clip = (so as TennisSoundObject).clip;
                        go.GetComponent<AudioSource>().Play();
                        go.GetComponent<SoundScript>().initialize(engine);
                    }
                    if (so is TennisMovingObject) {
                        go.GetComponent<Movement>().engine = engine;
                    }
                    foreach (TennisCollider tc in go.GetComponentsInChildren<TennisCollider>()) {
                        tc.engine = engine;
                    }
                } else {
					if (so.hidden && so is SoundObject) {
						rendered[so].GetComponent<AudioSource>().Stop();
					}
                    if (!prefabs[so].Equals((so as IUnityRenderable).getPrefab())) {
                        UnityEngine.Object.Destroy(rendered[so]);
                        rendered.Remove(so);
                        prefabs.Remove(so);
                        GameObject go = (GameObject) GameObject.Instantiate(Resources.Load((so as IUnityRenderable).getPrefab()));
                        rendered.Add(so, go);
                        prefabs.Add(so, (so as IUnityRenderable).getPrefab());
                    }
                    if (so is TennisMenuItem) {
                        rendered[so].GetComponentInChildren<TextMesh>().text = (so as TennisMenuItem).message;
                    }
                    if (so is TextCanvasObject) {
                        rendered[so].GetComponentInChildren<Text>().text = (so as TextCanvasObject).text;
                    }
                    if (so is TennisMovingObject) {
                        so.position = rendered[so].transform.position;
                    } else {
                        rendered[so].transform.position = so.position;
                    }
                }
                currentRenderedObjects.Add(so);
            }
        }
    }
}