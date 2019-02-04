using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCloseMenu : MonoBehaviour
{
    public Tabber tabberMenu;

    void Update()
    {
        if (Input.GetButtonDown("MenuButton"))
            tabberMenu.gameObject.SetActive(!tabberMenu.gameObject.activeSelf);
    }
}
