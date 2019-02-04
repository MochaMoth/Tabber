using UnityEngine;
using UnityEngine.UI;

public class Tabs : MonoBehaviour
{
    [System.NonSerialized]
    public Button[] tabs;
    public Tabber tabber;

    private void Awake()
    {
        tabs = new Button[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            int index = i;
            tabs[i] = transform.GetChild(i).GetComponent<Button>();
            tabs[i].onClick.AddListener(() => { tabber.CycleTo(index); });
            tabs[i].onClick.AddListener(() => { SetTab(index); });
        }

        SetTab(0);
        tabber.tabs = this;
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
