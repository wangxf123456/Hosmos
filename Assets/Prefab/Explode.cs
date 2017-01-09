using UnityEngine;
using System.Collections;

public class Explode : MonoBehaviour {

	void OnDestroy() {
		explodes (this.transform.localScale.x * 4);
	}

	public void explodes(float mag) {
		Debug.Log ("Explode");
		Collider[] cols = Physics.OverlapSphere(this.transform.position, 4 * this.transform.localScale.x);
		foreach(Collider col in cols) {
			if (col.gameObject.GetComponent<Monster>()){
				Debug.Log ("Exert on "+ col.gameObject);
				Vector3 pointTo = this.transform.position - col.gameObject.transform.position;
				Vector3 vDiff = mag * pointTo / (pointTo.magnitude * pointTo.magnitude * col.rigidbody.mass);
				if (vDiff.magnitude > 6f)
					vDiff = Vector3.Normalize(vDiff) * 6f;
				col.rigidbody.velocity -= vDiff;
			}
		}
	}

}
