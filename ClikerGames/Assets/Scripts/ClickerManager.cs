using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClickerManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI clickValueText;
    public Button clickButton;
    public Button upgradeButton;
    
    public GameObject moneyPrefab;
    public RectTransform moneyContainer;
    public int poolSize = 10;
    public Transform moneyPoint;
    
    private int money = 0;
    private int clickValue = 1;
    private int upgradeLevel = 0;
    private int upgradeCost = 20;

    private Queue<GameObject> moneyPool = new Queue<GameObject>();
    private bool isClickProcessing = false;

    void Start()
    {
        UpdateUI();
        InitializeMoneyPool();
        clickButton.onClick.RemoveAllListeners();
        clickButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        if (isClickProcessing) return;
        isClickProcessing = true;

        money += clickValue;
        UpdateUI();

        Vector2 clickPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            moneyContainer, 
            Input.mousePosition, 
            Camera.main, 
            out clickPosition
        );

        SpawnMoney(clickPosition);
        StartCoroutine(ResetClick());
    }

    private IEnumerator ResetClick()
    {
        yield return new WaitForSeconds(0.05f);
        isClickProcessing = false;
    }

    public void OnUpgrade()
    {
        if (money >= upgradeCost)
        {
            money -= upgradeCost;
            upgradeLevel++;
            clickValue += 1;
            upgradeCost += 20;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        moneyText.text = money.ToString();
        levelText.text = "LV " + upgradeLevel;
        clickValueText.text = "+" + clickValue;
        upgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "UPGRADE " + upgradeCost;
        upgradeButton.interactable = money >= upgradeCost;
    }

    private void InitializeMoneyPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newMoney = Instantiate(moneyPrefab, moneyContainer);
            newMoney.SetActive(false);
            moneyPool.Enqueue(newMoney);
        }
    }

        private void SpawnMoney(Vector2 spawnPos)
    {
        if (moneyPrefab == null || moneyContainer == null) return;

        GameObject newMoney = moneyPool.Count > 0 ? moneyPool.Dequeue() : Instantiate(moneyPrefab, moneyContainer);
        newMoney.SetActive(true);

        RectTransform moneyRect = newMoney.GetComponent<RectTransform>();
        moneyRect.anchoredPosition = spawnPos;

        MoneyMovement movement = newMoney.GetComponent<MoneyMovement>();
        if (movement == null) return;

        movement.SetTarget(spawnPos, moneyPoint, () => ReturnMoneyToPool(newMoney));
    }

    private void ReturnMoneyToPool(GameObject money)
    {
        money.SetActive(false);
        moneyPool.Enqueue(money);
    }
}
