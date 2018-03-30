using UnityEngine;
using System.Collections;

public class EfectoEscudo : MonoBehaviour {

	public float velocidad;

	Material mat;
	void Start () {
		mat = GetComponentInChildren<MeshRenderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		float offset = Time.time * velocidad % 1;
		mat.mainTextureOffset = new Vector2 (0, -offset);
	}
}
