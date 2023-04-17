using UnityEngine;

public class Plane : MonoBehaviour
{
    public int hp = 4;
    public float speed = 10f;
    public Color currentC;
    public float maxAngle = 45f;
    public int waypointIndex = 0;
    private string[] letter = {"A", "B", "C", "D", "E", "F"};
    private bool isRandom = false;

    void Start()
    {
        var oldC = GetComponent<Renderer>().material.color;
        currentC = new Color(oldC.r, oldC.g, oldC.b, oldC.a);
        waypointIndex = Random.Range(0, 6); // Fly to random index
    }

    void Update()
    {
        if(hp <= 0)
            SelfDestruct();

        // Update Moving Pattern
        if(Input.GetKeyDown(KeyCode.J))
        {
            isRandom = !isRandom;
            UpdateIndex();
        }
        
        Movement();
    }

    void Movement()
    {
        RotatePlane();
        // Passive forward movement
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void RotatePlane()
    {
        var rV = Controller.getWayPointPos(waypointIndex) - transform.position;
        var angle = Mathf.Atan2(rV.y, rV.x) * Mathf.Rad2Deg - 90f;
        var deltaMaxAngle = maxAngle * Time.deltaTime;
        //Debug.Log(deltaMaxAngle);
        //Debug.Log(angle);
        //var z = transform.rotation.z;
        //var deltaAngle = Mathf.Clamp(angle, z - deltaMaxAngle, z + deltaMaxAngle);

        var r = Quaternion.AngleAxis(angle, Vector3.forward);
        //var r = Quaternion.AngleAxis(deltaAngle, Vector3.forward);
        transform.rotation = r;
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject);
        Controller.spawnCount--;
        Controller.planeDestroyed++;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.tag == "Waypoint" && c.name == letter[waypointIndex])
            UpdateIndex();
    }

    void UpdateIndex()
    {
        if(isRandom)
        {
            var currentIndex = waypointIndex;
            while(currentIndex == waypointIndex)
                waypointIndex = Random.Range(0, 6);
        }
        else if(++waypointIndex >= 6)
            waypointIndex = 0;    
    }
}
