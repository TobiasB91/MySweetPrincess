using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public int speed = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W)) transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) transform.position += new Vector3(-1, 0, 0) * speed  * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) transform.position += new Vector3(0, 0, 1) * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) transform.position += new Vector3(0, 0, -1) * speed * Time.deltaTime;
        
    }
}
