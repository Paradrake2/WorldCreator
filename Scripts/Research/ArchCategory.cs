using UnityEngine;

[CreateAssetMenu(fileName = "ArchCategory", menuName = "Scriptable Objects/ArchCategory")]
public class ArchCategory : ScriptableObject
{
    [SerializeField] private ResearchCategory[] researchCategories;
    public ResearchCategory[] ResearchCategories => researchCategories;
}
