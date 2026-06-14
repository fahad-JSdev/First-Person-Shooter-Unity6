using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
  
    public void PlayButtonStart()
    {
   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
