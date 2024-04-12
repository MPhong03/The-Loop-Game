using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public TextMeshPro ButtonText;
    public void Change()
    {
        ButtonText.color = Color.blue;
    }
}
