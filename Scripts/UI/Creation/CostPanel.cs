using UnityEngine;
using TMPro;
public class CostPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costText;

    public void UpdateCostText(BuildingDefinition bd, int amount = 1)
    {
        if (bd == null)
        {
            costText.text = "";
            return;
        }
        costText.text = "Cost:\n";
        if (bd.matterCost > 0)
            costText.text += $"Matter: {bd.matterCost * amount}\n";
        if (bd.landCost > 0)
            costText.text += $"Land: {bd.landCost * amount}\n";
        if (bd.buildCosts != null)
        {
            foreach (var cost in bd.buildCosts)
            {
                if (cost.resource == null) continue;
                costText.text += $"{cost.resource.resourceName}: {cost.amount * amount}\n";
            }
        }
    }
}
