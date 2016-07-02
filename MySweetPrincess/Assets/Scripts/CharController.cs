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
    public Camera camera;
    Vector3 offSet;
    Vector3 startPos;
    GameObject pFat;
    GameObject pNormal;
    GameObject pThin;
    public bool isDead;
    public GameObject raycast;
    bool moveForward = false;
    public bool enteredDeepWater;

	// Use this for initialization
	void Start () {
	    pFat = transform.FindChild("Princess fat").gameObject;
        pNormal = transform.FindChild("Princess normal").gameObject;
        pThin = transform.FindChild("Princess thin").gameObject;
        startPos = transform.position;
        offSet = camera.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update() {
        if (!isDead) {

            RaycastHit hit;
            RaycastHit hitAbove;
            RaycastHit hitBeneath;
            RaycastHit hitBeneathBeneath;

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
                if (raycast.GetComponent<RaycastController>().ColliderInfront(out hit)) {
                    // Hit candy
                    if (hit.collider.gameObject.tag == "Candy") {
                        transform.Translate(Vector3.left);
                    }
                    if (raycast.GetComponent<RaycastController>().ColliderInfrontAbove(out hitAbove)) {
                        // Hit but candy above
                        if (hitAbove.collider.gameObject.tag == "Candy") {
                            transform.Translate(Vector3.left + Vector3.up);
                        }
                    // No hit above
                    } else { 
                        if (hit.collider.gameObject.name == "Level Cube") {
                            transform.Translate(Vector3.left + Vector3.up);
                            enteredDeepWater = false;
                            weight -= calorieLossUp;
                        }
                    }
                // No hit in front
                } else {
                    // Hit beneath
                    if (raycast.GetComponent<RaycastController>().ColliderInfrontBeneath(out hitBeneath)) {
                        if (hitBeneath.collider.gameObject.tag == "Candy" || (hitBeneath.collider.gameObject.name == "Deep Water" &&
                                !enteredDeepWater && weight >= sinkInWater)) { 
                            transform.Translate(Vector3.down);
                            enteredDeepWater = true;
                        }
                    } else {
                        // Hit beneath beneath
                        if (raycast.GetComponent<RaycastController>().ColliderInfrontBeneathBeneath(out hitBeneathBeneath) && 
                                hitBeneathBeneath.collider.gameObject.name == "Level Cube") {
                            transform.Translate(Vector3.down);
                        }
                        else {
                            return;
                        }
                    }
                    transform.Translate(Vector3.left);

                    if (enteredDeepWater == true) {
                        weight -= calorieLossDeepWater;
                    } else {
                        weight -= calorieLoss;
                    }
                }
                moveForward = false;
            }

            if (weight <= starvationWeight || weight >= adiposeWeight)
            {
                Die();
            }
        }

        ChangeWeight();
        UpdateText();
    }

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

    void Die() {
        isDead = true;

        // Starved or too thick?
        if (weight <= 0) { 
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 90));
        } else {
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, -90));
        }
        // Move to ground
        transform.position -= new Vector3(0, 0.3f, 0);
    }

    void UpdateText() {
        weightText.text = "Weight: " + weight;
    }

    void OnTriggerEnter(Collider hit) {
        if (hit.gameObject.tag == "Candy") {
            weight += hit.gameObject.GetComponent<Sweets>().calories;
            hit.gameObject.SetActive(false); 
        }
    }
}
