using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private PlayableDirector playDirector;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject skipText;
    [SerializeField] private PlayerMovement playerMovement;
    public CanvasGroup gameOverPanelCanvasGroup;


    private void Start()
    {
        playDirector.stopped += OnCutsceneFinished;

        gameOverPanelCanvasGroup.alpha = 0;
        gameOverPanelCanvasGroup.interactable = false;
        gameOverPanelCanvasGroup.blocksRaycasts = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) &&
            playDirector.state == PlayState.Playing)
        {
            SkipCutscene();
        }
    }

    //void OnCutsceneFinished(PlayableDirector director)
    //{
    //    crosshair.SetActive(true);
    //    player.SetActive(true);
    //    skipText.SetActive(false);  
    //}
    void OnCutsceneFinished(PlayableDirector director)
    {
        skipText.SetActive(false);
        StartCoroutine(EnablePlayer());
    }
    IEnumerator EnablePlayer()
    {
        crosshair.SetActive(true);

        yield return new WaitUntil(() => !Input.GetKey(KeyCode.Space));

        player.SetActive(true);

        StartCoroutine(playerMovement.EnableJumpAfterDelay());
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
    public void SkipCutscene()
    {
        playDirector.time = playDirector.duration;
        playDirector.Evaluate();
        playDirector.Stop();
    }
}
