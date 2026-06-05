using UnityEngine;
using System;
public class ScreenManager : MonoBehaviour
{
    public static event Action creationScreenUnlocked; 
    public static event Action researchScreenUnlocked;
    [SerializeField] private ResourceManager resourceManager;
    void OnEnable()
    {
        // Subscribe to events if needed
    }
    private void UnlockCreationScreen()
    {
        creationScreenUnlocked?.Invoke();
    }
    private void UnlockResearchScreen()
    {
        researchScreenUnlocked?.Invoke();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resourceManager.RegisterMatterThreshold(1000, UnlockCreationScreen);
        resourceManager.RegisterMatterThreshold(5000, UnlockResearchScreen);
    }

}
