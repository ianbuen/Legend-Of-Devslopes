using UnityEngine.Assertions;
using UnityEngine;

public class RangerAttack : EnemyAttack {

    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform bow;

    private void Awake() {
        Assert.IsNotNull(arrow);
        Assert.IsNotNull(bow);
    }

    public void FireArrow() {
        GameObject arrow = Instantiate(this.arrow) as GameObject;
        arrow.transform.position = bow.position;
        arrow.transform.rotation = transform.rotation;
        arrow.GetComponent<Rigidbody>().velocity = transform.forward * 25f;       
    }
}
