using UnityEngine;

public class Arrow : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        Destroy(transform.parent.gameObject);
    }
}
