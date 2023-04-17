using UnityEngine;
using UnityEngine.UIElements;

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
        var z = transform.rotation.z;
        var rV = Controller.getWayPointPos(waypointIndex) - transform.position;
        var angle = Vector2.SignedAngle(Vector2.up, rV);
        var deltaMaxAngle = Time.deltaTime * maxAngle;
        var r = Quaternion.AngleAxis(angle, Vector3.forward);
        r = Quaternion.RotateTowards(transform.rotation, r, deltaMaxAngle);
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
