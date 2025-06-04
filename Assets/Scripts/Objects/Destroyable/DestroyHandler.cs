<<<<<<< Updated upstream
=======
using Unity.VisualScripting;
>>>>>>> Stashed changes
using UnityEngine;

public class DestroyHandler : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] private Sprite[] loot;
    private GameObject parentObject;
    [SerializeField] private int durability; //how many hits player needs to destroy
    [SerializeField] 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentObject = this.gameObject;
=======
    [SerializeField] private GameObject loot;
    [SerializeField] private int durability; //number of hits to destroy object
    private GameObject mainObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainObject = this.gameObject;
        
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHit()
    {
<<<<<<< Updated upstream
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
=======
        if (durability > 0)
            durability--;
        if (durability == 0)
        {
            if (loot != null)
            {
                Object.Destroy(mainObject);
            }
            else
            {
                //put loot logic here, propably after adding inventory
                Object.Destroy(mainObject);

>>>>>>> Stashed changes
            }
        }
    }
}
