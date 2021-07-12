using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private Vector3 startPos;     // Начальное положение свайпа
    private Camera cam;

    //private float targetPosX;
    //private float targetPosY;

    public float speed;
    public bool moveIsOn;
    public static CameraMovement instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        moveIsOn = true;
        //targetPosX = transform.position.x;
        //targetPosY = transform.position.y;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && moveIsOn)
            startPos = cam.ScreenToWorldPoint(Input.mousePosition);           // Выполняется один раз
        else if (Input.GetMouseButton(0) && Input.touchCount < 2 && moveIsOn)
        {
            Vector3 difference = cam.ScreenToWorldPoint(Input.mousePosition) - startPos;
            //float posX = cam.ScreenToWorldPoint(Input.mousePosition).x - startPos.x;
            //float posY = cam.ScreenToWorldPoint(Input.mousePosition).y - startPos.y;
            //float posZ = cam.ScreenToWorldPoint(Input.mousePosition).z - startPos.z;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x - difference.x, -100, 100), Mathf.Clamp(transform.position.y - difference.y, -100, 100), Mathf.Clamp(transform.position.z - difference.z, -100, 100));
            //targetPosX = Mathf.Clamp(transform.position.x - posX, -18.5f, 41);
            //targetPosY = Mathf.Clamp(transform.position.y - posY, -7, 50);
        }

        /*transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, targetPosX, speed * Time.deltaTime), 
            Mathf.Lerp(transform.position.y, targetPosY, speed * Time.deltaTime), 
            transform.position.z);*/
        
        if (Input.touchCount > 1)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0prevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1prevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0prevPos - touch1prevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            cam.orthographicSize += deltaMagnitudeDiff * 0.05f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 10f, 100);
        }
    }
}
