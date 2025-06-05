using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    private void Awake()
    {
        instance = this;
    }
    public TMP_Text ammoText, remainingAmmoText;
    public TMP_Text healthText;

    public GameObject deathScreen;

    public string mainMenuScene;
    public GameObject pauseScreen;
    public InputActionReference pauseAction;

    void OnEnable()
    {
        pauseAction.action.Enable();
        
    }
    void OnDisable()
    {
        pauseAction.action.Disable();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pauseAction.action.WasPressedThisFrame())
        {
            PauseUnpause();
        }
    }

    public void UpdateAmmoText(int currentAmmo, int remainingAmmo)
    {
        ammoText.text = currentAmmo.ToString();
        remainingAmmoText.text =  "/" + remainingAmmo.ToString();
    }
    public void UpdateHealthText(float currentHealth)
    {
        healthText.text = "Health: " + Mathf.RoundToInt(currentHealth).ToString();
    }
    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PauseUnpause()
    {
        if(pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1f;
        }
        else
        {
            pauseScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0;
        }
    }
}
