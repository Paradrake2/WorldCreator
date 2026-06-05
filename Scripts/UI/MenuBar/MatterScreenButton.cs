using UnityEngine;

public class MatterScreenButton : MonoBehaviour
{
    [SerializeField] private MenuBarUI menuBarUI;
    [SerializeField] private bool isInteractable = true;
    public void OnButtonClicked()
    {
        if(menuBarUI != null && isInteractable)
        {
            menuBarUI.ShowMatterScreen();
        }
    }
    public void SetInteractable(bool interactable)
    {
        isInteractable = interactable;
    }
}
