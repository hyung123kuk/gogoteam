using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public float Level;
    public float Exp;

    public float MONEY;         //���� ��
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

    public float LEV_ADD_STR;
    public float LEV_ADD_DEX;
    public float LEV_ADD_INT;

    [SerializeField]
    private PlayerST playerST; // 0 => ���� , 1 => �ü� , 2 => ����

    public Slot[] equSlots;

    void Start()
    {
        playerST = GetComponent<PlayerST>();
        BasicCharacterStatUpdate();
        LevelUpStat();
        StatAllUpdate();

    }

    public void BasicCharacterStatUpdate()
    {
        if (playerST.CharacterType == PlayerST.Type.Warrior)
        {
            _STR += 20f;
            _DEX += 15f;
            _INT += 5f;
        }
        else if (playerST.CharacterType == PlayerST.Type.Archer)
        {
            _STR += 10f;
            _DEX += 25f;
            _INT += 5f;
        }
        else if (playerST.CharacterType == PlayerST.Type.Archer)
        {
            _STR += 10f;
            _DEX += 10f;
            _INT += 20f;
        }
    }


    public void LevelUpStat()
    {
        _STR += LEV_ADD_STR;
        _DEX += LEV_ADD_DEX;
        _INT += LEV_ADD_INT;

    }

    
    public void StatAllUpdate()
    {
        for(int i = 0; i < equSlots.Length; i++)
        {
            if (equSlots[i].item != null)
            {
                _STR += equSlots[i].item._STR;
                _DEX += equSlots[i].item._DEX;
                _INT += equSlots[i].item._INT;
                _DAMAGE += equSlots[i].item._DAMAGE;
                _DEFENCE += equSlots[i].item._DEFENCE;
                _SKILL_COOLTIME_DEC_PER += equSlots[i].item._SKILL_COOLTIME_DEC_PER;
                _SKILL_ADD_DAMAGE_PER += equSlots[i].item._SKILL_ADD_DAMAGE_PER;
                _HP += equSlots[i].item._HP;
                _MP += equSlots[i].item._MP;
                _CRITICAL_PROBABILITY += equSlots[i].item._CRITICAL_PROBABILITY;
                _CRITICAL_ADD_DAMAGE_PER += equSlots[i].item._CRITICAL_ADD_DAMAGE_PER;
                _MOVE_SPEED += equSlots[i].item._MOVE_SPEED;
            }
        }

        Statcalculate();
    }

    public void Statcalculate()
    {
        _DAMAGE += ((_STR * 1f) + (_DEX * 0.5f) + (_INT * 0.25f)) * Level;
        _DEFENCE += ((_STR * 0.2f) + (_DEX * 0.1f)) * Level;
        _SKILL_COOLTIME_DEC_PER += _INT;
        _SKILL_ADD_DAMAGE_PER += ((_INT * 0.5f) + (_DEX * 0.25f)) * Level;
        _HP += (200 + (_STR * 5) + (_DEX * 3) + (_INT * 1)) * Level;
        _MP += (200 + (_INT * 10) + (_DEX * 2) + (_STR * 2)) * Level / 2;
        _CRITICAL_PROBABILITY += (_STR * 0.25f) + (_DEX * 0.25f);
        _CRITICAL_ADD_DAMAGE_PER += (30 + (_STR * 1f) + (_DEX * 0.5f));
        _MOVE_SPEED += _DEX * 0.5f;


    }

}
