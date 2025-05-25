using UnityEngine;

public class DestroyHandler : MonoBehaviour
{
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] private Sprite[] loot;
    private GameObject parentObject;
    [SerializeField] private int durability; //how many hits player needs to destroy
    [SerializeField] 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentObject = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit()
    {
        if (parentObject != null)
        {
            if (durability > 0)
                durability--;
            if (durability <= 0)
            {
                if (loot != null)
                {
                    if (inventoryManager != null)
                    {
                        foreach (var item in loot)
                        {
                            inventoryManager.OnItemGet(item);
                        }
                    }
                }
                Object.Destroy(parentObject);
            }
        }
    }
}
