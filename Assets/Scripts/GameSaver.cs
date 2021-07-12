using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.IO;

public class GameSaver : MonoBehaviour {

    public static GameSaver gameLoader;

    private string path;

	public List<SaveableObject> objects = new List<SaveableObject>();           // Создаем лист из обьектов класса SaveableObject

    private void Awake()
    {
        path = Application.persistentDataPath + "/testsave.xml";                // Путь с файлом сохранения
        gameLoader = this;
    }

    public void Save()
    {
        XElement root = new XElement("root");                                   // Создаем элемент, в который будет записывать все остальные элементы
        
        foreach (SaveableObject obj in objects)
        {
            root.Add(obj.GetElement());                                         // Добавляем по очереди элементы в элемент root 
        }

        root.AddFirst(new XElement("score", Data.score));                       // Добавляем в root новый первый элемент с названием "score" вмещающий значение score
        Debug.Log(root);

        XDocument saveDoc = new XDocument(root);

        File.WriteAllText(path, saveDoc.ToString());                            // WriteAllText(путь, строка)
        Debug.Log(path);
    }

    public void Load(bool newGame)
    {
        XElement root = null;

        if (!File.Exists(path)||newGame)                                        // Если файла не существует ИЛИ переменная newGame = true
        {
            if (File.Exists(Application.persistentDataPath + "/newGame.xml"))     // Если существует файл с именем newGamelevel.xml  (чистый уровень)
                // Читаем файл в виде строки ==> Возвращаем документ ==> В документе берем элемент root
                root = XDocument.Parse(File.ReadAllText(Application.persistentDataPath + "/newGame.xml")).Element("root");
            
            Data.score = 50;                                                    // Значению очков в Data-файле присваивается превичное значение очком
        }
        else                                                                    // Иначе (Если файл существует)
        {
            root = XDocument.Parse(File.ReadAllText(path)).Element("root");     // root присваивается значение root из этого файла (путь указан)
        }

        if (root == null) { 
            Debug.Log("Level load failed...");
            return;
        }

        Debug.Log(root);

        GenerateScene(root);
    }
    
    private void GenerateScene(XElement root)
    {
        foreach (SaveableObject obj in objects)
        {
            obj.DestroySelf();
        }

        foreach (XElement instance in root.Elements("instance"))    // Перечисляет все элементы с названием instance
        {
            Vector3 position = Vector3.zero;

            position.x = float.Parse(instance.Attribute("x").Value);
            position.y = float.Parse(instance.Attribute("y").Value);
            position.z = float.Parse(instance.Attribute("z").Value);

            GameObject instanceObj = Resources.Load<GameObject>(instance.Value);
            
            Debug.Log(instance.Value);

            if (instanceObj.tag == "Zavod")
            {
                GameObject newZavod = Instantiate(instanceObj, position, Quaternion.identity);

                newZavod.GetComponent<ZavodScript>().currentStock = float.Parse(instance.Attribute("currentStock").Value);
                newZavod.GetComponent<ZavodScript>().iThisZavoda = int.Parse(instance.Attribute("iZavoda").Value);
                newZavod.GetComponent<ZavodScript>().prdctLevel = int.Parse(instance.Attribute("prdctLevel").Value);
                newZavod.GetComponent<ZavodScript>().loaded = true;
            }
            else
            {
                Instantiate(Resources.Load<GameObject>(instance.Value), position, Quaternion.identity);
            }
        }
        Data.score = int.Parse(root.Element("score").Value);
    }
}
