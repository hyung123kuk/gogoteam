using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySlime : MonoBehaviour
{

    public int maxHealth; //�ִ� ü��
    public int curHealth; //���� ü��
    public BoxCollider meleeArea; //���� ���ݹ���
    public bool isChase; //�������� ����
    public bool isAttack; //���� ������
    public Transform respawn;

    Transform target;
    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat; //�ǰݽ� �����ϰ�
    NavMeshAgent nav; //����
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim=GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        Targerting();
        if (Vector3.Distance(target.position,transform.position)<=15f && nav.enabled)
        {
            if (!isAttack)
            {
                isChase = true;
                nav.isStopped = false;
                nav.SetDestination(target.position);
                anim.SetBool("isWalk", true);
            }
        }
        else if(Vector3.Distance(target.position,transform.position) > 15f && nav.enabled)
        {
            nav.SetDestination(respawn.position);
            isChase = false;
            if (Vector3.Distance(respawn.position,transform.position)<1f)
            {
                nav.isStopped = true;
                anim.SetBool("isWalk", false);
            }
     
        }

        if (isChase || isAttack) //�����̳� �������϶���
            transform.LookAt(target); //�÷��̾� �ٶ󺸱�
    }

    void FreezeVelocity() //�̵�����
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }
    void Targerting()//Ÿ����
    {
        float targetRadius = 1.5f; 
        float targetRange = 1.0f; 

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
            targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));  //����ĳ��Ʈ

        if(rayHits.Length>0 && !isAttack) //����ĳ��Ʈ�� �÷��̾ �����ٸ� && ���� �������� �ƴ϶��
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack() //������ �ϰ� �������ϰ� �ٽ� ������ ����
    {
        isChase = false;
        isAttack = true;
        
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.7f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.8f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.5f);
        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }
    void FixedUpdate()
    {
        
        FreezeVelocity();
    }

    void OnTriggerEnter(Collider other)  //�ǰ�
    {
       if(other.tag == "Melee")
        {
            Weapons weapon = other.GetComponent<Weapons>();
            curHealth -= weapon.damage;
           
            StartCoroutine(OnDamage());
            
        }
       else if(other.tag=="Arrow")
        {
            Arrow arrow = other.GetComponent<Arrow>();
            curHealth -= arrow.damage;
            
            StartCoroutine(OnDamage());
        }
    }

    IEnumerator OnDamage() 
    {
        mat.color = Color.red;
        
        yield return new WaitForSeconds(0.1f);

        if(curHealth>0)
        {
            mat.color = Color.white;
        }
        else
        {
            mat.color = Color.black;
            isChase = false; //�׾����� ��������
            anim.SetTrigger("doDie");
            Destroy(gameObject, 2f);
        }
    }
}
