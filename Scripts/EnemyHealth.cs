using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour {

    [SerializeField] private int initialHealth = 100;
    [SerializeField] private float timeSinceLastHit = 0.5f;
    [SerializeField] private float disappearSpeed = 1f;

    private int currentHealth;
    private ParticleSystem blood;

    private AudioSource audio;
    private Animator animator;

    private NavMeshAgent agent;
    private Rigidbody rigidbody;
    private CapsuleCollider collider;
    private bool disappearEnemy = false;

    private float timer;

    public bool IsAlive { get; private set; }

    // Use this for initialization
    void Start() {
        GameManager.Instance.RegisterEnemy(this);

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        blood = GetComponentInChildren<ParticleSystem>();

        currentHealth = initialHealth;
        IsAlive = true;
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;

        if (disappearEnemy)
            transform.Translate(Vector3.down * disappearSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {

        if (timer >= timeSinceLastHit && !GameManager.Instance.GameOver) {
            if (other.tag == "PlayerWeapon") {
                blood.Play();
                TakeHit();
                timer = 0;
            }
        }
    }

    private void TakeHit() {

        if (currentHealth > 0) {
            animator.Play("Hurt");
            audio.PlayOneShot(audio.clip);
            currentHealth -= 10;
        }

        if (currentHealth <= 0) {
            IsAlive = false;
            KillEnemy();
        }
    }

    private void KillEnemy() {
        collider.enabled = false;
        agent.enabled = false;
        animator.SetTrigger("EnemyDead");
        rigidbody.isKinematic = true;

        GameManager.Instance.RegisterKill();
        StartCoroutine(RemoveEnemy());
    }

    private IEnumerator RemoveEnemy() {

        yield return new WaitForSeconds(4f);
        disappearEnemy = true;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
