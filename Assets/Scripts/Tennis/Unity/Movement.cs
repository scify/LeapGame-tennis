using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class Movement : MonoBehaviour {

    public GameEngine engine;

    public abstract void OnCollisionEnter(Collision collision);
}