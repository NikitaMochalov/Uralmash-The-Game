using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour {

    public static LandManager instance;

    private GameObject landToAdd;
    public GameObject standartLand;
    public GameObject stopPickLandBttn;
    public GameObject confirmationWin;

    public List<GameObject> landPlaces = new List<GameObject>();

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        landToAdd = standartLand;
    }
    public void LandsToShow()
    {
        GameController.instance.ShopPan();
        stopPickLandBttn.SetActive(!stopPickLandBttn.activeSelf);

        foreach (GameObject land in landPlaces)
        {
            land.SetActive(true);
        }

        Debug.Log(landPlaces.Count);
    }
    public void LandsToHide()
    {
        stopPickLandBttn.SetActive(!stopPickLandBttn.activeSelf);
        foreach (GameObject land in landPlaces)
        {
            land.SetActive(false);
        }
    }
    public GameObject LandToAdd()
    {
        return landToAdd;
    }
    public void CloseConfirmWin()
    {
        confirmationWin.SetActive(false);
    }
    public void AcceptBuild()
    {
        CloseConfirmWin();
        foreach (GameObject land in landPlaces)
        {
            if (land.GetComponent<LandPlaceScript>().letsBuild)
            {
                land.GetComponent<LandPlaceScript>().BuildLand();
            }
        }
    }
    /*void Start()
    {
        // Создает при запуске массив, в который заносит все gameobjects с тегом landplace, и добавляет каждый обьект из массива в лист 
        CheckLandPlaces(false);        
        landToAdd = standartLand;
    }

    public GameObject LandToAdd()
    {
        return landToAdd;
    }

    public void LandsToShow()
    {
        GameController.instance.ShopPan();
        stopPickLandBttn.SetActive(!stopPickLandBttn.activeSelf);

        foreach (GameObject land in landPlaces)
        {
            land.SetActive(true);
        }

        Debug.Log(landPlaces.Count);
    }

    public void LandsToHide()
    {
        stopPickLandBttn.SetActive(!stopPickLandBttn.activeSelf);
        CheckLandPlaces(false);
    }
    public void CloseConfirmWin()
    {
        confirmationWin.SetActive(false);
    }
    public void AcceptBuild()
    {
        CloseConfirmWin();
        CheckLandPlaces(true);
        foreach (GameObject land in landPlaces)
        {
            if (land.GetComponent<LandPlaceScript>().letsBuild)
            {
                land.GetComponent<LandPlaceScript>().BuildLand();
            }
        }
    }

    void CheckLandPlaces(bool show)
    {
        GameObject[] landPlcs = GameObject.FindGameObjectsWithTag("LandPlace");
        Debug.Log(landPlcs.Length);

        foreach (GameObject landPlc in landPlcs)
        {
            landPlaces.Add(landPlc);
            landPlc.SetActive(show);
        }
    }*/
}
