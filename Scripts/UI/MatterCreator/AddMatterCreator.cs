using UnityEngine;

public class AddMatterCreator : MonoBehaviour
{
    [SerializeField] private int creatorAmountToAdd = 1;
    [SerializeField] private MatterCreator mc;
    public void AddCreators()
    {
        mc.IncreaseNumberOfCreators(creatorAmountToAdd);
    }

}
