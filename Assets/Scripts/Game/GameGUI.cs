using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameGUI : MonoBehaviour
{
    public Animator screenTransistion;

    public TMP_Text stabCounterText;
    public TMP_Text timerText;

    public Animator gameOverAnim;
    public TMP_Text gameOverText;

    public AudioSource uiSelectSound;

    private float secondTimer = 0f;
    private int minuteTimer = 0;

    private int stabCount = 0;

    void Awake()
    {
        
    }

    void Update()
    {
        UpdateTimer();

        if (stabCount >= 23)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                uiSelectSound.Play();
                StartCoroutine(ReturnToTitle());
            }
        }
    }

    void UpdateTimer()
    {
        if (stabCount >= 23)
            return;

        secondTimer += Time.deltaTime;

        if (secondTimer >= 60)
        {
            secondTimer = 0;
            minuteTimer++;
        }

        timerText.text = minuteTimer + ":" + secondTimer.ToString("00");
    }

    public void AddStab()
    {
        stabCount++;

        stabCounterText.text = "Stabs: " + stabCount;
    }

    public void ShowGameOver()
    {
        gameObject.GetComponent<EnemySpawner>().EndGame();

        gameOverAnim.SetTrigger("show");
        gameOverText.text = "Et tu, Brute?" +
            "\nYou survived for " + timerText.text +
            "\n\n\nPress SPACE to return to title.";
    }

    IEnumerator ReturnToTitle()
    {
        screenTransistion.SetTrigger("CloseScene");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }
}
