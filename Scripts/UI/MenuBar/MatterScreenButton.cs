using UnityEngine;

public class MatterScreenButton : MenuBarButton
{
    public override void OnButtonClicked()
    {
        if(menuBarUI != null && isInteractable)
        {
            menuBarUI.ShowMatterScreen();
        }
    }
}
