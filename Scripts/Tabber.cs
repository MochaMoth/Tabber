using Malee;
using UnityEngine;
using UnityEngine.UI;


namespace FedoraEssentials
{
    [System.Serializable]
    public class Transforms : ReorderableArray<Transform> { }

    [System.Serializable]
    public class Buttons : ReorderableArray<Button> { }

    [System.Serializable]
    public class TabberUseParent
    {
        public bool useParent;
        public Transform parent;
    }

    [System.Serializable]
    public class TabberPanels
    {
        public TabberUseParent usePanelParent;
        public TabberUseParent useTabParent;
        [Reorderable]
        public Buttons tabs;
        [Reorderable]
        public Transforms panels;
    }

    public class Tabber : MonoBehaviour
    {
        public TabberPanels tabberPanels;
        public int activePanel = 0;
        public bool retainPageOnDisable = false;
        public bool debugMode = false;

        public virtual void Start()
        {
            if (tabberPanels.useTabParent.useParent)
            {
                tabberPanels.tabs = new Buttons();
                for (int i = 0; i < tabberPanels.useTabParent.parent.childCount; i++)
                    try { tabberPanels.tabs.Add(tabberPanels.useTabParent.parent.GetChild(i).GetComponent<Button>()); }
                    catch { Debug.LogError(string.Format("Children of tabParent must have a Button component."), this); }
            }

            if (tabberPanels.usePanelParent.useParent)
            {
                tabberPanels.panels = new Transforms();
                for (int i = 0; i < tabberPanels.usePanelParent.parent.childCount; i++)
                    tabberPanels.panels.Add(tabberPanels.usePanelParent.parent.GetChild(i));
            }

            for (int i = 0; i < tabberPanels.tabs.Count; i++)
            {
                int index = i;
                tabberPanels.tabs[i].onClick.AddListener(() => { CycleTo(index); });
            }

            activePanel = 0;
            DeactivateAllPanels();
            ActivateActivePanel();
        }

        public virtual void DeactivateAllPanels()
        {
            foreach (Transform panel in tabberPanels.panels)
            {
                if (debugMode) Debug.Log(panel.name + " set inactive during DeactivateAllPanels()");
                panel.gameObject.SetActive(false);
            }
        }

        public virtual void ActivateActivePanel()
        {
            try
            {
                if (debugMode) Debug.Log(tabberPanels.panels[activePanel].name + " set active during ActivateActivePanel().");
                tabberPanels.panels[activePanel].gameObject.SetActive(true);
                SetTab(activePanel);
            }
            catch
            {
                if (debugMode) Debug.Log("Unable to set any panel as active. Have you added anything to 'Panels'?", gameObject);
            }
        }

        public virtual void CyclePanelForwards()
        {
            if (debugMode) Debug.Log("Cycling to next panel...");
            if (debugMode) Debug.Log(tabberPanels.panels[activePanel].name + " set inactive in CyclePanelForwards().");
            tabberPanels.panels[activePanel].gameObject.SetActive(false);
            activePanel = (activePanel + 1) % tabberPanels.panels.Count;
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
            if (debugMode) Debug.Log(tabberPanels.panels[activePanel].name + " set inactive in CyclePanelBackwards().");
            tabberPanels.panels[activePanel].gameObject.SetActive(false);
            activePanel--;
            if (activePanel < 0)
                activePanel = tabberPanels.panels.Count - 1;
            SetTab(activePanel);

            ActivateActivePanel();
        }

        public virtual void CycleTo(int panel)
        {
            if (tabberPanels.panels.Count == 0)
            {
                if (debugMode) Debug.Log("There are no panels. Canceling cycle.");
                activePanel = 0;
                return;
            }

            if (panel >= tabberPanels.panels.Count || panel < 0)
            {
                if (debugMode) Debug.Log("Cannot cycle to panel " + panel + ". Available range: 0 - " + (tabberPanels.panels.Count - 1) + ".");

                while (tabberPanels.panels.Count < panel && panel > 0)
                    panel--;
            }

            if (debugMode) Debug.Log(tabberPanels.panels[activePanel].name + " set inactive in CycleTo(" + panel + ").");
            if (activePanel < tabberPanels.panels.Count)
                tabberPanels.panels[activePanel].gameObject.SetActive(false);
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
                SetTab(0);
            }
            else
                if (debugMode) Debug.Log("UICycle disabled. Retaining page user left off on.");
        }

        public void SetTab(int tabIndex)
        {
            for (int i = 0; i < tabberPanels.tabs.Count; i++)
            {
                if (tabIndex == i)
                {
                    if (debugMode) Debug.Log(string.Format("Tab '{0}' interactable deactivated in SetTab('{1}')", i, tabIndex));
                    tabberPanels.tabs[i].interactable = false;
                }
                else
                {
                    if (debugMode) Debug.Log(string.Format("Tab '{0}' interactable activated in SetTab('{1}')", i, tabIndex));
                    tabberPanels.tabs[i].interactable = true;
                }
            }
        }
    }
}