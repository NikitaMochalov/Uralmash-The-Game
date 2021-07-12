using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceScript : MonoBehaviour {

    public bool created;
    public int iPlace;
    void Start()
    {
        created = false;
        iPlace = 0;
    }
    public void OnMouseDown()
    {
        BuildFactory(GameController.instance.iZavoda);
        iPlace = ++GameController.instance.sumPlaces;
    }
    public void BuildFactory(int i)
    {
        int price = GameController.instance.prices[i];
        if (GameController.instance.buildMode && GameController.instance.score >= price) 
        {
            GameObject factory = GameController.instance.zavodi[i];
            //int price = GameController.instance.prices[i];
            Vector3 place = new Vector3(transform.position.x, transform.position.y + 0.04f, transform.position.z);      // Обьявление переменной, записывающей координаты места
            Quaternion zavodRotation = Quaternion.identity;                                                          // Обьявление переменной, записывающей координаты поворота завода

            Instantiate(factory, place, zavodRotation);                                               // Клонируется префаб завода и ставится по координатам и поворачивается под определенным углом
            GameController.instance.score -= price;
            created = true;
        }
    }
}
