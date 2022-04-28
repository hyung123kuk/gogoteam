using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{
    public static bool iDown=false; // �κ��丮�� ���������� true
    public GameObject Inven; // �κ��丮 â
    static public Slot[] slots;
    [SerializeField]
    private GameObject SlotsParent;
    

    private void Start()
    {
        slots =  SlotsParent.GetComponentsInChildren<Slot>();
        
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) //�κ��丮 �ѱ�/����
        {
            iDown = !iDown;
            if (!iDown) //��������
            {
                Inven.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else //��������
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
                if (slots[i].item != null)  // null �̶�� slots[i].item.itemName �� �� ��Ÿ�� ���� ����
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
