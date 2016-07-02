using UnityEngine;
using System.Collections;

public class FanController : MonoBehaviour {
	public float rotationSpeed;
    AudioSource audio;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x,
                                                          transform.rotation.eulerAngles.y,
                                                          transform.rotation.eulerAngles.z + rotationSpeed));
	}

	void OnTriggerEnter(Collider hit) {
		if (hit.gameObject.tag == "Player" && hit.gameObject.GetComponent<CharController>().weight <= hit.gameObject.GetComponent<CharController>().floatWeight) {
			hit.gameObject.transform.position += new Vector3(0, 0, 3); // teleport player 3 blocks
            hit.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            audio.Play();
		}
	}
}
