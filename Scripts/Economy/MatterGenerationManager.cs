using UnityEngine;

/// <summary>
/// Always-active manager that owns the matter generation loop.
/// MatterCreator prefabs register themselves here and push their values.
/// When prefabs are disabled the registered values remain unchanged,
/// so generation continues uninterrupted.
/// </summary>
public class MatterGenerationManager : MonoBehaviour
{
    public static MatterGenerationManager Instance { get; private set; }

    [SerializeField] private ResourceManager resourceManager;

    // Each registered entry represents one MatterCreator prefab's contribution.
    private struct GeneratorEntry
    {
        public float matterAmount;
        public int numberOfCreators;
        public float creationInterval;
        public float timer;
    }

    private GeneratorEntry[] entries = new GeneratorEntry[0];
    private int entryCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (resourceManager == null)
            resourceManager = FindAnyObjectByType<ResourceManager>();
    }

    /// <summary>
    /// Called by a MatterCreator on Start. Returns an ID used to update values later.
    /// </summary>
    public int Register(float matterAmount, int numberOfCreators, float creationInterval)
    {
        // Grow array if needed
        if (entryCount >= entries.Length)
        {
            var bigger = new GeneratorEntry[Mathf.Max(4, entries.Length * 2)];
            System.Array.Copy(entries, bigger, entries.Length);
            entries = bigger;
        }

        int id = entryCount++;
        entries[id] = new GeneratorEntry
        {
            matterAmount = matterAmount,
            numberOfCreators = numberOfCreators,
            creationInterval = creationInterval,
            timer = 0f
        };
        return id;
    }

    /// <summary>
    /// Called by a MatterCreator whenever its values change (upgrade purchased, etc.).
    /// </summary>
    public void UpdateEntry(int id, float matterAmount, int numberOfCreators, float creationInterval)
    {
        if (id < 0 || id >= entryCount) return;
        entries[id].matterAmount = matterAmount;
        entries[id].numberOfCreators = numberOfCreators;
        entries[id].creationInterval = creationInterval;
    }

    private void Update()
    {
        for (int i = 0; i < entryCount; i++)
        {
            entries[i].timer += Time.deltaTime;
            if (entries[i].timer >= entries[i].creationInterval)
            {
                float generated = entries[i].matterAmount * entries[i].numberOfCreators;
                resourceManager.AddMatter(generated);
                entries[i].timer = 0f;
            }
        }
    }
}
