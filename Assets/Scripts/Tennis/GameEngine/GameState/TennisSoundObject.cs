using UnityEngine;
using System.Collections;

public class TennisSoundObject : SoundObject, IUnityRenderable {

    public AudioClip clip;
    public string prefab;
    public TennisSoundObject(string prefab, AudioClip clip) : base(Vector3.zero, false) {
        this.clip = clip;
        this.prefab = prefab;
        this.position = position;
    }

    public TennisSoundObject(string prefab, AudioClip clip, Vector3 position) : base(position, false) {
        this.clip = clip;
        this.prefab = prefab;
        this.position = position;
	}

    public string getPrefab() {
        return prefab;
    }

}