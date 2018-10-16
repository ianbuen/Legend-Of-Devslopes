using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PowerUp : MonoBehaviour {

    private GameObject player;

    [SerializeField] private float spinSpeed = 75f;
    [SerializeField] private int restoreHealthAmount = 30;
    [SerializeField] private float speedUpAmount = 0.25f;
    [SerializeField] private float timeImmunity = 2f;

    private AudioSource audioSource;

    // Use this for initialization
    void Start () {
        player = GameManager.Instance.Player;
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, Time.deltaTime * spinSpeed);
	}

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Player") {
            player.GetComponent<AudioSource>().PlayOneShot(audioSource.clip, 1f);

            switch (gameObject.tag) {
                case "HealthUp":
                    player.GetComponent<PlayerHealth>().RestoreHealth(restoreHealthAmount);
                    break;
                case "SpeedUp":
                    player.GetComponent<PlayerController>().SetSpeed(speedUpAmount);
                    StartCoroutine(player.GetComponent<PlayerHealth>().AddTimeImmunity(timeImmunity));
                    break;
            }

            Destroy(gameObject);
        }
    }
}
