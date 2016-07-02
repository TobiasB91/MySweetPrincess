using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float speed = 1;
    public float rotSpeed = 1;
    Quaternion startRot;

    void Start() {
        startRot = transform.rotation;
    }

	// Update is called once per frame
	void Update () {
        // Forward and backword Movement in world space (world is not rotated correctly -_-)
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
        // Left and right movement in local space
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * speed * Time.deltaTime);
        // Change to topview and revert
        if (Input.GetKey(KeyCode.Q)) transform.rotation = Quaternion.Euler(new Vector3(90, 90, 0));
        if (Input.GetKey(KeyCode.E)) ResetCamera();
    }

    public void ResetCamera() {
        transform.rotation = startRot;
    }
}
