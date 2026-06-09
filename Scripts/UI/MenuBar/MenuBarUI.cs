using UnityEngine;
using System;
public class MenuBarUI : MonoBehaviour
{
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] private GameObject matterScreenButton;
    [SerializeField] private GameObject creationScreenButton;
    [SerializeField] private GameObject researchScreenButton;
    [SerializeField] private GameObject settingsScreenButton;
    [SerializeField] private GameObject matterScreen;
    [SerializeField] private GameObject creationScreen;
    [SerializeField] private GameObject researchScreen;
    [SerializeField] private GameObject settingsScreen;
    [SerializeField] private BuildingListUI buildingListUI;
    [SerializeField] private ResourceListUI resourceListUI;

    private void OnEnable()
    {
        ScreenManager.creationScreenUnlocked += UnlockCreationScreen;
        ScreenManager.researchScreenUnlocked += UnlockResearchScreen;
    }
    private void OnDisable()
    {
        ScreenManager.creationScreenUnlocked -= UnlockCreationScreen;
        ScreenManager.researchScreenUnlocked -= UnlockResearchScreen;
    }
    private void UnlockCreationScreen()
    {
        if(creationScreenButton != null) creationScreenButton.SetActive(true);
    }
    private void UnlockResearchScreen()
    {
        if(researchScreenButton != null) researchScreenButton.SetActive(true);
    }
    public void ShowMatterScreen()
    {
        if(matterScreen != null) matterScreen.SetActive(true);
        if(creationScreen != null) creationScreen.SetActive(false);
        if(researchScreen != null) researchScreen.SetActive(false);
        if(settingsScreen != null) settingsScreen.SetActive(false);
    }
    public void ShowCreationScreen()
    {
        if(matterScreen != null) matterScreen.SetActive(false);
        if(creationScreen != null) creationScreen.SetActive(true);
        if(researchScreen != null) researchScreen.SetActive(false);
        if(settingsScreen != null) settingsScreen.SetActive(false);
        if(buildingListUI != null) buildingListUI.PopulateBuildingList();
    }
    public void ShowResearchScreen()
    {
        if(matterScreen != null) matterScreen.SetActive(false);
        if(creationScreen != null) creationScreen.SetActive(false);
        if(researchScreen != null) researchScreen.SetActive(true);
        if(settingsScreen != null) settingsScreen.SetActive(false);
    }
    public void ShowSettingsScreen()
    {
        if(matterScreen != null) matterScreen.SetActive(false);
        if(creationScreen != null) creationScreen.SetActive(false);
        if(researchScreen != null) researchScreen.SetActive(false);
        if(settingsScreen != null) settingsScreen.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowMatterScreen();
        if (creationScreenButton != null) creationScreenButton.SetActive(false);
        if (researchScreenButton != null) researchScreenButton.SetActive(false);
        if (settingsScreenButton != null) settingsScreenButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
