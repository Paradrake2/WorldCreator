using UnityEngine;

public class ResearchScreenButton : MenuBarButton
{
    public override void OnButtonClicked()
    {
        if(menuBarUI != null && isInteractable)
        {
            menuBarUI.ShowResearchScreen();
        }
    }
}
