using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDMarkScript : MonoBehaviour {

    public int indexPDM;
    public GameObject conectedZavod;

    void Update()
    {
        conectedZavod = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>().FindZavod(indexPDM);
        if (conectedZavod == null)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
            conectedZavod.GetComponent<ZavodScript>().NextParty();
            Destroy(gameObject);
    }
}
