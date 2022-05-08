﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerST : MonoBehaviour
{
    public enum Type { Warrior, Archer, Mage };
    public Type CharacterType; //원래 앞에 static이 붙어있었는데 테스트할때 인스펙터창에 타입이 안떠서 임시로 뻈어요
    Transform _transform;
    Rigidbody rigid;

    public float jump = 5.0f; //점프력
    public float speed = 5.0f;  //플레이어 이동속도
    public GameObject cam; //플레이어 카메라
    public CapsuleCollider SelectPlayer; //제어할 플레이어
    public Animator anim; //애니메이션
    public static float health = 0; //체력
    public float maxhealth = 100; //체력최대치
    public Weapons[] equipWeapon;    //현재 무기. 나중에 배열로 여러무기를 등록하려고함
    public int NowWeapon; //현재 무기
    public enum SwordNames { Sword1, Sword5_normal, Sword5_rare, Sword10_normal, Sword10_rare }; //무기이름 위의 배열의 순서에 따라.
    public SwordNames basicSword = 0;

    public float bowMinPower = 0.2f;
    public float bowPower; // 화살 충전 데미지
    public float bowChargingTime = 1.0f; //화살 최대 충전시간
    public bool isSootReady = true;


    float h; //X값 좌표
    float v; //Z값 좌표
    float fireDelay; //공격속도 계산용

    private bool fDown; //마우스 왼쪽버튼을 눌렀다면 true
    private bool fDowning; //마우스 왼쪽버튼을 눌르고 있다면 true
    private bool fUp;
    private bool f2Down; //마우스 우클 눌렀다면
    public bool isFireReady = true;  //무기 rate가 fireDelay보다 작다면 true로 공격가능상태
    public bool isDamage; //무적타임. 연속으로 다다닥 맞을수있기때문에
    private bool sDown; //점프입력
    private bool Rdown;//알트입력
    private bool Ddown; //쉬프트입력 
    private bool Key1; //키보드 1번입력
    private bool Key2; //키보드 2번입력
    private bool Key3; //키보드 3번입력

    public static bool isJump; //현재 점프중?
    public bool archerattack = false; //현재 궁수공격중
    public static bool isStun; //현재 스턴상태
    public bool isRun; //현재 달리는상태?

    Vector3 moveVec;
    Vector3 dodgeVec;

    public Weapons weapons;



    public static bool isFall; //공중에 떠있는상태? 몬스터들의 룩엣을 조정하기위함.
    //======================전사 스킬========================//

    private float DodgeTimePrev = 0f; //구르기 시간체크용  
    private float BlockTimePrev = 0f; //방패치기 시간체크용
    private float BuffTimePrev = 0f; //폭주스킬 시간체크용
    private float RushTimePrev = 0f; //돌진스킬 시간체크용
    private float AuraTimePrev = 0f; //검기스킬 시간체크용

    private float dodgecool = 3f; //구르기 쿨타임
    private float blockcool = 10f; //방패치기 쿨타임
    private float buffcool = 0f; //폭주 쿨타임
    private float rushcool = 0f; //돌진 쿨타임
    private float auracool = 0f; //검기 쿨타임

    public bool isDodge; //현재 회피중?
    private bool isBlock; //현재 방패치기중? ㅣ마우스 우클릭
    public static bool isBuff; //현재 폭주상태? ㅣ키보드 1번
    private bool isRush; //현재 돌진상태? ㅣ키보드 2번
    private bool isAura; //현재 검기날리는 상태? ㅣ 키보드 3번
    private bool isYes; //돌진 벽에서 쓰는행위 막는용도

    [Header("전사 관련")]
    public GameObject BuffEff;
    public GameObject RushEff;
    public Transform Aurapos;
    public GameObject SwordAura; //검기스킬 투사체

    //======================궁수 스킬========================// 
    [Header("궁수 관련")]
    public GameObject BackStepEff;
    public Transform BackStepPos;

    private float BackStepTimePrev = 0f; //백스텝 시간체크용  
    private float backstepcool = 0f; //백스텝 쿨타임
    private float PoisonArrowTimePrev = 0f; //독화살 시간체크용  
    private float poisonarrowcool = 0f; //독화살 쿨타임

    public bool isBackStep; //현재 백스텝상태?
    public static bool isPoison; //현재 독화살버프상태?  나중에 몬스터 충돌판정에서 if걸고 추가데미지를 줄예정

    //======================마법사 스킬========================// 
    [Header("마법사 관련")]
    public MeshRenderer mesh;           //순간이동때 투명화
    public SkinnedMeshRenderer smesh;   //순간이동때 투명화
    public GameObject FlashEff;  //순간이동이펙트

    private float FlashTimePrev = 0f;  //순간이동 쿨체크
    private float flashcool = 0f;      //순간이동 쿨타임
    public bool isFlash; //현재 순간이동중?


    void Start()
    {
        DodgeTimePrev = Time.time; //구르기용
        BlockTimePrev = Time.time; //전사방패치기용
        BuffTimePrev = Time.time; //전사폭주용
        RushTimePrev = Time.time; //전사 돌진용
        AuraTimePrev = Time.time; //전사 검기용
        BackStepTimePrev = Time.time; //궁수 백스텝용
        PoisonArrowTimePrev = Time.time; //궁수 독화살용
        FlashTimePrev = Time.time; //마법사 순간이동용

        health = maxhealth;
        bowPower = bowMinPower;
        _transform = GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Anima() //애니메이션 
    {
        anim.SetBool("isRun", Rdown);
        if (v >= 0.1f) //앞
        {
            anim.SetBool("forword", true);
            anim.SetBool("back", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }
        else if (v <= -0.1f) //뒤
        {
            anim.SetBool("back", true);
            anim.SetBool("forword", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }
        else if (h >= 0.1f) //오른쪽
        {
            anim.SetBool("right", true);
            anim.SetBool("forword", false);
            anim.SetBool("back", false);
            anim.SetBool("left", false);
        }
        else if (h <= -0.1f) //왼쪽
        {
            anim.SetBool("left", true);
            anim.SetBool("forword", false);
            anim.SetBool("back", false);
            anim.SetBool("right", false);
        }
        else
        {
            anim.SetBool("forword", false);
            anim.SetBool("back", false);
            anim.SetBool("left", false);
            anim.SetBool("right", false);
        }
    }

    void Attack()   //공격
    {
        if ((CharacterType == Type.Mage || CharacterType == Type.Warrior) && !isDodge && !isFlash && !weapons.isLightning &&
           !weapons.isIceage && !Weapons.isMeteo && !isJump && !isRun && !isBlock && !isRush && !isAura && !isStun)
        {
            fireDelay += Time.deltaTime;     //공격속도 계산
            isFireReady = equipWeapon[NowWeapon].rate < fireDelay;  //공격 가능 타임

            if (fDown)
            {
                if (isFireReady)  //공격할수있을때
                {
                    equipWeapon[NowWeapon].Use();
                    fireDelay = 0;
                }
            }
        }

        else if (CharacterType == Type.Archer && !isDodge && !isJump && !isRun && !isStun && !isBackStep && !weapons.isEnergyReady)
        {
            fireDelay += Time.deltaTime;

            if (fDowning && bowPower < bowChargingTime)
            {

                bowPower += Time.deltaTime;
            }

            if (fDowning && isFireReady && equipWeapon[NowWeapon].rate < fireDelay)
            {
                archerattack = true;
                bowPower = bowMinPower;
                anim.SetTrigger("doSwing");
                isSootReady = false;
                isFireReady = false;
                fireDelay = 0f;
            }
            else if (fUp && !isSootReady)
            {
                archerattack = true;
                anim.SetBool("doShot", true);

                equipWeapon[NowWeapon].Use();
            }
        }
    }
    void Jump()
    {
        if (sDown && !isJump && !isDodge && !isBlock && !isRush && !isAura && !isBackStep && !weapons.isEnergyReady &&
            !weapons.isLightning && !weapons.isIceage && !Weapons.isMeteo && !isFlash && !isStun
            )
        {
            rigid.AddForce(Vector3.up * jump, ForceMode.Impulse); //애드포스 : 힘을주다/ 포스모드,임펄스 : 즉발적
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Dodge()
    {
        if (Ddown && !isStun &&!isJump && !isBlock && !isBackStep && !weapons.isEnergyReady && !isRush && !isAura && !isFlash &&
           !weapons.isLightning && !weapons.isIceage && !Weapons.isMeteo && Time.time - DodgeTimePrev > dodgecool)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;
            isDamage = true;
            DodgeTimePrev = Time.time;

            Invoke("DodgeOut", 0.4f); //구르기를 하면 0.4초후에 이동속도가 정상으로돌아옴
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
        isDamage = false;
    }

    //==================================여기서부터 전사스킬=======================================
    void Block() //방패 치기
    {
        if (f2Down && !isRush && !isAura && !isJump && !isDodge && !isStun && !isRun &&
            Time.time - BlockTimePrev > blockcool)
        {
            anim.SetBool("isBlock", true);
            isBlock = true;
            isDamage = true;
            BlockTimePrev = Time.time;

            Invoke("BlockOut", 1f);
        }
    }
    void BlockOut()
    {
        anim.SetBool("isBlock", false);
        isBlock = false;
        isDamage = false;
    }
    void Buff()
    {
        if (Key1 && !isRush && !isAura && !isJump && !isDodge && !isBlock && !isStun && !isRun &&
            Time.time - BuffTimePrev > buffcool)
        {
            isBuff = true;
            BuffEff.SetActive(true);
            Invoke("BuffOut", 6f);
        }
    }
    void BuffOut()
    {
        BuffTimePrev = Time.time;
        isBuff = false;
        BuffEff.SetActive(false);
    }
    void Rush()
    {
        if (Key2 && !isYes && !isJump && !isDodge && !isBlock && !isAura && !isStun && !isRun &&
            Time.time - RushTimePrev > rushcool)
        {
            StartCoroutine(RushPlay());
        }
    }
    IEnumerator RushPlay()
    {
        isRush = true;
        isFall = true;
        anim.SetBool("isRush", true);
        yield return new WaitForSeconds(0.5f);
        rigid.AddForce(transform.forward * 40 + transform.up * 20, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);

        RushEff.SetActive(true);
        equipWeapon[NowWeapon].Use();
        yield return new WaitForSeconds(0.2f);

        anim.SetBool("isRush", false);
        RushTimePrev = Time.time;
        isRush = false;
        isFall = false;
        yield return new WaitForSeconds(0.5f);
        RushEff.SetActive(false);

    }
    void Aura()
    {
        if (Key3 && !isRun && !isJump && !isDodge && !isBlock && !isRush && !isStun && 
            Time.time - AuraTimePrev > auracool)
        {
            StartCoroutine(AuraPlay());
        }
    }
    IEnumerator AuraPlay()
    {
        isAura = true;
        AuraTimePrev = Time.time;
        anim.SetBool("isAura", true);
        yield return new WaitForSeconds(0.7f);

        GameObject swordaura = Instantiate(SwordAura, Aurapos.position, Aurapos.rotation);
        Rigidbody aurarigid = swordaura.GetComponent<Rigidbody>();
        aurarigid.velocity = Aurapos.forward * 20;
        Destroy(swordaura, 1f);

        yield return new WaitForSeconds(0.8f);
        isAura = false;
        anim.SetBool("isAura", false);
    }
    //==================================여기서부터 궁수스킬=======================================
    void Smoke() //마우스 우클릭 연막
    {
        if (f2Down && !isJump && !isDodge && !isStun && !isRun && Time.time - BackStepTimePrev > backstepcool)
        {
            gameObject.layer = LayerMask.NameToLayer("Back");
            isFall = true;
            isBackStep = true;
            rigid.AddForce(transform.forward * -23 + transform.up * 10, ForceMode.Impulse);
            GameObject arceff = Instantiate(BackStepEff, BackStepPos.position, BackStepPos.rotation);
            Destroy(arceff, 2f);
            anim.SetBool("isSmoke", true);
            Invoke("SmokePlay", 0.3f);
        }
    }
    void SmokePlay()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        BackStepTimePrev = Time.time;
        anim.SetBool("isSmoke", false);
        isBackStep = false;
        isFall = false;
    }
    void PoisonArrow()
    {
        if (Key1 && !isStun && !isRun && Time.time - PoisonArrowTimePrev > poisonarrowcool)
        {
            isPoison = true;
            Invoke("PAout", 10f);
        }
    }
    void PAout()
    {
        isPoison = false;
        PoisonArrowTimePrev = Time.time;
    }
    //==================================여기서부터 마법사스킬=======================================
    void Flash()
    {
        if(f2Down && !isDodge && !isJump && !isRun && !isStun && !weapons.isLightning && !weapons.isIceage && !Weapons.isMeteo &&
            Time.time - FlashTimePrev > flashcool)
        {
            StartCoroutine(FlashStart());  
        }
    }
    IEnumerator FlashStart()
    {
        gameObject.layer = LayerMask.NameToLayer("Back");
        isFlash = true;
        isFall = true;
        mesh.enabled = false;
        smesh.enabled = false;
        FlashEff.SetActive(true);
        rigid.AddForce(transform.forward * 40 + transform.up * 10, ForceMode.Impulse);
        yield return new WaitForSeconds(0.3f);
        gameObject.layer = LayerMask.NameToLayer("Player");
        isFlash = false;
        isFall = false;
        FlashEff.SetActive(false);
        mesh.enabled = true;
        smesh.enabled = true;
        FlashTimePrev = Time.time;
    }
    void InputManager()
    {
        h = Input.GetAxisRaw("Horizontal");    //X좌표 입력받기
        v = Input.GetAxisRaw("Vertical"); //Z좌표 입력받기
        fDown = Input.GetButtonDown("Fire1"); //마우스1번키 입력
        fDowning = Input.GetButton("Fire1");
        fUp = Input.GetButtonUp("Fire1");
        f2Down = Input.GetButtonDown("Fire2"); //마우스 2번키입력
        sDown = Input.GetButtonDown("Jump"); //점프사용 스페이스바
        Rdown = Input.GetButton("Run"); //달리기  알트키 
        Ddown = Input.GetButtonDown("Dodge"); //구르기 쉬프트키
        Key1 = Input.GetButtonDown("Key1"); //1번키
        Key2 = Input.GetButtonDown("Key2"); //2번키
        Key3 = Input.GetButtonDown("Key3"); //3번키
    }
    void Update()
    {
        if (inventory.iDown)
            return;

        Cursor.lockState = CursorLockMode.Locked;//마우스커서 고정
        if (isBuff && CharacterType == Type.Warrior)  //전사 폭주상태면 공격 애니메이션 속도증가
        {
            anim.SetFloat("Attack1Speed", 1.5f);
            anim.SetFloat("Attack2Speed", 1.5f);
            anim.SetFloat("Attack3Speed", 1.5f);
        }
        else if (!isBuff && CharacterType == Type.Warrior)
        {
            anim.SetFloat("Attack1Speed", 1f);
            anim.SetFloat("Attack2Speed", 1f);
            anim.SetFloat("Attack3Speed", 1f);
        }

        if (fDowning && CharacterType == Type.Archer) //화살땡길때 이속감소
            speed = 2.5f;
        else if (!fDowning && CharacterType == Type.Archer)
            speed = 6f;

        //Debug.Log("shield");
        InputManager(); //입력매니저
        Anima(); //애니메이션
        Attack(); //근접 공격
        Jump(); //점프
        Dodge(); //구르기
        WarriorMove(); //전사이동제한
        ArcherMove(); //궁수이동제한
        MageMove(); //마법사이동제한
        SkillClass(); //스킬직업제한
    }
    void SkillClass()
    {
        if (CharacterType == Type.Warrior)
        {
            Block(); //방패치기
            Buff(); //폭주스킬
            Rush(); //돌진스킬
            Aura(); //검기스킬
        }
        else if (CharacterType == Type.Archer)
        {
            Smoke(); //섬광탄백스텝
            PoisonArrow(); //독화살버프
        }
        else if(CharacterType == Type.Mage)
        {
            Flash(); //점멸
        }
    }
    void MageMove()
    {
        if (!isStun && !weapons.isLightning && !weapons.isIceage && !Weapons.isMeteo
            && !weapons.isEnergyReady && CharacterType == Type.Mage)
        {
            if (isDodge)
                moveVec = dodgeVec;
            moveVec = (Vector3.forward * v) + (Vector3.right * h);
            if (Rdown)
            {
                isRun = true;
                _transform.Translate(moveVec.normalized * Time.deltaTime * speed * 1.4f, Space.Self);
            }
            else
            {
                isRun = false;
                _transform.Translate(moveVec.normalized * Time.deltaTime * speed, Space.Self);
            }
        }
    }
    void ArcherMove()
    {
        if (!isStun && !isBackStep && !weapons.isEnergyReady && CharacterType == Type.Archer)
        {
            if (isDodge)
                moveVec = dodgeVec;
            moveVec = (Vector3.forward * v) + (Vector3.right * h);
            if (Rdown)
            {
                isRun = true;
                _transform.Translate(moveVec.normalized * Time.deltaTime * speed * 1.4f, Space.Self);
            }
            else
            {
                isRun = false;
                _transform.Translate(moveVec.normalized * Time.deltaTime * speed, Space.Self);
            }
        }
    }
    void WarriorMove()
    {
        if (!isStun && !isBlock && !isRush && !isAura && !isBackStep && CharacterType == Type.Warrior)
        {
            if (isDodge) //회피중이면 다른방향으로 전환이 느리게
                moveVec = dodgeVec;
            moveVec = (Vector3.forward * v) + (Vector3.right * h); //전 후진과 좌우 이동값 저장
            if (Rdown)//달리는 중이면 1.4배 이속증가
            {
                isRun = true;
                _transform.Translate(moveVec.normalized * Time.deltaTime * speed * 1.4f, Space.Self);//이동 처리를 편하게 하게해줌
            }
            else
            {
                isRun = false;
                _transform.Translate(moveVec.normalized * Time.deltaTime * speed, Space.Self);
            }
        }
    }
    void FreezeVelocity()  //카메라 버그 안생기게하는거
    {
        if (!isJump)
        {

            rigid.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {

        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other) //충돌감지
    {
        if (other.gameObject.tag == "EnemyRange")  //적에게 맞았다면
        {
            if (!isDamage) //무적타이밍이 아닐때만 실행
            {
                Debug.Log("아야!");
                EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                health -= enemyRange.damage;
                StartCoroutine(OnDamage());

            }
        }
        else if (other.gameObject.tag == "Boss1Skill")  //1보스 스턴스킬
        {
            if (!isDamage) //무적타이밍이 아닐때만 실행
            {
                EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                health -= enemyRange.damage;

                StartCoroutine(OnDamageNuck());
            }
        }
        else if (other.gameObject.tag == "Boss2Skill")  //최종보스 1.5초스턴스킬
        {
            if (!isDamage) //무적타이밍이 아닐때만 실행
            {
                EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                health -= enemyRange.damage;

                StartCoroutine(OnDamageNuck2());
            }

        }
        else if (other.gameObject.tag == "Boss3Skill")  //최종보스 5초스턴스킬
        {
            if (!isDamage) //무적타이밍이 아닐때만 실행
            {
                EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                health -= enemyRange.damage;

                StartCoroutine(OnDamageNuck3());
            }

        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            rigid.velocity = Vector3.zero;
            anim.SetBool("isJump", false);
            isJump = false;
        }
        if (collision.gameObject.tag == "WALL")
        {
            isYes = true; //벽쪽에서 돌진스킬못쓰게
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "WALL")
        {
            isYes = false;
        }
    }



    IEnumerator OnDamage() //무적타임
    {
        isDamage = true; //무적타임 true

        yield return new WaitForSeconds(1f);

        isDamage = false;

    }
    IEnumerator OnDamageNuck() //무적타임
    {
        anim.SetBool("isStun", true);
        isStun = true;

        yield return new WaitForSeconds(3f);
        anim.SetBool("isStun", false);
        isStun = false;

    }
    IEnumerator OnDamageNuck2() //무적타임
    {
        anim.SetBool("isStun", true);
        isStun = true;

        yield return new WaitForSeconds(1.5f);
        anim.SetBool("isStun", false);
        isStun = false;

    }
    IEnumerator OnDamageNuck3() //무적타임
    {
        anim.SetBool("isStun", true);
        isStun = true;

        yield return new WaitForSeconds(5f);
        anim.SetBool("isStun", false);
        isStun = false;

    }

    public void WeaponChange(SwordNames WeaponNum) //무기를 바꿨을때 캐릭터에 적용시키기 위해 사용하는 함수
    {
        equipWeapon[NowWeapon].gameObject.SetActive(false);
        NowWeapon = (int)WeaponNum;
        equipWeapon[NowWeapon].gameObject.SetActive(true);

    }

}
