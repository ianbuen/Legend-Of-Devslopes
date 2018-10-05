using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] private int initialHealth = 100;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float timeSinceLastHit = 2f; 

    private float timer;
    private int currentHealth;
    private CharacterController charController;
    private AudioSource audio;
    private Animator animator;
    private ParticleSystem blood;

    private void Awake() {
        Assert.IsNotNull(healthSlider);
    }

    // Use this for initialization
    void Start () {

        currentHealth = initialHealth;
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        blood = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;
	}

    private void OnTriggerEnter(Collider other) {
        
        if (timer >= timeSinceLastHit && !GameManager.Instance.GameOver) {
            if (other.tag == "Weapon") {
                blood.Play();
                TakeHit();
                timer = 0;
            }
        }
    }

    private void TakeHit() {
        
        if (currentHealth > 0) {
            GameManager.Instance.PlayerHit(currentHealth);
            animator.Play("Hurt");
            audio.PlayOneShot(audio.clip);
            currentHealth -= 10;
            healthSlider.value = currentHealth;
        } 
        
        if (currentHealth <= 0) {
            healthSlider.value = 0;
            KillPlayer();
        }
    }

    private void KillPlayer() {
        GameManager.Instance.PlayerHit(currentHealth);
        animator.SetTrigger("PlayerDeath");
        charController.enabled = false;
    }
}
