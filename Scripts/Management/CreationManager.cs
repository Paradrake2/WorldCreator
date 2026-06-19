using UnityEngine;

public class CreationManager : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private Transform panel;
    [SerializeField] private GameObject matterCreatorPrefab; // basic matter creator, spawns at the start
    [SerializeField] private GameObject secondMatterCreatorPrefab; //advanced matter creator
    [SerializeField] private GameObject thirdMatterCreatorPrefab; // even more advanced matter creator

    void Start()
    {
        resourceManager.RegisterMatterThreshold(10, SpawnMatterCreator);
        resourceManager.RegisterMatterThreshold(55000, UnlockSecondMatterCreator);
        resourceManager.RegisterMatterThreshold(100000, UnlockThirdMatterCreator);
    }

    private void SpawnMatterCreator()
    {
        Instantiate(matterCreatorPrefab, panel);
    }
    private void UnlockSecondMatterCreator()
    {
        Instantiate(secondMatterCreatorPrefab, panel);
    }
    private void UnlockThirdMatterCreator()
    {
        Instantiate(thirdMatterCreatorPrefab, panel);
    }
}
