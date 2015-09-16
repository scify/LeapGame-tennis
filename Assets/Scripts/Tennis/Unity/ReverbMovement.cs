using UnityEngine;

public class ReverbMovement : MonoBehaviour {

	public Transform target;

	public void Start() {
		target = gameObject.transform.parent;
	}

    public void Update() {
		gameObject.transform.position = new Vector3(0f, -5, -target.position.z);
	}
}