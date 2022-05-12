﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Weapons : MonoBehaviour
{
    public enum Type { Melee, Range, Mage }; //근접무기와 원거리무기 구분
    public Type type;
    public float damage; //근접무기 공격력 / 원거리무기는 Arrow 스크립트
    public float rate; //공격속도

    bool Key1; //키보드 1번입력
    bool Key2; //키보드 2번입력
    public bool Key3; //키보드 3번입력
    public bool Key3Up;

    public static Weapons weapons;
    public Animator anim;
    private Rigidbody rigid;
    public BoxCollider meleeArea; //근딜범위
    public bool isSootReady = true;


    public PlayerST playerST;
    public AttackDamage attackdamage;
    public PlayerStat playerstat;
  

    /*=========================궁수 스킬 관련===================================*/
    public bool isEnergyReady; //3스킬쓰고있는상태
    public bool isEnergy1; //3스킬 1.5초장전상태
    public bool isEnergy2; //3스킬 5초 풀장전상태
    public float EnergyReady; //3스킬 1.5초장전 시간재기
    public float EnergyReady2; //3스킬 5초장전 시간재기
    public float EnergyChargingTime = 5f; //풀차징 제한시간


    [Header("궁수 관련")]
    public Transform arrowPos; //화살나가는위치
    public Transform Arc3SkillPos; //3스킬 나가는 위치
    public GameObject ArcDefaultAttack; //기본화살
    public GameObject Arc1Skilarrow; //독화살
    public GameObject Arc2Skilarrow; //폭탄화살
    public GameObject Arc3SkillBuff1;  //기모을때 버프
    public GameObject Arc3SkillBuff2;  //기모을때 버프2차
    public GameObject Arc3SkillBuff3;  //기모을때 버프3차
    public GameObject Arc3SkillArrow1; //1화살 발사이펙트
    public GameObject Arc3SkillArrow2; //2화살 발사이펙트

    /*=========================마법사 스킬 관련===================================*/
    private float MeteoCasting; //3스킬 캐스팅시간
    private float MeteoMaxCasting = 4f; //3스킬 최대 캐스팅시간


    public bool isLightning;  //현재 스킬사용중?
    public bool isIceage;
    public static bool isMeteo;
    public bool isMeteoShot;

    [Header("마법사 관련")]
    public Transform MagicPos; //마법나가는위치
    public GameObject MageDefaultAttack; //마법사 기본공격
    public GameObject Mage1SkillEff; //마법사1스킬
    public Transform Mage1SkillPos1; //1스킬 도착위치
    public Transform Mage1SkillPos2;
    public Transform Mage1SkillPos3;
    public GameObject Mage2SkillEff; //2스킬 이펙트
    public GameObject Mage2SkillReadyEff; //2스킬 준비동작이펙트
    public Transform Mage3SkillPos1; //3스킬 시작위치
    public Transform Mage3SkillPos2; //3스킬 도착위치
    public GameObject Mage3SkillPosEff; //3스킬 도착위치 이펙트
    public GameObject Mage3SkillEff; //3스킬 이펙트
    public GameObject Mage3SkillPlayerEff; //3스킬 이펙트 플레이어쪽
    public GameObject Skillarea; //켜지면 데미지만
    public GameObject Skillarea2; //켜지면 데미지만
    public GameObject CCarea;  //켜지면 CC기 


    private void Start()
    {
        weapons = this;
        anim = GetComponentInParent<Animator>();
    }
    private void Update()
    {
        Key1 = Input.GetButtonDown("Key1"); //1번키
        Key2 = Input.GetButtonDown("Key2"); //2번키
        Key3 = Input.GetButton("Key3"); //3번키 꾹
        Key3Up = Input.GetKeyUp(KeyCode.Alpha3);

        if (type == Type.Range)
        {
            BombArrow();
            EnergyArrow();
        }
        else if (type == Type.Mage)
        {
            LightningBall();
            IceAge();
            Meteo();

        }

        if (attackdamage.Duration_Buff && playerST.ImWar)
        {
            rate = 0.45f;
        }
        else if (!attackdamage.Duration_Buff && playerST.ImWar)
        {
            playerST.BuffEff.SetActive(false);
            rate = 0.6f;
        }
    }
    private void FixedUpdate()
    {
        attackdamage.SkillPassedTimeFucn();
    }
    //============================궁수스킬========================================
    void BombArrow() //폭탄화살 궁수용
    {
        if (Key2 && !playerST.isDodge && !isEnergyReady && !PlayerST.isJump
            && !PlayerST.isStun && !playerST.isRun && attackdamage.Usable_Skill2)
        {
            StartCoroutine(BombArrowPlay());
        }
    }
    IEnumerator BombArrowPlay()
    {
        attackdamage.Skill_2_Cool();
        anim.SetBool("isBomb", true);
        GameObject bombarrow = Instantiate(Arc2Skilarrow, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigid = bombarrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward * 20;
        Arrow arrow = bombarrow.GetComponent<Arrow>(); //스킬데미지설정
        arrow.damage = attackdamage.Skill_2_Damamge();

        Destroy(bombarrow, 1f);

        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isBomb", false);
    }
    void EnergyArrow()
    {
        if (Key3 && !playerST.isDodge && !PlayerST.isStun && !PlayerST.isJump && !playerST.isRun &&
            attackdamage.Usable_Skill3)
        {
            isEnergyReady = true;
            anim.SetBool("isReady", true);
            Arc3SkillBuff1.SetActive(true);

            if (Key3 && EnergyReady < EnergyChargingTime)
            {
                EnergyReady += Time.deltaTime;
                EnergyReady2 += Time.deltaTime;
            }

            if (EnergyReady2 >= 5f)
            {
                Arc3SkillBuff3.SetActive(true);
                isEnergy1 = false;
                isEnergy2 = true;
                Arc3SkillBuff2.SetActive(false);
                Arc3SkillBuff1.SetActive(false);
            }
            else if (EnergyReady > 1.5f)
            {
                isEnergy1 = true;
                Arc3SkillBuff1.SetActive(false);
                Arc3SkillBuff2.SetActive(true);
            }
        }

        else if (Key3Up)
        {
            isEnergyReady = false;
            
            anim.SetBool("isReady", false);
            anim.SetBool("isShot", true);
            if (isEnergy1)
            {
                attackdamage.Skill_3_Cool();
                GameObject intantArrow = Instantiate(Arc3SkillArrow1, Arc3SkillPos.position, Arc3SkillPos.rotation);
                Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
                arrowRigid.velocity = Arc3SkillPos.forward * 20;
                Arrow arrow = intantArrow.GetComponent<Arrow>(); //스킬데미지설정
                arrow.damage = attackdamage.Skill_3_Damamge();
                Destroy(intantArrow, 2f);
            }
            else if (isEnergy2)
            {
                attackdamage.Skill_3_Cool();
                GameObject intantArrow = Instantiate(Arc3SkillArrow2, Arc3SkillPos.position, Arc3SkillPos.rotation);
                Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
                arrowRigid.velocity = Arc3SkillPos.forward * 20;
                Arrow arrow = intantArrow.GetComponent<Arrow>(); //스킬데미지설정
                arrow.damage = 1.5f*attackdamage.Skill_3_Damamge();
                Destroy(intantArrow, 2f);
            }

            EnergyReady = 0;
            EnergyReady2 = 0;
            isEnergy1 = false;
            isEnergy2 = false;
            Invoke("EnergyAnimOut", 0.5f);
            Invoke("EnergyArrowOut", 1f);
        }

    }
    void EnergyArrowOut()
    {
        Arc3SkillBuff1.SetActive(false);
        Arc3SkillBuff2.SetActive(false);
        Arc3SkillBuff3.SetActive(false);
    }
    void EnergyAnimOut()
    {
        anim.SetBool("isShot", false);
    }
    //============================================마법사 스킬=====================================================
    void LightningBall()
    {
        if (Key1 && !playerST.isDodge && !PlayerST.isStun && !PlayerST.isJump && !playerST.isRun && !playerST.isFlash && !isIceage &&
           !isMeteo && attackdamage.Usable_Skill1)
        {
            StartCoroutine(LightningBallStart());
        }
    }

    IEnumerator LightningBallStart()
    {
        isLightning = true;
        anim.SetBool("Skill1", true);
        yield return new WaitForSeconds(1.2f);
        attackdamage.Skill_1_Cool();
        GameObject darkball1 = Instantiate(Mage1SkillEff, MagicPos.position, MagicPos.rotation);
        GameObject darkball2 = Instantiate(Mage1SkillEff, MagicPos.position, MagicPos.rotation); 
        GameObject darkball3 = Instantiate(Mage1SkillEff, MagicPos.position, MagicPos.rotation); 
        darkball1.transform.DOMove(Mage1SkillPos1.position, 1f).SetEase(Ease.Linear); 
        darkball2.transform.DOMove(Mage1SkillPos2.position, 1f).SetEase(Ease.Linear); 
        darkball3.transform.DOMove(Mage1SkillPos3.position, 1f).SetEase(Ease.Linear); 
        ArrowSkill arrow1 = darkball1.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow1.damage = attackdamage.Skill_1_Damamge();
        ArrowSkill arrow2 = darkball2.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow2.damage = attackdamage.Skill_1_Damamge();
        ArrowSkill arrow3 = darkball3.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow3.damage = attackdamage.Skill_1_Damamge();
        Destroy(darkball1, 1.1f);
        Destroy(darkball2, 1.1f);
        Destroy(darkball3, 1.1f);
        yield return new WaitForSeconds(0.5f);
        isLightning = false;
        anim.SetBool("Skill1", false);
    }
    void IceAge()
    {
        if (Key2 && !playerST.isDodge && !PlayerST.isStun && !PlayerST.isJump && !playerST.isRun && !isLightning && !playerST.isFlash &&
           !isMeteo && attackdamage.Usable_Skill2)
        {
            StartCoroutine(IceAgeStart());
        }
    }
    IEnumerator IceAgeStart()
    {
        Mage2SkillReadyEff.SetActive(true);
        anim.SetBool("Skill2", true);
        isIceage = true;
        yield return new WaitForSeconds(1.8f);
        BoxCollider Skillare = Skillarea.GetComponent<BoxCollider>(); //데미지 콜라이더 활성화
        Skillare.enabled = true;
        ArrowSkill arrow = Skillarea.GetComponent<ArrowSkill>(); //스킬데미지설정
        arrow.damage = attackdamage.Skill_2_Damamge();
        BoxCollider CCare = CCarea.GetComponent<BoxCollider>(); //cc기 콜라이더 활성화
        CCare.enabled = true;
        attackdamage.Skill_2_Cool();
        Mage2SkillReadyEff.SetActive(false);
        Mage2SkillEff.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        CCare.enabled = false;
        Skillare.enabled = false;
        yield return new WaitForSeconds(0.4f);
        anim.SetBool("Skill2", false);
        isIceage = false;
        yield return new WaitForSeconds(1f);
        Mage2SkillEff.SetActive(false);
    }
    void Meteo()
    {
        if (Key3Up||Key3 && !playerST.isDodge && !PlayerST.isStun && !PlayerST.isJump && !playerST.isRun && !playerST.isFlash
            && !isLightning && !isIceage && attackdamage.Usable_Skill3)
        {
            if (Key3 && MeteoCasting < MeteoMaxCasting)
            {
                isMeteo = true;
                Mage3SkillPlayerEff.SetActive(true);
                anim.SetBool("Skill3", true);
                anim.SetBool("Skill3Cut", false);
                Mage3SkillPosEff.SetActive(true);
                MeteoCasting += Time.deltaTime;
            }
            else if (Key3Up && !isMeteoShot)
            {
                MeteoCasting = 0f;
                isMeteo = false;
                Mage3SkillPlayerEff.SetActive(false);
                anim.SetBool("Skill3", false);
                Mage3SkillPosEff.SetActive(false);
                anim.SetBool("Skill3Cut", true);
            }
            if (MeteoCasting > MeteoMaxCasting)
            {
                isMeteoShot = true;
                anim.SetBool("Skill31", true);
                GameObject meteo = Instantiate(Mage3SkillEff, Mage3SkillPos2.position, Mage3SkillPos2.rotation);
                meteo.transform.DOMove(Mage3SkillPos1.position, 1.5f).SetEase(Ease.Linear);
                ArrowSkill arrow1 = meteo.GetComponent<ArrowSkill>(); //스킬데미지설정
                arrow1.damage = attackdamage.Skill_3_Damamge();
                Destroy(meteo, 1.6f);
                MeteoCasting = 0f;
                attackdamage.Skill_3_Cool();
                Invoke("MeteoEnd", 0.8f);
                Invoke("MeteoEnd2", 1.5f);
            }
        }
    }
    void MeteoEnd()
    {
        Mage3SkillPlayerEff.SetActive(false);
        anim.SetBool("Skill3", false);
        anim.SetBool("Skill31", false);
    }
    void MeteoEnd2()
    {
        Mage3SkillPosEff.SetActive(false);
        isMeteoShot = false;
        isMeteo = false;
    }
    //====================================================================================
    public void Use()//무기 사용
    {
        //if (type == Type.Melee) //근접무기일때 실행
        //{
        //    StopCoroutine("Swing");  //현재 공격중일시 멈춤
        //    StartCoroutine("Swing"); //공격실행
        //}
        if (type == Type.Range)
        {
            StartCoroutine("Shot");
        }
        else if (type == Type.Mage)
        {
            StartCoroutine("MagicShot");
        }
    }

    IEnumerator Swing()
    {
        

        yield return new WaitForSeconds(0.1f); // 대기
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = false;


    }

    IEnumerator Shot()
    {
        playerST.isSootReady = true;
        yield return new WaitForSeconds(0.2f); //애니메이션과 화살나가는속도와 맞추기위함
        if (!attackdamage.Duration_Buff)
        {
            GameObject intantArrow = Instantiate(ArcDefaultAttack, arrowPos.position, arrowPos.rotation);
            Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
            arrowRigid.velocity = arrowPos.forward * playerST.bowPower * 150;
            Arrow arrow = intantArrow.GetComponent<Arrow>(); //스킬데미지설정
            arrow.damage = attackdamage.Attack_Dam();
            Destroy(intantArrow, 1f);
        }
        else if (attackdamage.Duration_Buff)
        {
            GameObject intantArrow = Instantiate(Arc1Skilarrow, arrowPos.position, arrowPos.rotation);
            Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
            arrowRigid.velocity = arrowPos.forward * playerST.bowPower * 150;
            Arrow arrow = intantArrow.GetComponent<Arrow>(); //스킬데미지설정
            arrow.damage = 1.3f*attackdamage.Attack_Dam();
            Destroy(intantArrow, 1f);
        }

        playerST.anim.SetBool("doShot", false);

        playerST.bowPower = 0;

        yield return new WaitForSeconds(0.25f);

        playerST.isFireReady = true;
        yield return new WaitForSeconds(0.1f);
        playerST.archerattack = false;

    }
    IEnumerator MagicShot()
    {
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.3f); //애니메이션과 화살나가는속도와 맞추기위함
        GameObject intantArrow = Instantiate(MageDefaultAttack, MagicPos.position, MagicPos.rotation);
        Rigidbody arrowRigid = intantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = MagicPos.forward * 20;
        Arrow arrow = intantArrow.GetComponent<Arrow>(); //스킬데미지설정
        arrow.damage = attackdamage.Attack_Dam();
        Destroy(intantArrow, 0.7f);
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isAttack", false);
        yield return null;
    }
}
