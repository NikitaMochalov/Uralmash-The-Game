using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

public class SaveableObject : MonoBehaviour 
{
    [SerializeField]
    private string objectName;

    private GameSaver saver;                               // Создаем переменную для класса сохранений

    private void Awake()
    {
        saver = FindObjectOfType<GameSaver>();             // Находим обьект GameSaver на сцене
    }

	void Start () 
    {
        saver.objects.Add(this);                           // В лист objects добавляем с помощью функции Add этот элемент
	}

    private void OnDestroy()                               
    {
        saver.objects.Remove(this);                        // При разрушении обьекта удаляем обьект из листа
    }

    public XElement GetElement()                           // Функция возвращающая сохраняемый элемент
    {
        XAttribute x = new XAttribute("x", transform.position.x);        // Записывает атрибут x в файл xml под именем "x" и его значение (transform.position.x)
        XAttribute y = new XAttribute("y", transform.position.y);        // Записывает атрибут y в файл xml под именем "y" и его значение (transform.position.y)
        XAttribute z = new XAttribute("z", transform.position.z);        // Записывает атрибут z в файл xml под именем "z" и его значение (transform.position.z)


        
        if (this.tag == "Place")                                         // Если тег этого элемента "Place" то
        {
            XAttribute index = new XAttribute("iZavoda", this.gameObject.GetComponent<PlaceScript>().iPlace);  // Создаем новый атрибут, хранящий индекс места
            
            XElement element = new XElement("instance", objectName, x, y, z, index);                           // Записываем, как выглядят характеристики элемента
            return element;                                                                                    // Передаем элемент
        }
        else if (this.tag == "Zavod")                                    // Если тег этого элемента "Zavod" то
        {
            XAttribute index = new XAttribute("iZavoda", gameObject.GetComponent<ZavodScript>().iThisZavoda);  // Создаем новый атрибут, хранящий индекс завода
            XAttribute currentStock = new XAttribute("currentStock", gameObject.GetComponent<ZavodScript>().currentStock);  // Создаем новый атрибут, хранящий текущую заполненность склада завода
            XAttribute currPrdctLevel = new XAttribute("prdctLevel", gameObject.GetComponent<ZavodScript>().prdctLevel);

            XElement element = new XElement("instance", objectName, x, y, z, index, currentStock, currPrdctLevel);             // Записываем, как выглядят характеристики элемента
            return element;                                                                                    // Передаем элемент
        }
        else
        {
            XElement element = new XElement("instance", objectName, x, y, z);      // Создает элемент в xml файле с именем "instance" с атрибутами objectName, x, y, z
            return element;
        }
    }

    public void DestroySelf()               // Метод необходимый если на сцене уже есть обьекты (удаляет всё)
    {
        Destroy(gameObject);
    }
}
