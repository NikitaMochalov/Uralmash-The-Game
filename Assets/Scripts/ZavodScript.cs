using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ZavodScript : MonoBehaviour {

    public int iThisZavoda;

    public int[] productivity;
    public int[] productivityPrices;
    public int prdctLevel;

    public int[] pricesOfParty;
    public int[] pricesOfPartyPrices;
    public int priceLevel;

    public int[] timeBeforeSell;
    public int[] timeBeforeSellPrices;
    public int timeLevel;
    
    public int currentProductivity;                // Сколько очков будет добавлятся за определенное время
    public int currentPriceOfParty;               // Цена одной партии (сколько очков будет добавляться к общеигровому числу очков
    public float currentTimeBeforeSell;

    public GameObject factoryStatsWin;
    
    public float currentStock;               // Текущее кол-во товара (очков) на заводе
    public float stock;                      // Сколько товаров может вместить в себя завод (полная партия товаров)

    public float nextAdd;                  // Ну, тут понятно
    public float addRate;                  // Через сколько добавлять очки
    
    public GameObject partyDoneMark;       // Содержит ссылку на префаб марки
    public bool PDMcreated;                // Проверяет, готова ли партия

    public GameObject infoCanvas;          // Содержит ссылку на канвас
    public Image stockBar;                 // Содержит ссылку на заполняемую картинку состояния  
    [SerializeField]
    bool factoryClicked;                   // Проверяет, было ли нажатие на завод

    public bool loaded;
    float startTime;
    
    void Start()
    {
        if (!loaded)
        {
            iThisZavoda = GameController.instance.sumPlaces;     // Присваивается индекс копии завода, исходя из общего количества поставленных заводов (значение sumplaces)
            currentProductivity = productivity[0];
            currentPriceOfParty = pricesOfParty[0];
            currentTimeBeforeSell = timeBeforeSell[0];
        }
        currentProductivity = productivity[prdctLevel];
        PDMcreated = false;
    }

    void Update()
    {
        AddRate();
        PartyDone();
        ShowStockBar();
    }
    
    private void AddRate()
    {
        if (currentStock < stock && Time.time > nextAdd)
        {
            currentStock += currentProductivity;
            nextAdd = Time.time + addRate;
        }
    }  

    private void PartyDone()
    {
        if (currentStock >= stock && !PDMcreated)
        {
            CreatePDM();
        }
    }   

    public void CreatePDM()
    {
        Quaternion PDMrotation = Quaternion.Euler(23, 45, 0);                                                             // Создается переменная поворота PDMark
        Vector3 PDMposition = new Vector3(transform.position.x-25, transform.position.y+21, transform.position.z-25);     // Создается пременная позиции PDMark
        partyDoneMark.GetComponent<PDMscript>().indexPDM = iThisZavoda;                                                   // В скрипте PDMark переменной индекса присваивается значение индекса завода
        Instantiate(partyDoneMark, PDMposition, PDMrotation);                                                             // Создается PDMark
        PDMcreated = true;                                                                                                // Марка отмечается как созданная
    } 
    
    public void NextParty()
    {
        GameController.instance.score += currentPriceOfParty;                                                                    // В скрипт GameController'а добавляется цена партии к общим очкам
        currentStock = 0;                                                                                                 // Счетчик склада обнуляется
        PDMcreated = false;                                                                                               // Марка отмечается как НЕсозданная
    }
    
    void ShowStockBar()
    {
        if (factoryClicked)                                                     // Если произошло нажатие на фабрику (Завод открыт)
        {
            stockBar.fillAmount = currentStock / stock;                         // Свойству fillAmount картинки stockBar присваивается значение текущего кол-во товара поделенного на полную партию товаров
            Debug.Log("Factory clicked with index " + iThisZavoda);
        }
        
        if (GameController.instance.indexOfOpenedZavod != iThisZavoda)          // Если индекс ОТКРЫТОГО завода (GameController) не совпадает с индексом этого завода  
        {
            infoCanvas.SetActive(false);                                        // Инфопанель закрывается

            factoryClicked = false;                                             // Завод не открыт
        }
    }

    void OnMouseDown()
    {
        if (GameController.instance.demMode)
        {
            Destroy(gameObject);

            GameObject[] places = GameObject.FindGameObjectsWithTag("Place");                      // Обьявляется массив places, в который заносятся все обьекты с тегом "Place"
            foreach (GameObject place in places)                                                   // Идет перечисление обьетов в массиве places с использованием новой переменной place
            {
                if (place.GetComponent<PlaceScript>().iPlace == iThisZavoda)                                 // Если индекс места совпадает с индеском завода,
                    place.GetComponent<PlaceScript>().created = false;                                 // то переменной created задается значаение false (Ничего не построенно)
            }
        }
        startTime = Time.time;
        
    }
    public void OnMouseUp()
    {
        if (Time.time - startTime < 0.5f)
        {
            factoryClicked = !factoryClicked;                              // Завод открывается или закрывается (нажатие произошло)

            if (!infoCanvas.activeSelf)                                    // Если инфопанель до этого не была активна
            {
                GameController.instance.indexOfOpenedZavod = iThisZavoda;  // Индексу открытого завода (GameController) присваивается индекс этого завода
                infoCanvas.SetActive(true);                                // Инфопанель становится активной
            }
            else if (infoCanvas.activeSelf)                                // Иначе
            {
                infoCanvas.SetActive(false);                               // Инфопанель становится неактивной
            }
        }
    }
    public void OnMouseDrag()
    {
        if (Time.time - startTime > 0.25f)
        {
            GameController.instance.ShowStats(gameObject, true);
            if (infoCanvas.activeSelf)
            {
                factoryClicked = !factoryClicked;
                infoCanvas.SetActive(false);
            }
        }
    }
    public void UpProductivity()
    {
        if (GameController.instance.score >= productivityPrices[prdctLevel] && prdctLevel < 9)
        {
            GameController.instance.score -= productivityPrices[prdctLevel];
            prdctLevel++;
            currentProductivity = productivity[prdctLevel];
            GameController.instance.ShowStats(gameObject, false);
        }
    }
    public void UpPriceOfParty()
    {
        if (GameController.instance.score >= pricesOfPartyPrices[priceLevel] && priceLevel < 9)
        {
            GameController.instance.score -= pricesOfPartyPrices[priceLevel];
            priceLevel++;
            currentPriceOfParty = pricesOfParty[priceLevel];
            GameController.instance.ShowStats(gameObject, false);
        }
    }
    public void UpTimeBeforeSell()
    {
        if (GameController.instance.score >= timeBeforeSellPrices[timeLevel])
        {
            GameController.instance.score -= timeBeforeSellPrices[timeLevel];
            timeLevel++;
            currentTimeBeforeSell = timeBeforeSell[timeLevel];
            GameController.instance.ShowStats(gameObject, false);
        }
    }

    public void StartMG_1()
    {
        SceneManager.LoadScene("MG1");
    }
}
