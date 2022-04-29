using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item; // ȹ���� ������
    public int itemCount; // ȹ���� �������� ����
    public Image itemImage;  // �������� �̹���
    public WarriorSlot WarriorSlot;
    [SerializeField]
    private Text text_Count;
    public inventory inven;

    public Slot empSlot;

    private PlayerST WeaponChange;

   
    private void Start()
    {
        WeaponChange = FindObjectOfType<PlayerST>();
        if (item!= null)
        {
            itemImage.sprite = item.itemImage;
            SetColor(1);
        }

        


    }

    // ������ �̹����� ���� ����
    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // �κ��丮�� ���ο� ������ ���� �߰�
    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
          
            text_Count.text = itemCount.ToString();
        }
        else
        {
            text_Count.text = "";
            
        }

        SetColor(1);
    }

    // �ش� ������ ������ ���� ������Ʈ
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    // �ش� ���� �ϳ� ����
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "";
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
            if (item != null) 
            {
                if ( gameObject.layer== LayerMask.NameToLayer("equip") ) //�����ϰ� �ִ� ������ ���� ����
                {
                    if(gameObject.tag == "weaponslot")
                        WeaponChange.WeaponChange(WeaponChange.basicSword);
                    EmptySlotEq(); //�� ���Կ� ����

                }



                else if (item.itemType == Item.ItemType.Equipment) //������ �����ϱ�
                {
                    WarriorSword(); //���� �����ϱ� (�����)
                    shoulder(); //��� �����ϱ� (�����)


                    Chest(); //���� �����ϱ�
                    Cloak(); //���� �����ϱ�
                    Boots(); //�Ź� �����ϱ�
                    gloves();//�尩 �����ϱ�
                    helm();  //���� �����ϱ�
                    pants(); //���� �����ϱ�
                    

                }
                else if ( item.itemType == Item.ItemType.Used)
                {
                    // �Һ�
                    Debug.Log(item.itemName + " �� ����߽��ϴ�.");
                    SetSlotCount(-1);
                }
            }
        }
    }

    private void shoulder()
    {
        if (item != null && item.equipType == Item.EquipType.shoulder )
        {
            if (WarriorSlot.shoulder.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.shoulder);

            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.shoulder);
            }

        }
    }

    private void pants()
    {
        if (item != null && item.equipType == Item.EquipType.pants )
        {
            if (WarriorSlot.pants.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.pants);

            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.pants);
            }

        }
    }

    private void helm()
    {
        if (item != null && item.equipType == Item.EquipType.helm )
        {
            if (WarriorSlot.helm.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.helm);

            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.helm);
            }

        }
    }

    private void gloves()
    {
        if (item != null && item.equipType == Item.EquipType.gloves)
        {
            if (WarriorSlot.gloves.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.gloves);

            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.gloves);
            }

        }
    }

    private void Boots()
    {
        if (item != null && item.equipType == Item.EquipType.boots )
        {
            if (WarriorSlot.boots.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.boots);

            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.boots);
            }

        }
    }

    private void Cloak()
    {
        if (item != null && item.equipType == Item.EquipType.cloak )
        {
            if (WarriorSlot.cloak.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.cloak);

            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.cloak);
            }

        }
    }

    private void Chest()
    {
        if (item != null && item.equipType == Item.EquipType.chest)
        {
            if (WarriorSlot.chest.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.chest);

            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.chest);
            }

        }
    }

    private void WarriorSword()
    {
        if (item != null && item.equipType == Item.EquipType.Sword)
        {
            WeaponChange.WeaponChange(item.SwordNames);


            if (WarriorSlot.weapon.item != null) // ( ������ �Ǿ������� )
            {
                SwapSlot(WarriorSlot.weapon);

            }
            else // ( ������ �Ǿ� ���� ������ )
            {
                EqItem(WarriorSlot.weapon);
            }

        }
    }

    private void SwapSlot(Slot swapSlot)
    {
        Item swapItem = item;

        item = swapSlot.item;

        swapSlot.item = swapItem;

        itemImage.sprite = item.itemImage;

        swapSlot.itemImage.sprite = swapSlot.item.itemImage;
    } //���� ������ ���� �ٲٱ�

    private void EqItem(Slot EqSlot )
    {
        EqSlot.item = item;
        item = null;

        EqSlot.itemImage.sprite = EqSlot.item.itemImage;
        EqSlot.SetColor(1);
        SetColor(0);

    } // ������ ����


    private void EmptySlotEq()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {


            if (inventory.slots[i].item == null)
            {

                SetColor(0);
                inventory.slots[i].item = item;
                item = null;

                inventory.slots[i].itemImage.sprite = inventory.slots[i].item.itemImage;
                inventory.slots[i].SetColor(1);
                break;
            }
        }
    } //�� ���� ã�Ƽ� �ֱ�


    public void OnBeginDrag(PointerEventData eventData)
    {
       if(item!=null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(itemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }

   

    public void OnDrop(PointerEventData eventData)
    {



        if (gameObject.layer == LayerMask.NameToLayer("equip") && DragSlot.instance.dragSlot.item.itemType != Item.ItemType.Equipment) { Debug.Log("���ƴѰ�->���â"); } //��� �ƴ� ��-> ���â���� �巡�׽� ������ ���� ��
        else if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("equip") && item!=null && item.itemType != Item.ItemType.Equipment) { Debug.Log("���â->���ƴѰ�"); } //���â -> ��� �ƴ� �� �巡�׽� ������ ���� ��
        else if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("equip") && item == null) { ChangeSlot(); }
        else if (gameObject.tag == "weaponslot") // �Ϲݽ��� ���� -> ����â���� �ű涧
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Sword)
            {
                DragSlot.instance.dragSlot.WeaponChange.WeaponChange(DragSlot.instance.dragSlot.item.SwordNames);
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "weaponslot") //����â -> �Ϲ� �������� �ű拚
        {
            if (item.equipType == Item.EquipType.Sword)
            {
                    WeaponChange.WeaponChange(item.SwordNames);
               
                ChangeSlot();
            }
        }

        else if (gameObject.tag == "chest")
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.chest)
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "chest")
        {
            if (item.equipType == Item.EquipType.chest)
            {
                ChangeSlot();
            }
        }

        else if (gameObject.tag == "pants")
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.pants)
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "pants")
        {
            if (item.equipType == Item.EquipType.pants)
            {
                ChangeSlot();
            }
        }

        else if (gameObject.tag == "helm")
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.helm)
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "helm")
        {
            if (item.equipType == Item.EquipType.helm)
            {
                ChangeSlot();
            }
        }

        else if (gameObject.tag == "shoulder")
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.shoulder)
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "shoulder")
        {
            if (item.equipType == Item.EquipType.shoulder)
            {
                ChangeSlot();
            }
        }

        else if (gameObject.tag == "boots")
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.boots)
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "boots")
        {
            if (item.equipType == Item.EquipType.boots)
            {
                ChangeSlot();
            }
        }

        else if (gameObject.tag == "gloves")
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.gloves)
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "gloves")
        {
            if (item.equipType == Item.EquipType.gloves)
            {
                ChangeSlot();
            }
        }

        else if (gameObject.tag == "cloak")
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.cloak)
            {
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "cloak")
        {
            if (item.equipType == Item.EquipType.cloak)
            {
                ChangeSlot();
            }
        }

       
        else if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }




    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;
        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if(_tempItem !=null)
        {
            DragSlot.instance.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.instance.dragSlot.ClearSlot();
        }
    }
}