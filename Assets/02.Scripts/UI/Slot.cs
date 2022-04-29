using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage;  // 아이템의 이미지
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

    // 아이템 이미지의 투명도 조절
    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 인벤토리에 새로운 아이템 슬롯 추가
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

    // 해당 슬롯의 아이템 갯수 업데이트
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    // 해당 슬롯 하나 삭제
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
                if ( gameObject.layer== LayerMask.NameToLayer("equip") ) //장착하고 있는 아이템 장착 빼기
                {
                    if(gameObject.tag == "weaponslot")
                        WeaponChange.WeaponChange(WeaponChange.basicSword);
                    EmptySlotEq(); //빈 슬롯에 빼기

                }



                else if (item.itemType == Item.ItemType.Equipment) //아이템 장착하기
                {
                    WarriorSword(); //무기 장착하기 (전사용)
                    shoulder(); //어깨 장착하기 (전사용)


                    Chest(); //상의 장착하기
                    Cloak(); //망토 장착하기
                    Boots(); //신발 장착하기
                    gloves();//장갑 장착하기
                    helm();  //모자 장착하기
                    pants(); //하의 장착하기
                    

                }
                else if ( item.itemType == Item.ItemType.Used)
                {
                    // 소비
                    Debug.Log(item.itemName + " 을 사용했습니다.");
                    SetSlotCount(-1);
                }
            }
        }
    }

    private void shoulder()
    {
        if (item != null && item.equipType == Item.EquipType.shoulder )
        {
            if (WarriorSlot.shoulder.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.shoulder);

            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.shoulder);
            }

        }
    }

    private void pants()
    {
        if (item != null && item.equipType == Item.EquipType.pants )
        {
            if (WarriorSlot.pants.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.pants);

            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.pants);
            }

        }
    }

    private void helm()
    {
        if (item != null && item.equipType == Item.EquipType.helm )
        {
            if (WarriorSlot.helm.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.helm);

            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.helm);
            }

        }
    }

    private void gloves()
    {
        if (item != null && item.equipType == Item.EquipType.gloves)
        {
            if (WarriorSlot.gloves.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.gloves);

            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.gloves);
            }

        }
    }

    private void Boots()
    {
        if (item != null && item.equipType == Item.EquipType.boots )
        {
            if (WarriorSlot.boots.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.boots);

            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.boots);
            }

        }
    }

    private void Cloak()
    {
        if (item != null && item.equipType == Item.EquipType.cloak )
        {
            if (WarriorSlot.cloak.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.cloak);

            }
            else // ( 장착이 되어 있지 않을때 )
            {
                EqItem(WarriorSlot.cloak);
            }

        }
    }

    private void Chest()
    {
        if (item != null && item.equipType == Item.EquipType.chest)
        {
            if (WarriorSlot.chest.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.chest);

            }
            else // ( 장착이 되어 있지 않을때 )
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


            if (WarriorSlot.weapon.item != null) // ( 장착이 되어있을때 )
            {
                SwapSlot(WarriorSlot.weapon);

            }
            else // ( 장착이 되어 있지 않을때 )
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
    } //슬롯 아이템 서로 바꾸기

    private void EqItem(Slot EqSlot )
    {
        EqSlot.item = item;
        item = null;

        EqSlot.itemImage.sprite = EqSlot.item.itemImage;
        EqSlot.SetColor(1);
        SetColor(0);

    } // 아이템 장착


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
    } //빈 슬롯 찾아서 넣기


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



        if (gameObject.layer == LayerMask.NameToLayer("equip") && DragSlot.instance.dragSlot.item.itemType != Item.ItemType.Equipment) { Debug.Log("장비아닌거->장비창"); } //장비 아닌 것-> 장비창으로 드래그시 장착을 막는 곳
        else if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("equip") && item!=null && item.itemType != Item.ItemType.Equipment) { Debug.Log("장비창->장비아닌거"); } //장비창 -> 장비 아닌 것 드래그시 장착을 막는 곳
        else if (DragSlot.instance.dragSlot.gameObject.layer == LayerMask.NameToLayer("equip") && item == null) { ChangeSlot(); }
        else if (gameObject.tag == "weaponslot") // 일반슬롯 무기 -> 무기창으로 옮길때
        {
            if (DragSlot.instance.dragSlot.item.equipType == Item.EquipType.Sword)
            {
                DragSlot.instance.dragSlot.WeaponChange.WeaponChange(DragSlot.instance.dragSlot.item.SwordNames);
                ChangeSlot();
            }
        }
        else if (DragSlot.instance.dragSlot.tag == "weaponslot") //무기창 -> 일반 슬롯으로 옮길떄
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