using UnityEngine;
using UnityEngine.UI;

public class Tabs : MonoBehaviour
{
    public Button[] tabs;

    private void Awake()
    {
        if (tabs.Length == 0)
        {
            tabs = new Button[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
                tabs[i] = transform.GetChild(i).GetComponent<Button>();
        }

        SetTab(0);
    }

    public void SetTab(int tabIndex)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if (tabIndex == i)
                tabs[i].interactable = false;
            else
                tabs[i].interactable = true;
        }
    }
}
