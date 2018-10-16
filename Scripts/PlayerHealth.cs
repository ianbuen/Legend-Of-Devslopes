using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour {

    [SerializeField] private int initialHealth = 100;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float timeSinceLastHit = 2f; 

    private float timer;
    private CharacterController charController;
    private AudioSource audio;
    private Animator animator;
    private ParticleSystem blood;

    public int CurrentHealth { get; private set; }

    private void Awake() {
        Assert.IsNotNull(healthSlider);
    }

    // Use this for initialization
    void Start () {

        CurrentHealth = initialHealth;
        charController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        blood = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        healthSlider.value = CurrentHealth;
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
        
        if (CurrentHealth > 0) {
            GameManager.Instance.PlayerHit(CurrentHealth);
            animator.Play("Hurt");
            audio.PlayOneShot(audio.clip);
            CurrentHealth -= 10;
        } 
        
        if (CurrentHealth <= 0) {
            CurrentHealth = 0;
            KillPlayer();
        }
    }

    private void KillPlayer() {
        GameManager.Instance.PlayerHit(CurrentHealth);
        animator.SetTrigger("PlayerDeath");
        charController.enabled = false;
    }

    public void RestoreHealth(int amount) {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, 100);
    }

    public IEnumerator AddTimeImmunity(float time) {
        timeSinceLastHit += time;
        yield return new WaitForSeconds(10f);
        timeSinceLastHit -= time;
    }
}
