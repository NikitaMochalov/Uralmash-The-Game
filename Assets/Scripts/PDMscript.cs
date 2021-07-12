using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDMscript : MonoBehaviour {

    public int indexPDM;

    public GameObject zavodOfPDM;

    void Start()
    {
        zavodOfPDM = GameController.instance.FindZavod(indexPDM);
    }

    void Update()
    {
        if (zavodOfPDM == null)
        {
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
        if (zavodOfPDM != null) zavodOfPDM.GetComponent<ZavodScript>().NextParty();
    }

    void OnDestroy()
    {
        
    }
}
