using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightningFlash : MonoBehaviour
{
    public AudioSource[] ThunderSource;
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
            StartCoroutine(ShowLightning());
            _lightningStrikeTimer = Random.Range(10f, 60f);
        }
    }

    IEnumerator ShowLightning()
    {
        GlobalLight2D.intensity = Random.Range(0.5f, 0.85f);

        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));

        GlobalLight2D.intensity = 0;

        yield return new WaitForSeconds(Random.Range(0.3f, 0.85f));

        int randomIndex = Random.Range(0, 2);
        ThunderSource[randomIndex].Play();
    }
}
