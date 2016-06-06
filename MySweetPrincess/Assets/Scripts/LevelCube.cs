using UnityEngine;
using System.Collections;

public class LevelCube : MonoBehaviour {

	public Texture2D floorTexture;

	// Use this for initialization
	protected virtual void Start () {
		gameObject.GetComponent<Renderer>().material.mainTexture = floorTexture;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
