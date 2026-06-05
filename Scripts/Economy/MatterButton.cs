using UnityEngine;

public class MatterButton : MonoBehaviour
{
    [SerializeField] private int matterAdded = 1;
    [SerializeField] private ResourceManager resourceManager;

    public void AddMatter()
    {
        resourceManager.AddMatter(matterAdded);
    }

    public void SetResourceManager(ResourceManager manager)
    {
        resourceManager = manager;
    }

    public void IncreaseMatterAdded(int amount)
    {
        matterAdded += amount;
    }

    public int GetMatterAdded()
    {
        return matterAdded;
    }
}
