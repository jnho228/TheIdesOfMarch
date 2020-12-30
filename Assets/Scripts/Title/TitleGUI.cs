using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleGUI : MonoBehaviour
{
    public Animator canvasAnimator;
    public Animator screenTransistion;
    public AudioSource uiSelectSound;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LoadGame());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit the game.
            Application.Quit(0);
        }
    }

    IEnumerator LoadGame()
    {
        canvasAnimator.SetTrigger("startGame");
        uiSelectSound.Play();
        screenTransistion.SetTrigger("CloseScene");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
}
