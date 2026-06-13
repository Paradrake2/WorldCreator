using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class ResearchNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ResearchNodeData nodeData;
    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private TextMeshProUGUI nodeNameText;
    [SerializeField] private ResearchManager researchManager;
    public ResearchNodeData NodeData => nodeData;
    public bool IsUnlocked => isUnlocked;
    public void OnClick()
    {
        if (isUnlocked)
        {
            Debug.Log(nodeData.NodeName + " is already unlocked.");
            return;
        }
        // Check if prerequisites are met
        foreach (var prereq in nodeData.Prerequisites)
        {
            if (!prereq.IsUnlocked)
            {
                Debug.Log("Cannot unlock " + nodeData.NodeName + ". Prerequisite " + prereq.NodeName + " is not unlocked.");
                return;
            }
        }
        // Check if player has enough resources

        // If all checks pass, unlock the node
        nodeData.Unlock();
        nodeData.OnUnlock();
        isUnlocked = true;
        // change appearance to show it's unlocked, e.g. change color or enable a checkmark
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        // hide tooltip
        throw new System.NotImplementedException();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        // show tooltip with nodeData info like costs
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nodeNameText.text = nodeData.NodeName;
        if (researchManager == null)
        {
            researchManager = FindAnyObjectByType<ResearchManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
