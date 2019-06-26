using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour {

    private static int currScene;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Submit")) {
            if (SceneManager.GetActiveScene().name == "TelaVoceVenceu") {
                nextLevel();
            } else if (SceneManager.GetActiveScene().name == "TelaVoceMorreu") {
                tryAgain();
            } else if (SceneManager.GetActiveScene().name == "FaseCastelo") {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void NewGame() {
        SceneManager.LoadScene("FaseFloresta");
    }

    public static void win() {
        currScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("TelaVoceVenceu");
    }

    public static void die() {
        currScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("TelaVoceMorreu");
    }

    private static void tryAgain() {
        SceneManager.LoadScene(currScene);
    }

    private static void nextLevel() {
        SceneManager.LoadScene(currScene + 1);
    }
}
