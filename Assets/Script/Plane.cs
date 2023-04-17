using UnityEngine;

public class Plane : MonoBehaviour
{
    public int hp = 4;
    public float speed = 10f;
    public Color currentC;
    private int targetWaypoint = 0;

    void Start()
    {
        var oldC = GetComponent<Renderer>().material.color;
        currentC = new Color(oldC.r, oldC.g, oldC.b, oldC.a);
    }

    void Update()
    {
        if(hp <= 0)
            SelfDestruct();
        
        Movement();
    }

    void Movement()
    {
        // Rotating to next waypoint
        var pos = transform.position;
        var toPos = Vector3.zero - pos;
        var angle = Vector2.SignedAngle(pos, toPos);
        transform.Rotate(0, 0, angle * Time.deltaTime);

        // Passive forward movement
        transform.position += transform.up * speed * Time.deltaTime;
    }

    public void SelfDestruct()
    {
        Destroy(this.gameObject);
        Controller.spawnCount--;
        Controller.planeDestroyed++;
    }
}
