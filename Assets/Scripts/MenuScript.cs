using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public void Play() {
        SceneManager.LoadScene("Main");
    }

    public void Leaderboard() {
        return;
    }

    public void Settings() {
        return;
    }
}