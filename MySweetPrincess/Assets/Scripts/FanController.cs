using UnityEngine;
using System.Collections;

public class FanController : MonoBehaviour {
	public float rotationSpeed;
    AudioSource audio;

	// Initialization of audiosource
	void Start () {
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		// rotation of the fan blades
		transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x,
                                                          transform.rotation.eulerAngles.y,
                                                          transform.rotation.eulerAngles.z + rotationSpeed));
	}
	/*
	 * If the character collides with the collider of the blades and is light enough
	 * the character will be moved and rotated to a new position and fitting sound will be played.
	 */
	void OnTriggerEnter(Collider hit) {
		if (hit.gameObject.tag == "Player" && hit.gameObject.GetComponent<CharController>().weight <= hit.gameObject.GetComponent<CharController>().floatWeight) {
			hit.gameObject.transform.position += new Vector3(0, 0, 3); // teleport player 3 blocks
            hit.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            audio.Play();
		}
	}
}
