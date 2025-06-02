using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }
}
