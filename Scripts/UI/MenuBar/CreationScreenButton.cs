using UnityEngine;

public class CreationScreenButton : MonoBehaviour
{
    [SerializeField] private MenuBarUI menuBarUI;
    [SerializeField] private bool isInteractable = true;
    public void OnButtonClicked()
    {
        if(menuBarUI != null && isInteractable)
        {
            menuBarUI.ShowCreationScreen();
        }
    }
    public void SetInteractable(bool interactable)
    {
        isInteractable = interactable;
    }
}
