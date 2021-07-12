using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region // ПЕРЕМЕННЫЕ
    public static GameController instance;

    public int score;                       // Общее кол-во очков
    public int iZavoda;                            // Индекс завода
    public int sumPlaces;                   // Всеобщая статичная переменная, созданная для того, чтобы связывать PLACE и ZAVOD 
      
    public bool buildMode;                         // Переменная, определяющая включение режима строительства
    public bool demMode;                           // Переменная, определяющая режим сноса
    
    public GameObject stopBttn;             // Обьект, хранящий кнпку остановки строительства
    public GameObject stopDemBttn;          // Пременная, хранящая кнопку остановки режима сноса
    
    public GameObject shopPan;              // Обьект, хранящий панель магазина
    public GameObject zaslon;               // Переменная хранящая коллайдер куба, заслоняющего площадки от нажатия
    public GameObject[] shopWindows;        // массив хранящий пенли в магазине
    int openedWindow;                       // индекс текущего открытого окна
    
    
    public GameObject place;                // Обьект, в который записывается "площадка", при нажатии на неё
    public Text scoreText;                  // Обьект, в котором хранится текст, выводящий очки

    public static Ray ray;                  // Обьект, хранящий луч, использующийся для нажатия на "площадку" или "завод"
    public static RaycastHit hit;           // Переменная удара, использующийся для записи площадки в "place"
    
    public GameObject[] zavodi;             // Массив хранящий заводы
    public int[] prices;                    // Массив с ценами на заводы
    int price;

    public GameObject[] zavodsOnScene;      // Массив, хранящий заводы на сцене
    public GameObject infoPan;              // Переменная, хранящая инфопанель
    public Text productivityText;           // Текст продуктивности
    public Text currentStockText;           // Текст текущей заполненности
    public int iInfoZavoda;                 // Инфоиндекс завода (для показа инфы)
    public bool zavodsStands;               // Переменная, показывающая есть ли заводы на сцене или нет   

    public int indexOfOpenedZavod;
    public GameObject pauseMenu;
    #endregion

    //////AWAKE//////
    void Awake()
    {
        instance = this;
        //Quaternion lightRotation = Quaternion.Euler(50, 30, 0);
        //Instantiate(directLight, transform.position, lightRotation);
    }
    
    //////START//////
    void Start()
    {
        buildMode = false;                  // При старте программы строительство отключено
        demMode = false;                    // При старте режим сноса выключен
        iZavoda = 0;                        // При старте программы индекс завода ставится на "0", но это ни на что не влияет
        sumPlaces = -1;
        GameSaver.gameLoader.Load(Data.newGame);
        score = Data.score;                         // При старте программы очков 30 
        Debug.Log(Data.newGame);
        Data.newGame = false;
        openedWindow = 0;
    } 

    //////UPDATE//////
    void Update()
    {
        scoreText.text = "Money: " + score.ToString();                              // В текст,выводящий очки, каждый кадр записывается слово "Money" и текущее кол-во очков
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);                    // Каждый кадр переменную луча заносится новое значение по позиции мыши игрока
        
        stopBttn.SetActive(buildMode);                                              // Если buildMode = true (режим СТРОИТЕЛЬСТВА вкл), то кнопка проявляется и наоборот
        stopDemBttn.SetActive(demMode);                                             // Если demMode = true (режим СНОСА вкл), то кнопка проявляется, и наоборот

        //Building();
        //Demolishion();

        ProverkaZavodov();
    }

    #region // МАГАЗИН
    public void ShopPan()                               // Метод, закрывающий-открывающий панель магазина
    {
        if (!shopPan.activeSelf)
        {
            shopPan.SetActive(true);        // Активирует панель магазина
            CameraMovement.instance.moveIsOn = false;
        }
        else
        {
            shopPan.SetActive(false);
            CameraMovement.instance.moveIsOn = true;
        }
        zaslon.SetActive(!zaslon.activeSelf);          // Активирует куб
        if (stopBttn.activeSelf) buildMode = false;    // Если заходишь в магазин во время СТРОИТЕЛЬСТВА, режим СТРОИТЕЛЬСТВА и кнопка СТРОИТЕЛЬСТВА выключались
        if (stopDemBttn.activeSelf) demMode = false;   // Если заходишь в магазин во время СНОСА, режим СНОСА и кнопка СНОСА выключались
    }
    public void ShowStopBttn()                          // Показывает кнопку останавливающую строительство
    {
        stopBttn.SetActive(!stopBttn.activeSelf);
    }
    public void StopDemBttn()                           // Показывает кнопку останавливающую снос
    {
        stopDemBttn.SetActive(!stopDemBttn.activeSelf);
    }
    public void ShowWindow(int index)
    {
        foreach (GameObject window in shopWindows)
        {
            window.SetActive(false);
        }
        shopWindows[openedWindow].SetActive(false);
        shopWindows[index].SetActive(true);
    }
    #endregion
    
    #region // МЕНЮ ПАУЗЫ
    // МЕНЮ ПАУЗЫ
    public void PauseMenu()                             // Включает-выключает меню паузы
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0f;                        // Если меню паузы активно == > время равно 0
        }
        else
        {
            Time.timeScale = 1f;                        // Выключенно ==> время идет в нормальном режиме
        }
    }
    public void Quit()                                  // Метод кнопки "выход"
    {
        PauseMenu();                                    // Закрывает меню паузы
        Data.score = score;
        GameSaver.gameLoader.Save();
        SceneManager.LoadScene("MainMenu");             // Загружает сцену главного меню
    }
    #endregion
    
    #region // ИНФА О ЗАВОДАХ
    public void ProverkaZavodov()                       // Проверяет наличие заводов на сцене и записывает имеющиеся в массив
    {
        zavodsOnScene = GameObject.FindGameObjectsWithTag("Zavod");   // Поиск заводов с тегом "Zavod" 
        sumPlaces = zavodsOnScene.Length-1;
        if (zavodsOnScene == null) zavodsStands = false;              // Если массив пустой == > переменная, определяющая существование заводов, отрицательная
        else zavodsStands = true;                                     // Если в массиве что-то есть == > переменная положительна
    }
    public GameObject FindZavod(int index)              // Метод, который возвращает завод по индексу
    {
        GameObject foundZavod = null;                                     // Обьявляется переменная, и в неё записывается значение null
        foreach (GameObject zavod in zavodsOnScene)                       // Перебор обьектов в массиве заводов, находящихся на сцене
        {
            if (zavod.GetComponent<ZavodScript>().iThisZavoda == index)   // Если индекс, прописанный в скрипте завода, равен нужному индексу
                foundZavod = zavod;                                       // Возвращаемой переменной приписывается значение этого завода
        }
        return foundZavod;                                                // Возвращает нужный завод
    }
    public GameObject FactoryStatsWin;
    public GameObject infoFactory;

    public Text productivity;
    public Text priceOfParty;
    public Text timeBeforeSell;

    public Text upgPrdctPrice;
    public Text upgPOPPrice;
    public Text upgTBSPrice;


    public void ShowStats(GameObject factory, bool refresh)
    {
        if (refresh)
        {
            FactoryStatsWin.SetActive(true);
            zaslon.SetActive(true);
            CameraMovement.instance.moveIsOn = false;
        }
        infoFactory = factory;
        productivity.text = infoFactory.GetComponent<ZavodScript>().currentProductivity.ToString();
        priceOfParty.text = infoFactory.GetComponent<ZavodScript>().currentPriceOfParty.ToString();
        timeBeforeSell.text = infoFactory.GetComponent<ZavodScript>().currentTimeBeforeSell.ToString();
        upgPrdctPrice.text = infoFactory.GetComponent<ZavodScript>().productivityPrices[infoFactory.GetComponent<ZavodScript>().prdctLevel].ToString();
        upgPOPPrice.text = infoFactory.GetComponent<ZavodScript>().pricesOfPartyPrices[infoFactory.GetComponent<ZavodScript>().priceLevel].ToString();
        upgTBSPrice.text = infoFactory.GetComponent<ZavodScript>().timeBeforeSellPrices[infoFactory.GetComponent<ZavodScript>().timeLevel].ToString();
    }
    public void FactoryUpgrade(int i)
    {
        switch (i)
        {
            case 1: infoFactory.GetComponent<ZavodScript>().UpProductivity();
                break;
            case 2: infoFactory.GetComponent<ZavodScript>().UpPriceOfParty();
                break;
            case 3: infoFactory.GetComponent<ZavodScript>().UpTimeBeforeSell();
                break;
            default:
                break;
        }
    }
    public void HideStats()
    {
        FactoryStatsWin.SetActive(false);
        zaslon.SetActive(false);
        CameraMovement.instance.moveIsOn = true;
    }
    #endregion

    #region // СТРОИТЕЛЬСТВО
    public void BuildMode(int i)                        // Метод, срабатывающий при нажатии на кнопку выбора завода, ставит индекс и разрешает постройку
    {
        iZavoda = i;                                    // В переменную индекс завода заносится значение, указанное на кнопке
        buildMode = true;                               // Включается режим постройки, building становится true
        ShopPan();                                      // Закрывает панель магазина
    }
    public void BuildModeOFF()                          // Выключает режим строительства
    {
        buildMode = false;                             // Ставит building на false
        ShowStopBttn();                                // Скрывает кнопку "Остановить строительство"
    }  
    #endregion
    
    #region // СНОС
    public void DemMode()                               // Метод, включающийся при нажатии на кнопку DESTROY (снос)
    {
        demMode = true;                                // Режим строительства включен
        ShopPan();                                     // Магазин закрывается
    }
    public void DemModeOFF()                            // Метод, срабатывающий при нажатии на кнопку STOP DESTROY
    {
        demMode = false;                               // Режим сноса останавливается
    }
    #endregion
}
