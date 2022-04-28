using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{
    public static bool iDown=false; // 인벤토리가 열려있으면 true
    public GameObject Inven; // 인벤토리 창
    static public Slot[] slots;
    [SerializeField]
    private GameObject SlotsParent;
    

    private void Start()
    {
        slots =  SlotsParent.GetComponentsInChildren<Slot>();
        
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) //인벤토리 켜기/끄기
        {
            iDown = !iDown;
            if (!iDown) //꺼져있음
            {
                Inven.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else //켜져있음
            {
                Inven.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }

        }
        
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)  // null 이라면 slots[i].item.itemName 할 때 런타임 에러 나서
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
   
}
