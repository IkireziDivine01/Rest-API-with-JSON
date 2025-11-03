using UnityEngine;
using UnityEngine.UI;
using TMPro; // Added TextMeshPro
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public DataFetcher dataFetcher;
    public GameObject itemPrefab;       // prefab with InventoryItemUI component
    public Transform contentParent;     // Scroll View -> Content
    
    [Header("TextMeshPro UI Elements")]
    public TMP_Text headerText;         // playerName
    public TMP_Text descriptionText;    // level & health / metadata
    public TMP_Text positionText;       // position
    public TMP_Text errorText;          // error message
    
    [Header("Button")]
    public Button refreshButton;

    void Awake()
    {
        if (dataFetcher != null)
        {
            dataFetcher.OnDataLoaded += OnDataLoaded;
            dataFetcher.OnError += OnError;
        }
        
        if (refreshButton != null && dataFetcher != null)
            refreshButton.onClick.AddListener(() => StartCoroutine(dataFetcher.FetchData()));
        
        if (errorText != null) 
            errorText.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (dataFetcher != null)
        {
            dataFetcher.OnDataLoaded -= OnDataLoaded;
            dataFetcher.OnError -= OnError;
        }
    }

    void OnDataLoaded(Root root)
    {
        // Clear previous items
        foreach (Transform t in contentParent) 
            Destroy(t.gameObject);
        
        // Header & description
        headerText.text = root.record.playerName;
        descriptionText.text = $"Level: {root.record.level}  |  Health: {root.record.health}";
        
        // Position
        var p = root.record.position;
        positionText.text = $"Position: X:{p.x} Y:{p.y} Z:{p.z}";
        
        // Inventory list
        if (root.record.inventory != null && root.record.inventory.Count > 0)
        {
            foreach (var inv in root.record.inventory)
            {
                GameObject go = Instantiate(itemPrefab, contentParent);
                InventoryItemUI ui = go.GetComponent<InventoryItemUI>();
                ui.SetData(inv);
            }
        }
        
        if (errorText != null) 
            errorText.gameObject.SetActive(false);
    }

    void OnError(string msg)
    {
        Debug.LogError(msg);
        
        if (errorText != null)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = msg;
        }
        
        // Optionally clear UI
        headerText.text = "Error loading data";
        descriptionText.text = "";
        positionText.text = "";
    }
}