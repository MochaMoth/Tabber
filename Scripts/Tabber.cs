using Malee;
using UnityEngine;

public class Tabber : MonoBehaviour
{
    [Reorderable] public GameObjects panels;
    public int activePanel = 0;
    public bool retainPageOnDisable = false;
    public bool debugMode = false;

    [System.NonSerialized]
    public Tabs tabs;
    
    public virtual void Start()
    {
        activePanel = 0;
        DeactivateAllPanels();
        ActivateActivePanel();
    }

    public virtual void DeactivateAllPanels()
    {
        foreach (GameObject gob in panels)
        {
            if (debugMode) Debug.Log(gob.name + " set inactive during DeactivateAllPanels()");
            gob.SetActive(false);
        }
    }

    public virtual void ActivateActivePanel()
    {
        try
        {
            if (debugMode) Debug.Log(panels[activePanel].name + " set active during ActivateActivePanel().");
            panels[activePanel].SetActive(true);
        }
        catch
        {
            if (debugMode) Debug.Log("Unable to set any panel as active. Have you added anything to 'Panels'?", gameObject);
        }
    }

    public virtual void CyclePanelForwards()
    {
        if (debugMode) Debug.Log("Cycling to next panel...");
        if (debugMode) Debug.Log(panels[activePanel].name + " set inactive in CyclePanelForwards().");
        panels[activePanel].SetActive(false);
        activePanel = (activePanel + 1) % panels.Length;

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
        if (debugMode) Debug.Log(panels[activePanel].name + " set inactive in CyclePanelBackwards().");
        panels[activePanel].SetActive(false);
        activePanel--;
        if (activePanel < 0)
            activePanel = panels.Length - 1;

        ActivateActivePanel();
    }

    public virtual void CycleTo(int panel)
    {
        if (panels.Length == 0)
        {
            if(debugMode) Debug.Log("There are no panels. Canceling cycle.");
            activePanel = 0;
            return;
        }

        if (panel >= panels.Length || panel < 0)
        {
            if (debugMode) Debug.Log("Cannot cycle to panel " + panel + ". Available range: 0 - " + (panels.Length - 1) + ".");
            
            while (panels.Length < panel && panel > 0)
                panel--;
        }

        if (debugMode) Debug.Log(panels[activePanel].name + " set inactive in CycleTo(" + panel + ").");
        if (activePanel < panels.Length)
            panels[activePanel].SetActive(false);
        activePanel = panel;

        ActivateActivePanel();
    }

    protected virtual void OnDisable()
    {
        if (!retainPageOnDisable)
        {
            if (debugMode) Debug.Log("UICycle disabled. Auto cycling back to first panel.");
            CycleTo(0);
            if (tabs != null)
                tabs.SetTab(0);
        }
        else
            if (debugMode) Debug.Log("UICycle disabled. Retaining page user left off on.");
    }
}

[System.Serializable]
public class GameObjects : ReorderableArray<GameObject> { }