using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float timeBetweenAttacks = 1f;

    private Animator animator;
    private GameObject player;

    public bool PlayerInRange { get; private set; }
    public bool Attacking { get; private set; }

    private BoxCollider[] weaponColliders;

    private EnemyHealth enemyHealth;

	// Use this for initialization
	void Start () {
        weaponColliders = GetComponentsInChildren<BoxCollider>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = GameManager.Instance.Player;
        animator = GetComponent<Animator>();
        StartCoroutine(Attack());
	}
	
	// Update is called once per frame
	void Update () {

        PlayerInRange = false;

        if (enemyHealth.IsAlive)
            // if player is in range
            if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
                PlayerInRange = true;
	}

    IEnumerator Attack() {

        if (PlayerInRange && !GameManager.Instance.GameOver) {
            Attacking = true;
            animator.Play("Attack");
            yield return new WaitForSeconds(timeBetweenAttacks);
        }

        Attacking = false;

        yield return null;
        StartCoroutine(Attack());
    }

    public void BeginAttack() {
        foreach (var weapon in weaponColliders) {
            weapon.enabled = true;
        }
    }

    public void EndAttack() {
        foreach (var weapon in weaponColliders) {
            weapon.enabled = false;
        }
    }
}
