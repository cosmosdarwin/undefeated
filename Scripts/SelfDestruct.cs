using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

    public float Wait = 5f;

	void Start () {
        Destroy(gameObject, Wait);
	}
	
}