using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    List<GameObject> sweets = new List<GameObject>();
    public GameObject player;

    Vector3 playerStartPos;
    Quaternion playerStartRot;
    int startWeight;

    public Camera camera;
    Vector3 cameraStartPos;
    Quaternion cameraStartRot;
    CharController CharController;

    // Initialization 
    void Start () {
        foreach (Transform child in transform) {
            sweets.Add(child.gameObject);
        }
        playerStartPos = player.transform.position;
        playerStartRot = player.transform.rotation;
        startWeight = player.GetComponent<CharController>().weight;
        cameraStartPos = camera.transform.position;
        cameraStartRot = camera.transform.rotation;
        CharController = player.GetComponent<CharController>();
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
		// continuously check if "R" for reset is pressed and if the level is completed
        if (Input.GetKeyDown(KeyCode.R)) Reset();
        LevelComplete();
	}

	//Reset everything to starting values.
    void Reset() {
        foreach (GameObject candy in sweets) {
            candy.SetActive(true);
        }

        CharController.weight = startWeight;
        CharController.isDead = false;
        CharController.enteredDeepWater = false;
        CharController.steps = 0;
        CharController.gameOver.text = "";
        player.transform.position = playerStartPos;
        player.transform.rotation = playerStartRot;
        camera.transform.position = cameraStartPos;
        camera.transform.rotation = cameraStartRot;
    }
	/*
	 * Check if any candy is still active. 
	 * If not, set character to dead so it cannot be moved anymore and change the gameover text.
	 */
    void LevelComplete() {
        foreach (GameObject candy in sweets) {
            if (candy.activeSelf) return;
        }
        CharController.isDead = true;
        CharController.gameOver.text = "Level Complete!";
    }
}
