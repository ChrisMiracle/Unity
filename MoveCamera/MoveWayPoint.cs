using UnityEngine;
using Mapbox.Directions;
using System.Collections.Generic;
using System.Linq;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Modifiers;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using System.Collections;
using Mapbox.Unity.MeshGeneration.Factories;

public class MoveWayPoint : MonoBehaviour {


    [SerializeField]
    AbstractMap _map;

    protected virtual void Awake()
    {
        if (_map == null)
        {
            _map = FindObjectOfType<AbstractMap>();
        }
        
    }

    public string move_info = "";
    public string location_info = "";
    float speed = 500f;
    //string lat_csit = "-35.27516, 149.1192";
    //string lat_center = "-35.2819, 149.1289";
    Vector2d latlon_csit = new Vector2d(-35.27516f, 149.1192f);
    Vector3 world_csit = new Vector3(-174.4598f, 0, 148.5087f);
    Vector2d latlon_center = new Vector2d(-35.2819f, 149.1289f);
    Vector2d world_center = new Vector2d(0, 0);
    Vector2d Mercator_center = new Vector2d(-4202256.52414, 16600953.21056);
    float WorldRelativeScale = 0.1635333f;
    Vector3 pos;
    Vector2d pos_2D;

    void OnGUI() {
        GUI.Label(new Rect(50, 630, 1000, 60), this.move_info);
        GUI.Label(new Rect(50, 730, 1000, 60), this.location_info);
        
    }
    // Use this for initialization
    void Start () {
        //pos = GameObject.Find("Map").GetComponent<AbstractMap>().GeoToWorldPosition(latlon); // move 0 ,0 ,0
        //pos = Conversions.GeoToWorldPosition(latlon_csit, Mercator_center, WorldRelativeScale).ToVector3xz(); // move 0, 0, 0
        //pos = Conversions.GeoToWorldPosition(-35.27516, 149.1192, latlon_center, 1f).ToVector3xz();
        /*pos.x = (float)pos_2D.y;
        pos.y = 0f;
        pos.z = (float)pos_2D.x;*/
        //pos = new Vector3(-174.4598f, 0, 148.5087f);   // location of csit
        //this.location_info = "location_info : "+ _map.CenterMercator;
        pos =GameObject.Find("Directions").GetComponent < DirectionsFactory> ().csitpos();      
        MovePoint(pos);
	}
    public virtual bool MovePoint(Vector3 pos)
    {
        this.move_info = "move_info : " + pos;
        float step = speed;
        gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, pos, step);
        return true;
    }
    public void UpdatePos()
    {
        pos = GameObject.Find("Directions").GetComponent<DirectionsFactory>().csitpos();
        MovePoint(pos);
    }
    // Update is called once per frame
    void Update () {
        
        //Vector3 pos = GameObject.Find("Map").GetComponent<AbstractMap>().GeoToWorldPosition(latlon);
        //MovePoint(pos);
    }
}
