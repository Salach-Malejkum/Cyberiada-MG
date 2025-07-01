using UnityEngine;

public class InventoryHiding : MonoBehaviour
{
    private bool showInventory = false;
    private CanvasGroup canvasGroup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            showInventory = !showInventory;
        }

        if (showInventory)
            canvasGroup.alpha = 1;
        else canvasGroup.alpha = 0;
        
    }
}
