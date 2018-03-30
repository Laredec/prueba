using UnityEngine;
using System.Collections;

public class EfectoStun : MonoBehaviour {

	public float timer;
	public float minScale, maxScale;
	Vector3 maxScaleVector, refVel;
	float colorTimer;
	public MeshRenderer[] rends;
	float alpha;
	float refVelF;
	void Start () {
		transform.localScale = new Vector3 (minScale, minScale,transform.localScale.z);
		maxScaleVector = new Vector3 (maxScale, transform.localScale.y, maxScale);
		refVel = Vector3.zero;
		colorTimer = timer - 0.5f;
		alpha = 1;
	}
	
	// Update is called once per frame
	void Update () {
        /*transform.localScale = Vector3.SmoothDamp (transform.localScale, maxScaleVector, ref refVel, 0.5f);
		timer -= Time.deltaTime;
		colorTimer -= Time.deltaTime;
		if (colorTimer <= 0f) {
			alpha -= Time.deltaTime * 2f;
			for (int i = 0; i < rends.Length; i++) {
				rends [i].material.color = new Color (rends [i].material.color.r, rends [i].material.color.g, rends [i].material.color.b, alpha);
			}
		}*/
        if (timer <= 0f) {
			Destroy (gameObject);
		}
	}
}
