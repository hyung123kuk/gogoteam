using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public GameObject toolTip;

    public Text itemName;
    public Text itemLevel;
    public Text itemKinds;//������ ���� �ؽ�Ʈ
    public Text itemPosition;
    public Text itemPrice;
    public Text itemAbility;
    public Text portionText;    
    public Text itemText;



    private PlayerST playerST;
    private PlayerStat playerStat;

    public Text[] DamDef;


    public Text[] AddAbility;

    private void Start()
    {
        playerST = FindObjectOfType<PlayerST>();
        playerStat = FindObjectOfType<PlayerStat>();
    }

    public void ToolTipOn(Item item,Vector2 toolTipPoint,int mode)
    {
        transform.position = toolTipPoint;
        if (mode == 0) // �κ��丮 â���� �ߴ� �ǸŰ�
            itemPrice.text = "�ǸŰ� : " + Mathf.Round(item._PRICE * 0.66f).ToString();
        else if (mode == 1) //������ �������� �ߴ� �ǸŰ�
        {
            if (item._PRICE > playerStat.MONEY)
                itemPrice.text = "<color=#FF0000>���Ű� : " + item._PRICE.ToString() + "</color>";
            else
                itemPrice.text = "���Ű� : " + item._PRICE.ToString();
        }

        NameColor(item);
        LevelSet(item);
        ItemExplanationSet(item);
        TypeToKorean(item);
        EqueTypeToKorean(item);
        ItemAbilitySet(item);


        toolTip.SetActive(true);
    }
    public void ToolTipOff()
    {
        toolTip.SetActive(false);

    }


    private void ItemExplanationSet(Item item)
    {
        if (item.itemType == Item.ItemType.Equipment)
        {
            portionText.enabled = false;
            itemText.enabled = true;
            itemText.text = item.itemText; //������ ����â
        }
        else
        {
            itemText.enabled = false;
            portionText.enabled = true;
            portionText.text = item.itemText;
        }
    }

    private void LevelSet(Item item)
    {
        if (item.itemEquLevel != 0f)
        {
            itemLevel.enabled = true;
            if (item.itemEquLevel > playerStat.Level)
            {
                itemLevel.text = "<color=#FF0000>���� : " + item.itemEquLevel.ToString() + "</color>";
            }
            else
            {
                itemLevel.text = "���� : " + item.itemEquLevel.ToString();
            }
        }
        else
            itemLevel.enabled = false;
    }

    private void NameColor(Item item)
    {
        if (item.ItemGrade == Item.Type.normal)
        {
            if (item.itemEquLevel >= 6)
            {
                itemName.text = "<color=#9ACD32>" + item.itemName + "</color>";
            }
            else
            {
                itemName.text = "<color=#C0C0C0>" + item.itemName + "</color>";
            }
        }
        else if(item.ItemGrade == Item.Type.rare)
        {
            itemName.text = "<color=#FFD700>" + item.itemName + "</color>";
        }
        
    }

   

    void TypeToKorean(Item item)
    {       
        if (item.itemType == Item.ItemType.Equipment && (item.equipType == Item.EquipType.Sword || item.equipType == Item.EquipType.Bow || item.equipType == Item.EquipType.Staff))
        {
            itemKinds.text = "Ÿ�� : ����";
        }
        else if (item.itemType == Item.ItemType.Equipment && item.armortype==Item.ArmorType.steel)
        {
            if(playerST.CharacterType == PlayerST.Type.Warrior)
                itemKinds.text = "Ÿ�� : ��ö";
            else
                itemKinds.text = "<color=#FF0000>Ÿ�� : ��ö</color>";
        }
        else if (item.itemType == Item.ItemType.Equipment && item.armortype == Item.ArmorType.leather)
        {
            if (playerST.CharacterType == PlayerST.Type.Archer)
                itemKinds.text = "Ÿ�� : ����";
            else
                itemKinds.text = "<color=#FF0000>Ÿ�� : ����</color>";
        }
        else if (item.itemType == Item.ItemType.Equipment && item.armortype == Item.ArmorType.cloth)
        {
            if (playerST.CharacterType == PlayerST.Type.Mage)
                itemKinds.text = "Ÿ�� : õ";
            else
                itemKinds.text = "<color=#FF0000>Ÿ�� : õ</color>";
        }
        else if(item.itemType == Item.ItemType.Used)
        {
            itemKinds.text = "Ÿ�� : �Ҹ�ǰ";
        }
        else 
        {
            itemKinds.text = "Ÿ�� : ��Ÿ";
        }
    }

    void EqueTypeToKorean(Item item)
    {
        if (item.itemType == Item.ItemType.Equipment)
        {

            itemPosition.enabled = true;
            if (item.equipType == Item.EquipType.Sword)
            {
                if (playerST.CharacterType == PlayerST.Type.Warrior)
                { itemPosition.text = "���� : �ҵ�"; }
                else { itemPosition.text = "<color=#FF0000>���� : �ҵ�</color>"; }
            }
            else if (item.equipType == Item.EquipType.Bow)
            {
                if (playerST.CharacterType == PlayerST.Type.Archer)
                { itemPosition.text = "���� : ����"; }
                else { itemPosition.text = "<color=#FF0000>���� : ����</color>"; }
            }
            else if (item.equipType == Item.EquipType.Staff)
            {
                if (playerST.CharacterType == PlayerST.Type.Mage)
                { itemPosition.text = "���� : ������"; }
                else { itemPosition.text = "<color=#FF0000>���� : ������</color>"; }
            }
            else if (item.equipType == Item.EquipType.boots)
                itemPosition.text = "���� : �Ź�";
            else if (item.equipType == Item.EquipType.chest)
                itemPosition.text = "���� : ����";
            else if (item.equipType == Item.EquipType.cloak)
                itemPosition.text = "���� : ����";
            else if (item.equipType == Item.EquipType.gloves)
                itemPosition.text = "���� : �尩";
            else if (item.equipType == Item.EquipType.helm)
                itemPosition.text = "���� : ���";
            else if (item.equipType == Item.EquipType.pants)
                itemPosition.text = "���� : ����";
            else if (item.equipType == Item.EquipType.shoulder)
                itemPosition.text = "���� : ���";
        }
        else
        {
            itemPosition.enabled = false;
        }
    }

    void ItemAbilitySet(Item item)
    {
        for (int i = 0; i < DamDef.Length; i++)
            DamDef[i].text = "";
        for (int i = 0; i < AddAbility.Length; i++)
            AddAbility[i].text = "";


        int index = 0;
        if (item._DAMAGE != 0f)
        {
            DamDef[0].text = "<color=#FA8072>���ݷ� : " + item._DAMAGE + "</color>";
            index++;
        }

        if (item._DEFENCE != 0f)
        {
            DamDef[index].text = "<color=#ADD8E6>���� : " + item._DEFENCE + "</color>";
            index++;
        }


        if (item._HP != 0f)
        {
            DamDef[index].text = "<color=#8B0000>�ִ� HP : " + item._HP + "</color>";
            index++;
        }

        if (item._MP != 0f)
        {
            DamDef[index].text = "<color=#1E90FF>�ִ� MP : " + item._MP + "</color>";
            index++;
        }

        if (item._STR != 0f)
        {
            DamDef[index].text = "<color=#DC143C>�� : " + item._STR + "  </color>";
        }

        if (item._DEX != 0f)
        {
            DamDef[index].text += "<color=#3CB371>��ø : " + item._DEX + "  </color>";
        }

        if (item._INT != 0f)
        {
            DamDef[index].text += "<color=#00BFFF>���� : " + item._INT + "  </color>";
        }

        index = 0;

        if (item._SKILL_COOLTIME_DEC_PER != 0f)
        {
            AddAbility[0].text = "<color=#00FF7F>��ų ��Ÿ��" + item._SKILL_COOLTIME_DEC_PER + "% ����</color>";
            index++;
        }

        if (item._SKILL_ADD_DAMAGE_PER != 0f)
        {
            AddAbility[index].text = "<color=#00FF7F>��ų ������" + item._SKILL_ADD_DAMAGE_PER + "% ����</color>";
            index++;
        }
        if (item._CRITICAL_PROBABILITY != 0f)
        {
            AddAbility[index].text = "<color=#00FF7F>ũ��Ƽ�� Ȯ��" + item._CRITICAL_PROBABILITY + "% ����</color>";
            index++;
        }
        if (item._CRITICAL_ADD_DAMAGE_PER != 0f)
        {
            AddAbility[index].text = "<color=#00FF7F>ũ��Ƽ�� ������" + item._CRITICAL_ADD_DAMAGE_PER + "% ����</color>";
            index++;
        }
        if (item._MOVE_SPEED != 0f)
            AddAbility[index].text = "<color=#4169E1>�̵��ӵ�" + item._MOVE_SPEED + "% ����</color>";

    }


}
