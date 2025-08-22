using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public RobotManager robotManager;
    public ProgressBarScript progressBar;

    public TextMeshProUGUI lowLevel;
    public TextMeshProUGUI highLevel;

    public CanvasGroup leftArrowCanvasGroup;
    public CanvasGroup rightArrowCanvasGroup;


    public void UISetLevel(string level)
    {
        if (level == "low")
        {
            lowLevel.color = Color.white;
            highLevel.color = Color.grey;
        }
        else if (level == "high")
        {
            lowLevel.color = Color.grey;
            highLevel.color = Color.white;
        }
        else Debug.Log("Cant show UI level");
    }

    public void UISetArrow(string side)
    {
        if (side == "left")
        {
            leftArrowCanvasGroup.alpha = 1;
            rightArrowCanvasGroup.alpha = 0;
        }
        else if (side == "right")
        {
            leftArrowCanvasGroup.alpha = 0;
            rightArrowCanvasGroup.alpha = 1;
        }
        else if (side == "disable")
        {
            leftArrowCanvasGroup.alpha = 0;
            rightArrowCanvasGroup.alpha = 0;
        }
        else Debug.Log("Cant show UI arrow");
    }

    public void StartInspectingUI(float duration)
    {
        progressBar.StartInspect(duration);
    }

    public void InspectingDone()
    {
        robotManager.InspectingDone();
    }


}
