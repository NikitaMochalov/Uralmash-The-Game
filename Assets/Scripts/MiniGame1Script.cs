using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGame1Script : MonoBehaviour {

    public void BackToMainGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
