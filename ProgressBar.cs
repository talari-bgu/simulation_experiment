using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarScript : MonoBehaviour
{
    public UIController UIController;
    public TextMeshProUGUI text;

    [SerializeField] private Image _ProgressImage;
    [SerializeField] private CanvasGroup _UIGroup;

    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    [SerializeField] private float duration;
    private void Start()
    {
        _ProgressImage = GetComponent<Image>();
        _UIGroup = GetComponent<CanvasGroup>();
        _UIGroup.alpha = 0;
    }
    private void Update()
    {
        if (fadeIn)
        {
            if (_UIGroup.alpha < 1)
            {
                _UIGroup.alpha += 2*Time.deltaTime;
                if (_UIGroup.alpha >= 1)
                {
                    fadeIn = false;
                    StartAnimate();
                }
            }
        }
        if (fadeOut)
        {
            if (_UIGroup.alpha >= 0)
            {
                _UIGroup.alpha -= 2*Time.deltaTime;
                if (_UIGroup.alpha == 0)
                {
                    fadeOut = false;
                    UIController.InspectingDone();
                }
            }
        }
    }

    public void StartInspect(float duration)
    {
        this.duration = duration;
        _ProgressImage.fillAmount = 0;
        text.text = "Inspecting...";
        fadeIn = true;
    }
    private void StartAnimate()
    {
        StartCoroutine(AnimateProgress(duration));
    }
    private IEnumerator AnimateProgress(float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            _ProgressImage.fillAmount = time / Mathf.Max(duration, 0.01f);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (time > duration)
        {
            text.text = "Completed";
            yield return new WaitForSeconds(2);
            fadeOut = true;
        }
    }

}
