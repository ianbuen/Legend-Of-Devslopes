using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {

    [SerializeField] protected float attackRange = 3f;
    [SerializeField] protected float timeBetweenAttacks = 1f;

    protected Animator animator;
    protected GameObject player;

    public bool PlayerInRange { get; private set; }
    public bool Attacking { get; private set; }

    private BoxCollider[] weaponColliders;

    protected EnemyHealth enemyHealth;

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

        if (enemyHealth.IsAlive && Vector3.Distance(transform.position, player.transform.position) < attackRange) {
            // Rotate towards target
            Vector3 lookPosition = player.transform.position - transform.position;
            lookPosition.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPosition.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
            // Finish rotating

            PlayerInRange = true;
        }

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
