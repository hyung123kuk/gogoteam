using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class itemBuyQuestion : MonoBehaviour , IPointerClickHandler
{
    public static bool isBuyQuestion = false;
    public GameObject itemBuyWindow;
    public Text itemBuyText;
    public GameObject PortionBuyWindow;
    public InputField PortionNum;
    public Text PortionBuyText;

    public Item item;
    public inventory inven;
    [SerializeField]
    private AllUI allUI;
    [SerializeField]
    private PlayerStat playerStat;

    bool isPortionWindow = false;
    // Start is called before the first frame update
    void Start()
    {
        inven = FindObjectOfType<inventory>();
        allUI = FindObjectOfType<AllUI>();
        playerStat = FindObjectOfType<PlayerStat>();
    }

    

    // Update is called once per frame
    void Update()
    {

        if (isBuyQuestion&&Input.GetKeyDown(KeyCode.Return) ){
            BuyItem();
        }

        if (isPortionWindow)
        {
            if (PortionNum.text != "")
            {
                if (int.Parse(PortionNum.text) * (int)item._PRICE > (int)playerStat.MONEY)
                {

                    Debug.Log((int)playerStat.MONEY / (int)item._PRICE);
                    PortionNum.text = ((int)playerStat.MONEY / (int)item._PRICE).ToString();
                }
                else if (int.Parse(PortionNum.text) < 0)
                {
                    PortionNum.text = 0.ToString();
                }
            }
        }

    }
   
    public void BuyQuestionOn(Item _BuyItem)
    {
        if (_BuyItem.itemType == Item.ItemType.Used)
        {
            isPortionWindow = true;
            PortionBuyWindow.SetActive(true);
            PortionBuyText.text = "<color=#9ACD32>" + _BuyItem.itemName + "</color>" + " ���� ���� : ";
        }
        else
        {
            itemBuyText.text = "<color=#9ACD32>" + _BuyItem.itemName + "</color>" + "�� ���� �Ͻðڽ��ϱ�?";
            itemBuyWindow.SetActive(true);
        }
        item = _BuyItem;
        isBuyQuestion = true;
        allUI.ItemBuyTop();
    }

    public void BuyQuestionOff()
    {
        PortionBuyWindow.SetActive(false);
        
        itemBuyWindow.SetActive(false);
        isBuyQuestion = false;
        isPortionWindow = false;
    }

    public void BuyItem()
    {
        isBuyQuestion = false;
        if (item.itemType == Item.ItemType.Equipment)
        {
            if (!inven.HasEmptySlot())
            {
                Debug.Log("��â�� �����ϴ�.");
                BuyQuestionOff();
                return;
            }
        }
        else if(item.itemType == Item.ItemType.Used){
            if (!inven.HasEmptySlot()&& !inven.HasSameSlot(item))
            {
                Debug.Log("��â�� �����ϴ�.");
                BuyQuestionOff();
                return;
            }
        }
        if(int.Parse(PortionNum.text)!=0)
            inven.BuyItem(item, int.Parse(PortionNum.text));
        if (item.itemType == Item.ItemType.Equipment)
            inven.BuyItem(item, 1);
        BuyQuestionOff();

     }

    public void OnPointerClick(PointerEventData eventData)
    {
        allUI.ItemBuyTop();
    }
}
