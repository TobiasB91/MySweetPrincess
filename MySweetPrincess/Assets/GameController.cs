using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    List<GameObject> sweets = new List<GameObject>();
    public GameObject player;
    Vector3 playerStartPos;
    Quaternion playerStartRot;
    int startWeight;

	// Use this for initialization
	void Start () {
        foreach (Transform child in transform) {
            sweets.Add(child.gameObject);
        }
        playerStartPos = player.transform.position;
        playerStartRot = player.transform.rotation;
        startWeight = player.GetComponent<CharacterController>().weight;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R)) Reset();
	}

    void Reset() {
        foreach (GameObject candy in sweets) {
            candy.SetActive(true);
        }
        // Camera Reset doesn't work
        player.GetComponent<CharacterController>().weight = startWeight;
        player.GetComponent<CharacterController>().isDead = false;
        player.transform.position = playerStartPos;
        player.transform.rotation = playerStartRot;
    }
}
