using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    // In Order, Hero-Ranger-Soldier-Tanker
    [SerializeField] private GameObject[] characters;

	// Use this for initialization
	void Start () {
        StartCoroutine(Showcase());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Showcase() {
        foreach(var one in characters) {
            if (one.tag == "Player")
                one.GetComponent<Animator>().Play("Spin Attack");
            else
                one.GetComponent<Animator>().Play("Attack");

            yield return new WaitForSeconds(1.5f);
        }

        yield return null;
        StartCoroutine(Showcase());
    }

    public void Battle() {
        SceneManager.LoadScene("Island");
    }

    public void Quit() {
        Application.Quit();
    }
}
