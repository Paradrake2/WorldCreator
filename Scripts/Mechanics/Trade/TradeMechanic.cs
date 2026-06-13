using UnityEngine;

public static class TradeMechanic 
{
    public static void Activate()
    {
        // Implement the activation logic for the trade mechanic here
        // fire event to unlock trade screen
        // TradeScreen.OnTradeScreenUnlocked?.Invoke();
        Debug.Log("Trade Mechanic Activated!");
    }
    public static void Trade(ResourceDefinition inputResource, ResourceDefinition outputResource, float inputAmount)
    {
        // Calculate the trade value of the input resource
        float tradeValue = inputAmount * inputResource.tradeInputValue;

        // Calculate how much of the output resource to give based on its trade output value
        float outputAmount = tradeValue / outputResource.tradeOutputValue;

        ProductionManager manager = Object.FindAnyObjectByType<ProductionManager>();
        manager.ModifyResource(inputResource, -inputAmount);
        manager.ModifyResource(outputResource, outputAmount);

        Debug.Log($"Traded {inputAmount} {inputResource.resourceName} for {outputAmount} {outputResource.resourceName}");
    }
}
