using UnityEngine;

public class ArchCategoryButton : MonoBehaviour
{
    [SerializeField] private ArchCategory archCategory;
    [SerializeField] private ResearchUI researchUI;
    public void OnClick()
    {
        researchUI.PopulateUI(archCategory);
    }
}
