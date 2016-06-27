using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterController : MonoBehaviour {

    public int weight = 0;
    public int calorieLoss = 5;
    public Text weightText;
    public Camera camera;
    Vector3 offSet;
    Vector3 startPos;
    GameObject pFat;
    GameObject pNormal;
    GameObject pThin;
    public bool isDead;

	// Use this for initialization
	void Start () {
	    pFat = transform.FindChild("Princess fat").gameObject;
        pNormal = transform.FindChild("Princess normal").gameObject;
        pThin = transform.FindChild("Princess thin").gameObject;
        startPos = transform.position;
        offSet = camera.transform.position; // Tiny Bug
	}

    // Update is called once per frame
    void Update() {
        if (!isDead) {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.position += new Vector3(1, 0, 0);
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                weight -= calorieLoss;
                camera.transform.position = transform.position + offSet;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                transform.position += new Vector3(-1, 0, 0);
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                weight -= calorieLoss;
                camera.transform.position = transform.position + offSet;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(0, 0, 1);
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                weight -= calorieLoss;
                camera.transform.position = transform.position + offSet;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += new Vector3(0, 0, -1);
                transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                weight -= calorieLoss;
                camera.transform.position = transform.position + offSet;
            }

            if (weight <= 0)
            {
                Die();
            }
        }

        ChangeWeight();
        UpdateText();
    }

    void ChangeWeight() {
        if (weight < 40) {
            pThin.SetActive(true);
            pFat.SetActive(false);
            pNormal.SetActive(false);
        }
        if (weight > 40 && weight < 100) {
            pNormal.SetActive(true);
            pThin.SetActive(false);
            pFat.SetActive(false);
        }
        if (weight > 100) {
            pFat.SetActive(true);
            pNormal.SetActive(false);
            pThin.SetActive(false);
        }
    }

    void Die() {
        isDead = true;
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 90));
        transform.position -= new Vector3(0, +0.25f, 0);
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
