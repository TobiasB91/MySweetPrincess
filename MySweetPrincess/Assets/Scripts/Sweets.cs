using UnityEngine;
using System.Collections;

public class Sweets : MonoBehaviour {

    public int calories;
    public float rotSpeed;

    void Update() {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x,
                                                          transform.rotation.eulerAngles.y + rotSpeed,
                                                          transform.rotation.eulerAngles.z));
    }
}
