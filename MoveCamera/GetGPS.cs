using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGPS : MonoBehaviour
{

    public string gps_info = "";
    public string move_info = "";
    public int flash_num = 1;
    public string flash_time = "";
    public float speed = 3f;
    Vector2 m_screenPos = new Vector2();
    public float zoom = 16f;
    public bool GPSbool = false;
    

    // Use this for initialization
    void Start()
    {

    }
    void Update()
    {


        if (Input.touchCount == 1) //单点触碰移动摄像机
        {
            if (Input.touches[0].phase == TouchPhase.Began)
                m_screenPos = Input.touches[0].position;   //记录手指刚触碰的位置
            if (Input.touches[0].phase == TouchPhase.Moved) //手指在屏幕上移动，移动摄像机
            {
                transform.Translate(new Vector3(Input.touches[0].deltaPosition.x * Time.deltaTime * -2f, Input.touches[0].deltaPosition.y * Time.deltaTime * -2f, 0));
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, speed, speed ));
        }
        //s键后退
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, -1 * speed, -1 * speed));
        }
        //a键后退
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-1 * speed, 0, 0 * Time.deltaTime));
        }
        //d键后退
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed, 0, 0 * Time.deltaTime));
        }
    }
    void UpdatePos()
    {
        GameObject.Find("Directions").GetComponent<DirectionsFactory>().UpdatePos();
        GameObject.Find("Waypoint1").GetComponent<MoveWayPoint>().UpdatePos();
    }
    void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        GUI.Label(new Rect(50, 350, 600, 60), this.gps_info);
        GUI.Label(new Rect(50, 450, 600, 60), this.flash_time);
        GUI.Label(new Rect(50, 550, 600, 60), this.move_info);


        GUI.skin.button.fontSize = 50;
        if (GUI.Button(new Rect(Screen.width / 2 - 550, 50, 400, 85), "GPS_Location"))
        {
            StartCoroutine(StartGPS());
            
            //Vector2d latLon = new Vector2d(Input.location.lastData.latitude, Input.location.lastData.longitude);
            //GameObject.Find("Map").GetComponent<AbstractMap>().UpdateMap(latLon);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 550, 150, 400, 85), "refresh_GPS"))
        {
            this.gps_info = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
            this.gps_info = this.gps_info + " Time:" + Input.location.lastData.timestamp;
            this.flash_num += 1;
            this.flash_time = "flash_time:" + this.flash_num.ToString();
            
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 550, 250, 400, 85), "Move_location"))
        {
            if (GPSbool)
            {
                Vector2d latLon = new Vector2d(Input.location.lastData.latitude, Input.location.lastData.longitude);
                UpdatePos();
                GameObject.Find("Map").GetComponent<AbstractMap>().UpdateMap(latLon);
                UpdatePos();
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(0, 1286, 0), 500f);
                this.move_info = "Move to current location";
            }
            else
            {
                this.move_info = "Please Get GPS First";
            }
        }

        
        if (GUI.Button(new Rect(Screen.width / 2 + 250, 50, 85, 85), "+"))
        {
            zoom = zoom + 1;
            
            GameObject.Find("Map").GetComponent<AbstractMap>().UpdateMap(zoom);
            UpdatePos();

        }
        if (GUI.Button(new Rect(Screen.width / 2 + 250, 150, 85, 85), "-"))
        {
            zoom = zoom - 1;
            
            GameObject.Find("Map").GetComponent<AbstractMap>().UpdateMap(zoom);
            UpdatePos();

        }
        /*if (GUI.Button(new Rect(Screen.width / 2 + 150, 150, 85, 85), "←"))
        {
            transform.Translate(new Vector3(-1 * speed, 0, 0 * Time.deltaTime));
        }
        if (GUI.Button(new Rect(Screen.width / 2 + 350, 150, 85, 85), "→"))
        {
            transform.Translate(new Vector3(speed, 0, 0 * Time.deltaTime));
        }*/

    }

    // Input.location = LocationService
    // LocationService.lastData = LocationInfo 

    void StopGPS()
    {
        Input.location.Stop();
    }

    IEnumerator StartGPS()
    {
        // Input.location                                   Get location
        // LocationService.isEnabledByUser                  Is GPS on?
        if (!Input.location.isEnabledByUser)
        {
            this.gps_info = "isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString() + " Please turn on the GPS";
            yield return false;
        }

        Input.location.Start(10.0f, 10.0f);

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {        
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            this.gps_info = "Init GPS service time out";
            yield return false;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            this.gps_info = "Unable to determine device location";
            yield return false;
        }
        else
        {
            this.gps_info = "N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude;
            this.gps_info = this.gps_info + " Time:" + Input.location.lastData.timestamp;
            GPSbool = true;
            yield return new WaitForSeconds(100);
        }
    }
}