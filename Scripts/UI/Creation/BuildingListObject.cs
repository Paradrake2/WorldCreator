using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public class BuildingListObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI buildingCountText;

    [SerializeField] private BuildingDefinition buildingDefinition;
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private GameObject hoverInfoPanel;
    [SerializeField] private Vector2 hoverInfoOffset = new Vector2(10f, -10f);

    private Coroutine _hoverCoroutine;
    private Vector2 _mousePositionAtHover;
    private Canvas _parentCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _parentCanvas = GetComponentInParent<Canvas>();
        if (hoverInfoPanel != null)
            hoverInfoPanel.SetActive(false);
    }
    public void Initialize(BuildingDefinition definition, BuildingManager manager)
    {
        buildingDefinition = definition;
        buildingManager = manager;
        buildingNameText.text = definition.buildingName;
        buildingManager.OnBuildingsChanged += UpdateCount;
        UpdateCount();
    }

    void OnDestroy()
    {
        if (buildingManager != null)
            buildingManager.OnBuildingsChanged -= UpdateCount;
    }

    private void UpdateCount()
    {
        if (buildingCountText != null)
            buildingCountText.text = buildingManager.GetCount(buildingDefinition).ToString();
    }

    public void OnBuildButtonPressed()
    {
        if (buildingManager.TryBuild(buildingDefinition))
        {
            Debug.Log($"Built {buildingDefinition.buildingName}!");
        }
        else
        {
            Debug.Log($"Failed to build {buildingDefinition.buildingName}.");
        }
    }
    public void OnBuildTenButtonPressed()
    {
        bool builtAtLeastOne = false;
        for (int i = 0; i < 10; i++)
        {
            if (buildingManager.TryBuild(buildingDefinition))
            {
                builtAtLeastOne = true;
            }
            else
            {
                break;
            }
        }
        if (builtAtLeastOne)
            Debug.Log($"Built up to 10 {buildingDefinition.buildingName}!");
        else
            Debug.Log($"Failed to build any {buildingDefinition.buildingName}.");
    }
    public void OnBuildHundredButtonPressed()
    {
        bool builtAtLeastOne = false;
        for (int i = 0; i < 100; i++)
        {
            if (buildingManager.TryBuild(buildingDefinition))
            {
                builtAtLeastOne = true;
            }
            else
            {
                break;
            }
        }
        if (builtAtLeastOne)
            Debug.Log($"Built up to 100 {buildingDefinition.buildingName}!");
        else
            Debug.Log($"Failed to build any {buildingDefinition.buildingName}.");
    }
    public void OnMaxBuildButtonPressed()
    {
        bool builtAtLeastOne = false;
        while (buildingManager.TryBuild(buildingDefinition))
        {
            builtAtLeastOne = true;
        }
        if (builtAtLeastOne)
            Debug.Log($"Built as many {buildingDefinition.buildingName} as possible!");
        else
            Debug.Log($"Failed to build any {buildingDefinition.buildingName}.");
    }
    public void OnDemolishButtonPressed()
    {
        if (buildingManager.TryDemolish(buildingDefinition))
        {
            Debug.Log($"Demolished {buildingDefinition.buildingName}!");
        }
        else
        {
            Debug.Log($"Failed to demolish {buildingDefinition.buildingName}.");
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _mousePositionAtHover = eventData.position;
        _hoverCoroutine = StartCoroutine(ShowPanelAfterDelay());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_hoverCoroutine != null)
        {
            StopCoroutine(_hoverCoroutine);
            _hoverCoroutine = null;
        }
        if (hoverInfoPanel != null)
            hoverInfoPanel.SetActive(false);
    }

    private System.Collections.IEnumerator ShowPanelAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        if (hoverInfoPanel == null) yield break;

        RectTransform panelRect = hoverInfoPanel.GetComponent<RectTransform>();
        if (panelRect != null && _parentCanvas != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.GetComponent<RectTransform>(),
                _mousePositionAtHover,
                _parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _parentCanvas.worldCamera,
                out Vector2 localPoint
            );
            panelRect.anchoredPosition = localPoint + hoverInfoOffset;
        }

        hoverInfoPanel.SetActive(true);
    }
}
