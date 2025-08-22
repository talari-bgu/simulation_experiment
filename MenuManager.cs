using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MenuManager : MonoBehaviour
{
    public SimulationManager simulationManager;

    public TMP_InputField inputField;
    public TextMeshProUGUI parNum;
    public TextMeshProUGUI configuraion_num;
    public TextMeshProUGUI email_num;

    public GameObject confirmationWindow;

    [SerializeField] int configuration = 0; 
    [SerializeField] int email = 0;


    public void ConfigurationChose(int num)
    {
        configuration = num;
        configuraion_num.text = num.ToString();
    }
    public void EmailChose(int num)
    {
        email = num;
        email_num.text = num.ToString();
    }
    public void NumberClicked(int number)
    {
        inputField.text = inputField.text + number.ToString();
    }
    public void RemoveNumber()
    {
        if (inputField.text.Length > 0) inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
    }

    public void ContinueButton()
    {
        parNum.text = inputField.text;
        confirmationWindow.SetActive(true);
    }

    public void CancelConfirmation()
    {
        confirmationWindow.SetActive(false);
        inputField.text = "";
    }
    public void ConfirmConfirmation()
    {
        simulationManager.LoadTutorial(parNum.text, configuration, email);
    }

}
