using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharController : MonoBehaviour {

    public int weight = 0;
    public int calorieLoss = 5;
    public int calorieLossUp = 2;
    public int calorieLossDeepWater = 2;
    public int adiposeWeight = 120;
    public int sinkInWater = 80;
    public int floatWeight = 50;
    public int starvationWeight = 30;
    public Text weightText;
    public Text stepsText;
    public Text gameOver;
    public Camera camera;
    Vector3 offSet;
    GameObject pFat;
    GameObject pNormal;
    GameObject pThin;
    public int steps = 0;
    public bool isDead;
    public GameObject raycast;
    bool moveForward = false;
    public bool enteredDeepWater;
    AudioSource audio;

	// Initialization 
	void Start () {
	    pFat = transform.FindChild("Princess fat").gameObject;
        pNormal = transform.FindChild("Princess normal").gameObject;
        pThin = transform.FindChild("Princess thin").gameObject;
        offSet = camera.transform.position - transform.position;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (!isDead) {

            RaycastHit hit;
            RaycastHit hitAbove;
            RaycastHit hitBeneath;
            RaycastHit hitBeneathBeneath;
			RaycastHit[] allBeneath;

			/*
			 * Check if one of the arrow keys were pressed. 
			 * If so the character has to rotate into that derection,
			 * the camera has to be set to the position of the character
			 * and the character is supposed to move forward.
			 */

            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                camera.transform.position = transform.position + offSet;
                moveForward = true;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                camera.transform.position = transform.position + offSet;
                moveForward = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                camera.transform.position = transform.position + offSet;
                moveForward = true;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                camera.transform.position = transform.position + offSet;
                moveForward = true;
            }

            if (moveForward) {
                // Reset camera to play rotation
                camera.GetComponent<CameraController>().ResetCamera();
				// hit in front
                if (raycast.GetComponent<RaycastController>().ColliderInfront(out hit)) {
                    // Hit candy in front, move normal, no weightloss
                    if (hit.collider.gameObject.tag == "Candy") {
                        transform.Translate(Vector3.left);
                        steps++;
                    }
					// hit in front above
                    if (raycast.GetComponent<RaycastController>().ColliderInfrontAbove(out hitAbove)) {
                        // Hit candy above, move forward and up, no weightloss
                        if (hitAbove.collider.gameObject.tag == "Candy") {
                            transform.Translate(Vector3.left + Vector3.up);
                            steps++;
                        }
                    // No hit above
                    } else { 
						// if nothing is above and a "Level Cube" in front move forward and up
                        if (hit.collider.gameObject.name == "Level Cube") {
                            transform.Translate(Vector3.left + Vector3.up);
                            enteredDeepWater = false;
                            weight -= calorieLossUp;
                            steps++;
                        }
                    }
                // No hit in front, but still we cannot just move forward
                } else {
                    // Hit in front beneath 
                    if (raycast.GetComponent<RaycastController>().ColliderInfrontBeneath(out hitBeneath)) {
						// hit candy beneath, move down, no weightloss
						if (hitBeneath.collider.gameObject.tag == "Candy") {
							transform.Translate(Vector3.down);
							weight += calorieLoss; // we substract the weight after the forward movement so we add the same amount here.
						}
						// hit "Deep Water" beneath while being heavy enough, move down
						if (hitBeneath.collider.gameObject.name == "Deep Water" &&
                                !enteredDeepWater && weight >= sinkInWater) { 
							// also hit candy, so there is no weightloss 
							allBeneath = raycast.GetComponent<RaycastController>().ColliderInfrontBeneathAll();
							for(int i = 0; i < allBeneath.Length; i++){
								if (allBeneath[i].collider.gameObject.tag == "Candy") weight += calorieLossDeepWater; // see above
							}	
                            transform.Translate(Vector3.down);
                            enteredDeepWater = true;
                        }
					// no hit beneath
                    } else {
                        // Hit "Level Cube" beneath beneath, move down
                        if (raycast.GetComponent<RaycastController>().ColliderInfrontBeneathBeneath(out hitBeneathBeneath) && 
                                hitBeneathBeneath.collider.gameObject.name == "Level Cube") {
                            transform.Translate(Vector3.down);
                        } else {
							// reaching this point is no movement possible.
							moveForward = false;
                            return;
                        }
                    }
					// forward movement for every normal step or step down
                    transform.Translate(Vector3.left);
                    steps++;

					// decide the weight cost for the forward movement
                    if (enteredDeepWater == true) {
                        weight -= calorieLossDeepWater;
                    } else {
                        weight -= calorieLoss;
                    }
                }
				// Movement completed
                moveForward = false;
            }

			// Determine whether the character has to die 
            if (weight <= starvationWeight || weight >= adiposeWeight)
            {
                Die();
            }
        }

        ChangeWeight();
        UpdateText();
    }

	/*
	 * Depending on the actual weight the princess has to change between thin, normal and fat. 
	 * Therefor we change which gameObject of the princess is active. 
	 */
    void ChangeWeight() {
        if (weight <= floatWeight) {
            pThin.SetActive(true);
            pFat.SetActive(false);
            pNormal.SetActive(false);
        }
        if (weight > floatWeight && weight < sinkInWater) {
            pNormal.SetActive(true);
            pThin.SetActive(false);
            pFat.SetActive(false);
        }
        if (weight >= sinkInWater) {
            pFat.SetActive(true);
            pNormal.SetActive(false);
            pThin.SetActive(false);
        }
    }
	/*
	 * If the character becomes to fat or thin the game is lost. 
	 * For better visualization we rotate the character to the belly if it dies of starvation
	 * and on the back if it is too fat to continue moving.
	 */
    void Die() {
        isDead = true;

        // Starved or too thick?
		if (weight <= starvationWeight) { 
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 90));
        } else {
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, -90));
        }
        // Move to ground
        transform.position -= new Vector3(0, 0.3f, 0);
    }


	// All textfields with information about the character are updated here. 
    void UpdateText() {
        weightText.text = "Weight: " + weight;
        stepsText.text = "Steps: " + steps;

        if (weight >= adiposeWeight) gameOver.text = "You're too fat to walk!";
        if (weight <= starvationWeight) gameOver.text = "You starved!";

        if (pThin.activeSelf || pFat.activeSelf) {
            weightText.color = Color.red;
        } else {
            weightText.color = Color.white;
        }
    }

	/*
	 * If the character collides with candy 
	 * the "calories" of the candy are added to the weight of the character
	 * and the collected candy is deactivated so it is no longer part of the scene.
	 * The chewing sound for collecting candy is played.
	 */
    void OnTriggerEnter(Collider hit) {
        if (hit.gameObject.tag == "Candy") {
            weight += hit.gameObject.GetComponent<Sweets>().calories;
            hit.gameObject.SetActive(false);
            audio.Play();
        }
    }
}
