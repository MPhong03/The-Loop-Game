using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanelButton : MonoBehaviour
{
    public GameObject shop;

    public void CloseShop()
    {
        shop.SetActive(false);
    }
}
