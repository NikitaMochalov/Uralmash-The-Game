using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandPlaceScript : MonoBehaviour {

    private GameObject land;
    public bool letsBuild;
    //public string name;
    //GameObject confirmWin;

    void Start()
    {
        letsBuild = false;
        //confirmWin = LandManager.instance.confirmationWin;
    }
	void OnMouseDown () 
    {
        //confirmWin.SetActive(!confirmWin.activeSelf);
        LandManager.instance.confirmationWin.SetActive(true);
        letsBuild = true;
        
	}
    void OnDestroy()
    {
        LandManager.instance.landPlaces.Remove(gameObject);
    }
    public void BuildLand()
    {
        if (letsBuild && GameController.instance.score >= 20)
        {
            GameObject landToAdd = LandManager.instance.LandToAdd();
            land = Instantiate(landToAdd, transform.position, Quaternion.identity) as GameObject;
            GameController.instance.score -= 20;
            Destroy(gameObject);
        }
    }
}
