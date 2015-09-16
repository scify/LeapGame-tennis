using UnityEngine;
using System.Collections;

public class TennisMovingObject : WorldObject, IUnityRenderable {

    public string prefab;

    public TennisMovingObject(string prefab, Vector3 position, bool hidden) : base(position, hidden) {
        this.prefab = prefab;
	}

    public string getPrefab() {
        return prefab;
    }

}