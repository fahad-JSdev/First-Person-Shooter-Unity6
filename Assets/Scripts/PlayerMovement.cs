using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public WeaponSwitch weaponSwitch;

    public float jumpHeight = 3f;
    public float moveSpeed = 6f;
    public float sprintSpeed = 12f;
    private float defaultSpeed;

    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    private bool canJump = true;
    bool isGrounded;
    bool isRunning = false;
    [HideInInspector] public bool gameOverFlag = false;
    [HideInInspector] public bool winFlag = false;


    public float health = 100f;
    public Text playerHealth;


    private GameObject[] obstacleObjects;
    private GameObject[] enemyObjects;

    [HideInInspector] public int obstacleCount;
    [HideInInspector] public int finalDestroyCount;
    public GameObject gameOverPanel;
    public CanvasGroup gameOverPanelCanvasGroup;
    public float fadeDuration = 2f; // Duration of the fade-in effect


    public GameObject pauseMenu;

    int totalEnemies;
    public int killedEnemies;
    public Text totalEnemiesCount;


    public static PlayerMovement instance;

    private void Awake()
    {
        if (instance == null) { instance = this;
        } 
        else 
        { 
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
        playerHealth.text = "Health: " + health;
        defaultSpeed = moveSpeed;

        obstacleObjects = GameObject.FindGameObjectsWithTag("crate");
        enemyObjects = GameObject.FindGameObjectsWithTag("enemy");
        finalDestroyCount = enemyObjects.Length;

        totalEnemies = enemyObjects.Length;
    }
    
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if(isRunning && z <= 0)
        {
            moveSpeed = defaultSpeed;
            isRunning = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            isRunning = !isRunning;
            if (isRunning) moveSpeed = sprintSpeed;
            else  moveSpeed = defaultSpeed;

        }
        if (canJump && Input.GetKeyDown("space") && isGrounded) 
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        Vector3 move = (transform.right * x) + (transform.forward * z);
        controller.Move(move.normalized * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (killedEnemies == finalDestroyCount)
        {
            Time.timeScale = 0.2f;
            winFlag = true;
            GameOver();
        }
        if (health <= 0)
        {
            Time.timeScale = 0.2f;
            GameOver();
        }

        playerHealth.text = "Health: " + health;
        totalEnemiesCount.text = "Enemies: " + killedEnemies + "/" + totalEnemies;

        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverFlag)
        {
            TogglePauseMenu();
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "donut")
        {
            
            health = 100f;
            Destroy(hit.gameObject);
        }     
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            health -= 10f;
        }
    }


    void TogglePauseMenu()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    

    public void GameOver()
    {
        enemyObjects = GameObject.FindGameObjectsWithTag("enemy");

        foreach (GameObject enemy in enemyObjects)
        {
            enemy.SetActive(false);
        }
        playerHealth.enabled = false;
        gameOverFlag = true;
        weaponSwitch.defaultState();

        FadeInGameOverPanel();

        Time.timeScale = 1f;
    }

    public void FadeInGameOverPanel() 
    { 
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration) 
        { 
            elapsedTime += Time.deltaTime;
            gameOverPanelCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration); 
            yield return null; 
        }
        //Make the panel fully interactable after the fade-in
        gameOverPanelCanvasGroup.interactable = true;
        gameOverPanelCanvasGroup.blocksRaycasts = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        SceneManager.LoadScene("FinalScene");
    }

    public IEnumerator EnableJumpAfterDelay()
    {
        canJump = false;

        yield return new WaitForSeconds(0.5f);
        canJump = true;
    }

}
