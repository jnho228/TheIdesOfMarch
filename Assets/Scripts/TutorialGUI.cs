using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class TutorialGUI : MonoBehaviour
{
    public Animator screenTransistion;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LoadLevel());
        }
    }

    IEnumerator LoadLevel()
    {
        screenTransistion.Play("Scene Close");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }
}
