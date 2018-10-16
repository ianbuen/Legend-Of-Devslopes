using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private Animator animator;
    private CharacterController charController;
    private BoxCollider[] weaponColliders;
    private GameObject wildFire;

    private float origSpeed;

    // Use this for initialization
    void Start() {
        charController = GetComponent<CharacterController>();
        weaponColliders = GetComponentsInChildren<BoxCollider>();
        wildFire = GameObject.FindGameObjectWithTag("Fire");
        animator = GetComponent<Animator>();

        wildFire.SetActive(false);
        origSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update() {

        if (!GameManager.Instance.GameOver) {

            if (Input.GetMouseButtonDown(0)) {
                animator.Play("Double Chop");
            } else if (Input.GetMouseButtonDown(1)) {
                animator.Play("Spin Attack");
            }

            Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            charController.SimpleMove(moveDirection.normalized * moveSpeed);

            if (moveDirection == Vector3.zero) {
                animator.SetBool("IsRunning", false);
            } else {
                animator.SetBool("IsRunning", true);
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

    public void SetSpeed(float speedAmount) {
        moveSpeed = origSpeed * (1 + speedAmount);
        StartCoroutine(GetLit());
    }

    IEnumerator GetLit() {
        wildFire.SetActive(true);
        animator.speed = 1.5f;
        yield return new WaitForSeconds(10f);
        animator.speed = 1;
        moveSpeed = origSpeed;
        var emission = wildFire.GetComponent<ParticleSystem>().emission;
        emission.enabled = false;
        yield return new WaitForSeconds(3f);
        emission.enabled = true;
        wildFire.SetActive(false);
    }
}
