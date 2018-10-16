using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class Enemy : MonoBehaviour {

    private Transform target;
    private NavMeshAgent navMeshAgent;
    private EnemyAttack enemyAttack;
    private EnemyHealth enemyHealth;
    private Animator animator;

    // Use this for initialization
    void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyHealth = GetComponent<EnemyHealth>();

        target = GameManager.Instance.Player.transform;
    }
	
	// Update is called once per frame
	void Update () {

        if (enemyHealth.IsAlive) {

            if(!GameManager.Instance.GameOver) {

                if (enemyAttack.PlayerInRange) {
                    navMeshAgent.enabled = false;
                    animator.SetBool("Running", false);
                } else {
                    navMeshAgent.enabled = true;
                    navMeshAgent.SetDestination(target.position);
                    animator.SetBool("Running", true);
                }

            } else {
                navMeshAgent.enabled = false;
                animator.SetBool("Running", false);
                animator.Play("Idle");
            }

        } else {
            navMeshAgent.enabled = false;
        }
	}
}
