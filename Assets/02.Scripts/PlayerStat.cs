using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public float Level;
    public float Exp;

    public float MONEY;         //가진 돈
    public float _STR;          //힘
    public float _DEX;          //덱
    public float _INT;          //지능
    public float _DAMAGE;       //공격력
    public float _DEFENCE;      //방어력
    public float _SKILL_COOLTIME_DEC_PER; //스킬 쿨타임 감소량
    public float _SKILL_ADD_DAMAGE_PER; // 스킬 추가 공격력
    public float _HP;               //체력
    public float _MP;               //마나
    public float _CRITICAL_PROBABILITY; //크리티컬확률
    public float _CRITICAL_ADD_DAMAGE_PER;//크리티커추가데미지
    public float _MOVE_SPEED;       //이동속도

    public float LEV_ADD_STR;
    public float LEV_ADD_DEX;
    public float LEV_ADD_INT;

    [SerializeField]
    private PlayerST playerST; // 0 => 전사 , 1 => 궁수 , 2 => 법사

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
