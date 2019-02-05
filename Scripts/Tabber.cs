using Malee;
using UnityEngine;
using UnityEngine.UI;

public class Tabber : MonoBehaviour
{
    //[Reorderable] public GameObjects panels;
    public GameObject tabs;
    public GameObject panels;
    public int activePanel = 0;
    public bool retainPageOnDisable = false;
    public bool debugMode = false;
    
    public virtual void Start()
    {
        for (int i = 0; i < tabs.transform.childCount; i++)
        {
            int index = i;
            tabs.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { CycleTo(index); });
        }

        activePanel = 0;
        DeactivateAllPanels();
        ActivateActivePanel();
    }

    public virtual void DeactivateAllPanels()
    {
        foreach (Transform tr in panels.transform)
        {
            if (debugMode) Debug.Log(tr.name + " set inactive during DeactivateAllPanels()");
            tr.gameObject.SetActive(false);
        }
    }

    public virtual void ActivateActivePanel()
    {
        try
        {
            if (debugMode) Debug.Log(panels.transform.GetChild(activePanel).name + " set active during ActivateActivePanel().");
            panels.transform.GetChild(activePanel).gameObject.SetActive(true);
        }
        catch
        {
            if (debugMode) Debug.Log("Unable to set any panel as active. Have you added anything to 'Panels'?", gameObject);
        }
    }

    public virtual void CyclePanelForwards()
    {
        if (debugMode) Debug.Log("Cycling to next panel...");
        if (debugMode) Debug.Log(panels.transform.GetChild(activePanel).name + " set inactive in CyclePanelForwards().");
        panels.transform.GetChild(activePanel).gameObject.SetActive(false);
        activePanel = (activePanel + 1) % panels.transform.childCount;
        SetTab(activePanel);

        ActivateActivePanel();
    }

    public virtual void CycleForwardEditor()
    {
        CyclePanelForwards();
    }

    public virtual void CycleBackwardsEditor()
    {
        CyclePanelBackwards();
    }

    public virtual void CyclePanelBackwards()
    {
        if (debugMode) Debug.Log("Cycling to previous panel...");
        if (debugMode) Debug.Log(panels.transform.GetChild(activePanel).name + " set inactive in CyclePanelBackwards().");
        panels.transform.GetChild(activePanel).gameObject.SetActive(false);
        activePanel--;
        if (activePanel < 0)
            activePanel = panels.transform.childCount - 1;
        SetTab(activePanel);

        ActivateActivePanel();
    }

    public virtual void CycleTo(int panel)
    {
        if (panels.transform.childCount == 0)
        {
            if(debugMode) Debug.Log("There are no panels. Canceling cycle.");
            activePanel = 0;
            return;
        }

        if (panel >= panels.transform.childCount || panel < 0)
        {
            if (debugMode) Debug.Log("Cannot cycle to panel " + panel + ". Available range: 0 - " + (panels.transform.childCount - 1) + ".");
            
            while (panels.transform.childCount < panel && panel > 0)
                panel--;
        }

        if (debugMode) Debug.Log(panels.transform.GetChild(activePanel).name + " set inactive in CycleTo(" + panel + ").");
        if (activePanel < panels.transform.childCount)
            panels.transform.GetChild(activePanel).gameObject.SetActive(false);
        activePanel = panel;
        SetTab(activePanel);

        ActivateActivePanel();
    }

    protected virtual void OnDisable()
    {
        if (!retainPageOnDisable)
        {
            if (debugMode) Debug.Log("UICycle disabled. Auto cycling back to first panel.");
            CycleTo(0);
            if (tabs != null)
                SetTab(0);
        }
        else
            if (debugMode) Debug.Log("UICycle disabled. Retaining page user left off on.");
    }

    public void SetTab(int tabIndex)
    {
        for (int i = 0; i < tabs.transform.childCount; i++)
        {
            if (tabIndex == i)
                tabs.transform.GetChild(i).GetComponent<Button>().interactable = false;
            else
                tabs.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
    }
}

[System.Serializable]
public class GameObjects : ReorderableArray<GameObject> { }