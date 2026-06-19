using UnityEngine;

public abstract class MenuBarButton : MonoBehaviour
{
    [SerializeField] public MenuBarUI menuBarUI;
    [SerializeField] public bool isInteractable = true;
    public abstract void OnButtonClicked();
    public void SetInteractable(bool interactable)
    {
        isInteractable = interactable;
    }
}
