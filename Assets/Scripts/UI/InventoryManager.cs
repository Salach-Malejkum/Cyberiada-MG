using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Image[] inventorySlots;
    [SerializeField] PlayerMove playerMove;
    private int numberOfItems; //how many different items did player collect

    [Header("List of key items")]
    [SerializeField] Sprite doubleJumpItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 16; i++)
        {
            inventorySlots[i] = transform.Find("itemSlot" + i).GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (System.Array.Exists(inventorySlots, sprite => sprite == doubleJumpItem))
        {
            Debug.Log("Player should now be able to double jump");
            playerMove.canDoubleJump = true;
        }
    }

    public void OnItemGet(Sprite item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i == numberOfItems)
            {
                inventorySlots[i].sprite = item;
                switch (item)
                {
                    default:
                        break;
                    case var _ when item == doubleJumpItem:
                        playerMove.canDoubleJump = true;
                        break;
                }
            }
        }
        numberOfItems++;
    }
}
