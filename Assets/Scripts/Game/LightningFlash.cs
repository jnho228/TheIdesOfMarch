using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightningFlash : MonoBehaviour
{
    public AudioSource[] ThunderAudioSource;
    public Light2D GlobalLight2D;

    private float _lightningStrikeTimer;

    private void Start()
    {
        _lightningStrikeTimer = Random.Range(5f, 15f);
    }

    private void Update()
    {
        _lightningStrikeTimer -= Time.deltaTime;

        if (_lightningStrikeTimer < 0)
        {
            StartCoroutine(FlashLightning());
            _lightningStrikeTimer = Random.Range(10f, 60f);
        }
    }

    IEnumerator FlashLightning()
    {
        GlobalLight2D.intensity = Random.Range(0.4f, 0.75f);

        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));

        GlobalLight2D.intensity = 0;

        // Add another chance to do a double flash! -> Bump up the speed and brightness, too
        int doubleFlashChance = Random.Range(0, 10);

        if (doubleFlashChance > 5)
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));

            GlobalLight2D.intensity = Random.Range(0.8f, 0.95f);

            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));

            GlobalLight2D.intensity = 0;
        }

        yield return new WaitForSeconds(Random.Range(0.3f, 0.85f));

        int randomIndex = Random.Range(0, 2);
        ThunderAudioSource[randomIndex].Play();
    }
}
