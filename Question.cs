using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : MonoBehaviour
{
    [TextArea(2, 10)]
    public string question;
    [TextArea(2, 10)]
    public string[] answers;
}
