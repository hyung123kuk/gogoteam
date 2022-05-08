using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private SphereCollider NPCRange;
    [SerializeField]
    public static bool isNPCRange=false;
    [SerializeField]
    private AllUI allUI;
    [SerializeField]
    private PlayerST playerST;
    [SerializeField]
    private Text NPCRangeText;

    public Camera Camera; // ī�޶� ����
    public RaycastHit hit; //�� �Ѱ� ����� �־�Ѱ�

  

    public void SetNPC()
    {
        NPCRange = GetComponent<SphereCollider>();
        allUI = FindObjectOfType<AllUI>();
        playerST = FindObjectOfType<PlayerST>();
        Camera = playerST.GetComponentInChildren<Camera>();

    }
   
    private void OnTriggerStay(Collider other)
    {
        Cursor.lockState = CursorLockMode.Confined;
        allUI.MouseCursor.transform_cursor.gameObject.SetActive(true);
        
        if (other.tag == "Player")
        {
            isNPCRange = true;
            
            NPCtext();
                
        }
        if(other.tag == "Arrow")
        {
            Destroy(other.gameObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Player")
        {
            isNPCRange = false;
            NPCRangeText.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            allUI.MouseCursor.transform_cursor.gameObject.SetActive(false);
        }
    }


    void NPCtext()
    {
        NPCRangeText.enabled = true;

        if (NPCRange.gameObject.tag == "SHOPIA")
        {
            NPCRangeText.text = "���ǾƸ� Ŭ���Ͻø� ������ ������ ���ϴ�.";
        }
        else if (NPCRange.gameObject.tag == "PETER")
        {
            NPCRangeText.text = "���͸� Ŭ���Ͻø� ��ų ������ ���ϴ�.";
        }
    }
 
    
}
