using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameGUI : MonoBehaviour
{
    public TMP_Text stabCounterText;
    public TMP_Text timerText;
    public TMP_Text gameOverText;
    public Animator screenTransistion;
    public Animator gameOverAnim;
    public AudioSource uiSelectSound;

    private float _secondTimer = 0f;
    private int _minuteTimer = 0;
    private int _stabCount = 0;

    void Update()
    {
        UpdateTimer();

        if (_stabCount >= 23)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                uiSelectSound.Play();
                StartCoroutine(ReturnToTitle());
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiSelectSound.Play();
                StartCoroutine(ReturnToTitle());
            }
        }
    }

    void UpdateTimer()
    {
        if (_stabCount >= 23)
            return;

        _secondTimer += Time.deltaTime;

        if (_secondTimer >= 60)
        {
            _secondTimer = 0;
            _minuteTimer++;
        }

        timerText.text = _minuteTimer + ":" + _secondTimer.ToString("00");
    }

    public void AddStab()
    {
        _stabCount++;

        stabCounterText.text = "Stabs: " + _stabCount;
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
