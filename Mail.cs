using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mail : MonoBehaviour
{
    [TextArea(2, 10)]
    public string subject;
    [TextArea(15, 20)]
    public string content;
}
