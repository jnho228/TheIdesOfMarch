using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleGUI : MonoBehaviour
{
    public Animator screenTransistion;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
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
        screenTransistion.SetTrigger("CloseScene");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
}
