using UnityEngine;

public class CameraFacing : MonoBehaviour {
	[SerializeField] private Camera cam;
	
	void Awake() {
		cam = Camera.main; 
	}
	void FixedUpdate() {
		Vector3 v = cam.transform.position - transform.position;
		v.x = v.z = 0.0f;
		transform.LookAt(cam.transform.position - v); 
	}
}