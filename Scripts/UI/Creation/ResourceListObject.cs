using UnityEngine;
using TMPro;
public class ResourceListObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resourceNameText;
    [SerializeField] private TextMeshProUGUI resourceAmountText;
    [SerializeField] private ResourceDefinition resourceDefinition;
    [SerializeField] private ProductionManager productionManager;

    public void Initialize(ResourceDefinition definition, ProductionManager production)
    {
        resourceDefinition = definition;
        // Unsubscribe from any previous manager before reassigning.
        if (productionManager != null)
            productionManager.OnResourceChanged -= HandleResourceChanged;
        productionManager = production;
        productionManager.OnResourceChanged += HandleResourceChanged;
        resourceNameText.text = definition.resourceName;
        UpdateAmount(production.GetResource(definition));
    }

    void OnDestroy()
    {
        if (productionManager != null)
            productionManager.OnResourceChanged -= HandleResourceChanged;
    }

    void OnEnable()
    {
    }

    void OnDisable()
    {
    }

    public void UpdateAmount(float amount)
    {
        resourceAmountText.text = amount.ToString("F1");
    }

    private void HandleResourceChanged(ResourceDefinition def, float amount)
    {
        if (def == resourceDefinition)
            UpdateAmount(amount);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
