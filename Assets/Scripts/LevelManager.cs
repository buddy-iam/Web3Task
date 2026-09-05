using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public void Update() {
        if (Input.GetKeyDown(KeyCode.R)){
            RestartGame();
        }
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);    
    }
}