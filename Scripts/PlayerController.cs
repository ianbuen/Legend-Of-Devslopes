using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private Animator animator;
    private Animation animation;
    private CharacterController charController;
    private BoxCollider[] weaponColliders;

    private Vector3 moveDirection = Vector3.zero;
    private float origSpeed;

	// Use this for initialization
	void Start () {
        charController = GetComponent<CharacterController>();
        weaponColliders = GetComponentsInChildren<BoxCollider>();
        animator = GetComponent<Animator>();
        animation = GetComponent<Animation>();

        origSpeed = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {

        if (!GameManager.Instance.GameOver) {

            if (Input.GetMouseButtonDown(0)) {
                animator.Play("Double Chop");
            } else if (Input.GetMouseButtonDown(1)) {
                animator.Play("Spin Attack");
            }

            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            charController.SimpleMove(moveDirection * moveSpeed);

            if (moveDirection == Vector3.zero) {
                animator.SetBool("IsRunning", false);
            } else {
                animator.SetBool("IsRunning", true);

                /*
                if (Input.GetKey(KeyCode.LeftShift)) {
                    animator.SetBool("IsRunning", true);
                    moveSpeed = origSpeed * 1.25f; // run = +25% move speed
                } else {
                    animator.SetBool("IsRunning", false);
                    moveSpeed = origSpeed;
                }*/
            }
        }
        
    }

    // Must use when dealing with game physics
    void FixedUpdate() {

        if (!GameManager.Instance.GameOver) {

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (moveDirection != Vector3.zero) {
                Vector3 targetPosition = new Vector3(transform.position.x - moveDirection.x, transform.position.y, transform.position.z - moveDirection.z);
                Quaternion rotation = Quaternion.LookRotation(transform.position - targetPosition);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10f);
            }
        }
    }

    public void PlayerBeginAttack() {
        foreach (var weapon in weaponColliders) {
            weapon.enabled = true;
        }
    }

    public void PlayerEndAttack() {
        foreach (var weapon in weaponColliders) {
            weapon.enabled = false;
        }
    }
}
