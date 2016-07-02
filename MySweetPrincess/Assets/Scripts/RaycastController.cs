using UnityEngine;
using System.Collections;

public class RaycastController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 0.8f;
        Debug.DrawRay(transform.position + Vector3.up, forward, Color.red);
    }

	public bool ColliderInfront(out RaycastHit hit) {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 0.7f;
        Debug.DrawRay(transform.position, forward, Color.green);

        return Physics.Raycast(transform.position, forward, out hit, 0.7f);
	}

    public bool ColliderInfrontAbove(out RaycastHit hit) {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 0.8f;
        Debug.DrawRay(transform.position + Vector3.up, forward, Color.red);

        return Physics.Raycast(transform.position + Vector3.up, forward, out hit, 0.8f);
    }

    public bool ColliderInfrontBeneath(out RaycastHit hit) {
        Vector3 forward = transform.TransformDirection(Vector3.forward + Vector3.down * 2);
        Debug.DrawRay(transform.position + Vector3.up * 1.5f, forward, Color.blue);

        return Physics.Raycast(transform.position + Vector3.up * 1.5f, forward, out hit, 3f);
    }

    public bool ColliderInfrontBeneathBeneath(out RaycastHit hit) {
        Vector3 forward = transform.TransformDirection(Vector3.forward + Vector3.down * 3.5f);
        Debug.DrawRay(transform.position + Vector3.up * 2f, forward, Color.yellow);

        return Physics.Raycast(transform.position + Vector3.up * 2f, forward, out hit, 4f);
    }
}
