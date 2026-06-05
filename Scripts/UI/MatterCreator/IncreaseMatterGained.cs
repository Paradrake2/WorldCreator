using UnityEngine;

public class IncreaseMatterGained : MonoBehaviour
{
    [SerializeField] private int matterAmountToAdd = 1;
    [SerializeField] private MatterCreator mc;
    public void IncreaseMatter()
    {
        mc.IncreaseMatterAmount(matterAmountToAdd);
    }
}
