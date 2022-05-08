using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerST : MonoBehaviour
{
    public enum Type { Warrior, Archer, Mage };
    public Type CharacterType; //원래 앞에 static이 붙어있었는데 테스트할때 인스펙터창에 타입이 안떠서 임시로 뻈어요
    Transform _transform;
    Rigidbody rigid;
    private float TimePrev = 0f;

    public float jump = 5.0f; //점프력
    public float speed = 5.0f;  //플레이어 이동속도
    public GameObject cam; //플레이어 카메라
    public CapsuleCollider SelectPlayer; //제어할 플레이어
    public Animator anim; //애니메이션
    public static int health=0; //체력
    public int maxhealth=100; //체력최대치
    public Weapons[] equipWeapon;    //현재 무기. 나중에 배열로 여러무기를 등록하려고함
    public int NowWeapon; //현재 무기
    public  enum SwordNames {Sword1, Sword5_normal, Sword5_rare, Sword10_normal, Sword10_rare, None }; //무기이름 위의 배열의 순서에 따라.
    public SwordNames basicSword=0 ;

    public float bowMinPower = 0.2f;  
    public float bowPower; // 화살 충전 데미지
    public float bowChargingTime = 1.0f; //화살 최대 충전시간
    public bool isSootReady= true;
    

    float h; //X값 좌표
    float v; //Z값 좌표
    float fireDelay; //공격속도 계산용

    bool fDown; //마우스 왼쪽버튼을 눌렀다면 true
    bool fDowning; //마우스 왼쪽버튼을 눌르고 있다면 true
    bool fUp;
    public bool isFireReady=true;  //무기 rate가 fireDelay보다 작다면 true로 공격가능상태
    public bool isDamage; //무적타임. 연속으로 다다닥 맞을수있기때문에
    bool sDown; //점프입력
    bool Rdown;//알트입력
    bool Ddown; //쉬프트입력
    bool isJump; //현재 점프중?
    bool isDodge; //현재 회피중?
    float dodgecool = 3f;
    public bool archerattack=false; //현재 궁수공격중
    bool isStun; //현재 스턴상태

    Vector3 moveVec;
    Vector3 dodgeVec;


    void Awake()
    {
        bowPower = bowMinPower;
        _transform = GetComponent<Transform>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        TimePrev = Time.time; //현재 시간을 대입
        health = maxhealth;
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
        if (CharacterType == Type.Warrior)
        {
            fireDelay += Time.deltaTime;     //공격속도 계산
            isFireReady = equipWeapon[NowWeapon].rate < fireDelay;  //공격 가능 타임

            if (fDown || fDowning)
            {
                if (isFireReady && !isDodge)  //공격할수있을때
                {
                    equipWeapon[NowWeapon].Use();
                    fireDelay = 0;
                }
            }
        }

        else if (CharacterType == Type.Archer)
        {
            fireDelay += Time.deltaTime;

            if (fDowning && bowPower < bowChargingTime)
            {
                
                bowPower += Time.deltaTime;
            }

            if (fDowning  && isFireReady&& !isDodge && equipWeapon[NowWeapon].rate < fireDelay)
            {
                archerattack = true;
                bowPower = bowMinPower;
                anim.SetTrigger("doSwing");
                isSootReady = false;
                isFireReady = false;
                fireDelay = 0f;
            }
            else if (fUp&& !isSootReady )
            {
                archerattack = true;
                anim.SetBool("doShot", true);

                equipWeapon[NowWeapon].Use();
                
                
             }



        }
    }
    void Jump()
    {
        if (sDown && !isJump && !isDodge)
        {
            rigid.AddForce(Vector3.up * jump,ForceMode.Impulse); //애드포스 : 힘을주다/ 포스모드,임펄스 : 즉발적
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Dodge()
    {
        if (Ddown && !isJump && Time.time - TimePrev > dodgecool)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;
            isDamage = true;
            TimePrev = Time.time;

            Invoke("DodgeOut", 0.4f); //구르기를 하면 0.4초후에 이동속도가 정상으로돌아옴
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
        isDamage = false;
    }

    void Update()
    {

        if (inventory.iDown)
            return;
        

            h = Input.GetAxisRaw("Horizontal");    //X좌표 입력받기
            v = Input.GetAxisRaw("Vertical"); //Z좌표 입력받기
            fDown = Input.GetButtonDown("Fire1"); //마우스1번키 입력
            fDowning = Input.GetButton("Fire1");
            fUp = Input.GetButtonUp("Fire1");
            sDown = Input.GetButtonDown("Jump"); //점프사용 스페이스바
            Rdown = Input.GetButton("Run"); //달리기  알트키 
            Ddown = Input.GetButtonDown("Dodge"); //구르기 쉬프트키

            if (!isStun)
            {
                if (archerattack == false)
                {
                    if (isDodge) //회피중이면 다른방향으로 전환이 느리게
                        moveVec = dodgeVec;
                    moveVec = (Vector3.forward * v) + (Vector3.right * h); //전 후진과 좌우 이동값 저장
                    if (Rdown)//달리는 중이면 1.4배 이속증가
                        _transform.Translate(moveVec.normalized * Time.deltaTime * speed * 1.4f, Space.Self); //이동 처리를 편하게 하게해줌
                    else
                        _transform.Translate(moveVec.normalized * Time.deltaTime * speed, Space.Self);
                }
            }

            Anima(); //애니메이션
        if (!NPC.isNPCRange)
        {
            Attack(); //근접 공격
        }
            Jump(); //점프
            Dodge(); //구르기
        
            
        
    }
    void FreezeVelocity()  //카메라 버그 안생기게하는거
    {
        if (!isJump)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other) //충돌감지
    {
        if(other.gameObject.tag == "EnemyRange")  //적에게 맞았다면
        {
            if (!isDamage) //무적타이밍이 아닐때만 실행
            {
                EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
                health -= enemyRange.damage;
                StartCoroutine(OnDamage());
               
            }
        }
        else if (other.gameObject.tag == "Boss1Skill")  //1보스 스턴스킬
        {
            EnemyAttack enemyRange = other.GetComponent<EnemyAttack>();
            health -= enemyRange.damage;
            
            StartCoroutine(OnDamageNuck());
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            anim.SetBool("isJump", false);
            isJump = false;
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
        anim.SetTrigger("doStun");
        isStun = true;
        
        yield return new WaitForSeconds(3f);
        isStun = false;

    }

    public void WeaponChange( SwordNames WeaponNum ) //무기를 바꿨을때 캐릭터에 적용시키기 위해 사용하는 함수
    {
        equipWeapon[NowWeapon].gameObject.SetActive(false);
        NowWeapon =(int) WeaponNum;
        equipWeapon[NowWeapon].gameObject.SetActive(true);

    }

}
