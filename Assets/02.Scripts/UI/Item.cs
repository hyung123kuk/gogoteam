using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType  // ������ ����
    {
        Equipment,
        Used,
        Ingredient,
        ETC,
    }
    public enum EquipType
    {
        Sword,
        Bow,
        Staff,
        boots,
        chest,
        cloak,
        gloves,
        helm,
        pants,
        shoulder

    }
    public enum Type { normal, rare }
    public enum ArmorType { steel, leather, cloth ,none}
    public ItemType itemType; // ������ ����
    public ArmorType armortype;
    public EquipType equipType;
    public Type ItemGrade;
    public int itemEquLevel; // ������ ���� ����
    


    public PlayerST.SwordNames SwordNames;

    [TextArea]
    public string itemText;

    public string itemName; // �������� �̸�

    public Sprite itemImage; // �������� �̹���(�κ� �丮 �ȿ��� ���)
    public GameObject itemPrefab;  // �������� ������ (������ ������ ���������� ��)

    public float _PRICE;        //����
    public float _STR;          //��
    public float _DEX;          //��
    public float _INT;          //����
    public float _DAMAGE;       //���ݷ�
    public float _DEFENCE;      //����
    public float _SKILL_COOLTIME_DEC_PER; //��ų ��Ÿ�� ���ҷ�
    public float _SKILL_ADD_DAMAGE_PER; // ��ų �߰� ���ݷ�
    public float _HP;               //ü��
    public float _MP;               //����
    public float _CRITICAL_PROBABILITY; //ũ��Ƽ��Ȯ��
    public float _CRITICAL_ADD_DAMAGE_PER;//ũ��ƼĿ�߰�������
    public float _MOVE_SPEED;       //�̵��ӵ�

}

