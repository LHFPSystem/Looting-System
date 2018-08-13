using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootingSystem : MonoBehaviour {

    public Transform LootingPanel;
    private int counter = 0;
    public Text MoneyShower;
    public Text PlayerMoney;
    public bool Called = false;
    GeneralEnemyController CurrentEnemy;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        CloseLootingPanel();
        TakeAll(CurrentEnemy);
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Dead")
        {
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Called == false)
                {
                    Debug.Log("You pressed E");
                    UIManager UI = FindObjectOfType<UIManager>();                    
                    Called = true;
                    foreach (Transform child in UI.CanvasItself)
                    {
                        if (child.tag == "LootingPanel")
                        {
                            counter += 1;
                        }
                        if (counter >= 2)
                        {
                            GameObject.Destroy(child.gameObject);
                            counter -= 1;
                        }
                    }
                    if (LootingPanel.childCount > 2)
                    {
                        UI.ChildKiller(LootingPanel);
                    }
                    Transform NewPanel = Instantiate(LootingPanel);
                    NewPanel.transform.SetParent(UI.CanvasItself, false);
                    NewPanel.gameObject.SetActive(true);
                    PlayerSkiller PlayerReference = FindObjectOfType<PlayerSkiller>();
                    InventoryDisplay inventory = Instantiate(UI.InventoryDisplayPrefab);
                    inventory.transform.SetParent(NewPanel, false);
                    inventory.GetComponent<RectTransform>().localScale = new Vector2(0.4f, 0.48f);
                    inventory.GetComponent<RectTransform>().sizeDelta = new Vector2(0.001f, 100);
                    inventory.transform.localPosition = new Vector2(-80, 33);
                    inventory.Cuarto(PlayerReference.items);
                    Text PlayersMoney = Instantiate(PlayerMoney);
                    PlayersMoney.tag = "PlayerMoney";
                    PlayersMoney.transform.SetParent(NewPanel, false);
                    PlayersMoney.transform.localPosition = new Vector2(-30, -55);
                    PlayersMoney.GetComponent<RectTransform>().localScale = new Vector2(0.1f, 0.1f);
                    PlayersMoney.text = "Money: " + PlayerReference.Wallet.ToString();                    
                    InventoryDisplay EnemyInventory = Instantiate(UI.InventoryDisplayPrefab);
                    EnemyInventory.transform.SetParent(NewPanel, false);
                    EnemyInventory.transform.localPosition = new Vector2(80, 33);
                    EnemyInventory.GetComponent<RectTransform>().localScale = new Vector2(0.4f, 0.48f);
                    EnemyInventory.GetComponent<RectTransform>().sizeDelta = new Vector2(0.001f, 100);                    
                    Text MoneyText = Instantiate(MoneyShower);
                    MoneyText.tag = "MonsterMoney";
                    MoneyText.transform.SetParent(NewPanel, false);
                    MoneyText.transform.localPosition = new Vector2(150, -40);
                    MoneyText.GetComponent<RectTransform>().localScale = new Vector2(0.1f, 0.1f);
                    GeneralEnemyController EnemyWallet = other.GetComponent<GeneralEnemyController>();
                    MoneyText.text = "Money: " + EnemyWallet.EnemyWallet.ToString();
                    CurrentEnemy = other.GetComponent<GeneralEnemyController>();
                    //EnemyInventory.Cuarto(CurrentEnemy.EnemyList);

                }              
            }
        }
    }

    public void TakeAll(GeneralEnemyController Enemy)
    {
        if(Called == true)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("You Took Everything mate");
                Text Texterer = GetTheMonsterText();
                Text PlayerMoney = GetThePlayerText();
                PlayerSkiller PlayerReferenced = FindObjectOfType<PlayerSkiller>();
                PlayerReferenced.Wallet += Enemy.EnemyWallet;                
                Enemy.EnemyWallet = 0;
                Texterer.text = "Money: " + Enemy.EnemyWallet.ToString();
                PlayerMoney.text = "Money: " + PlayerReferenced.Wallet.ToString();
                InventoryDisplay Functioner = FindObjectOfType<InventoryDisplay>();
                Functioner.Cuarto(Enemy.EnemyList);
            }
            
        }
        
    }

    public void CloseLootingPanel()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            UIManager UI = FindObjectOfType<UIManager>();
            foreach (Transform child in UI.CanvasItself)
            {
                if (child.tag == "LootingPanel")
                {
                    child.gameObject.SetActive(false);
                    Called = false;
                }
            }

        }
    }

    public Text GetTheMonsterText()
    {
        UIManager UI = FindObjectOfType<UIManager>();
        foreach (Transform child in UI.CanvasItself)
        {
            if (child.tag == "LootingPanel")
            {
               foreach(Transform child2 in child)
                {
                     if(child2.tag == "MonsterMoney")
                    {
                        Text finaler = child2.GetComponent<Text>();
                        return finaler;
                    }
                }
            }
        }
        return null;
    }
    public Text GetThePlayerText()
    {
        UIManager UI = FindObjectOfType<UIManager>();
        foreach (Transform child in UI.CanvasItself)
        {
            if (child.tag == "LootingPanel")
            {
                foreach (Transform child2 in child)
                {
                    if (child2.tag == "PlayerMoney")
                    {
                        Text finaler = child2.GetComponent<Text>();
                        return finaler;
                    }
                }
            }
        }
        return null;
    }




}
