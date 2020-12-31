using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class TutorialGUI : MonoBehaviour
{
    public Animator screenTransistion;

    private AudioSource _audioSource;
    private float _autoLoadLevelTimer = 5f;
    private bool _isLoadingLevel = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_isLoadingLevel)
            return;

        _autoLoadLevelTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) || _autoLoadLevelTimer < 0)
        {
            StartCoroutine(LoadLevel());
        }
    }

    IEnumerator LoadLevel()
    {
        _isLoadingLevel = true;
        _audioSource.Play();
        screenTransistion.Play("Scene Close");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }
}
