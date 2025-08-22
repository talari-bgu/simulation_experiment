using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private RobotManager robotManager;
    [SerializeField] private Image ProgressImage;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private CanvasGroup _UIGroup;

    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;

    [SerializeField] private float duration;
    private void Start()
    {
        text = GameObject.Find("ProgressBarText").GetComponent<TextMeshProUGUI>();
        _UIGroup = GameObject.Find("Progress Bar").GetComponent<CanvasGroup>();
        //robotManager = GameObject.Find("SimulationManager").GetComponent<RobotManager>();
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
                if (_UIGroup.alpha < 0)
                {
                    fadeOut = false;
                    // send signal
                }
            }
        }
    }

    public void StartInspect(float duration)
    {
        this.duration = duration;
        ProgressImage.fillAmount = 0;
        fadeIn = true;
    }
    private void StartAnimate()
    {
        StartCoroutine(AnimateProgress(duration));
    }
    private IEnumerator AnimateProgress(float duration)
    {
        text.text = "Inspecting...";
        float time = 0f;


        while (time < duration)
        {
            ProgressImage.fillAmount = time / Mathf.Max(duration, 0.01f);
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
