using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalSceneScript : MonoBehaviour
{

    public Text gameOverText;
    public ParticleSystem Confetti;
    public ParticleSystem Confetti1;

    bool flag = PlayerMovement.instance.winFlag;

    private void Start()
    {
        
        Time.timeScale = 1f;
        Debug.Log(flag);
        if (flag)
        {
            gameOverText.text = "YOU WIN";
            Confetti.Play();
            Confetti1.Play();
        }
        else gameOverText.text = "GAME OVER";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
