using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButtonLogic : MonoBehaviour {

    bool buttonPressed = false;


	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && touch.phase == TouchPhase.Began && hit.transform.tag == "Button")
                {
                    hit.collider.SendMessage("BuildMode");
                    Debug.Log("Something" + hit.collider.name);
                }

            }
        }
	}
    
}
