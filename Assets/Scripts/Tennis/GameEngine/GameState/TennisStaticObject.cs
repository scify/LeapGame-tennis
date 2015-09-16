using UnityEngine;
using System.Collections;

public class TennisStaticObject : StaticObject, IUnityRenderable {

    public string prefab;

    public TennisStaticObject(string prefab, Vector3 position, bool hidden) : base(position, hidden) {
        this.prefab = prefab;
	}

    public string getPrefab() {
        return prefab;
    }

}