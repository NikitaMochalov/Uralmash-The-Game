using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryButton : TouchButtonLogic {

    [SerializeField]
    int indx;

    void BuildMode()
    {
        GameController.instance.BuildMode(indx);
    }
}
