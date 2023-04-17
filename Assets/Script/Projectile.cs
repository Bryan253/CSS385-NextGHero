using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 40f;
    private Renderer r;
    
    void Start()
    {
        // Setup velocity and misc
        r = GetComponent<Renderer>();
        GetComponent<Rigidbody2D>().velocity = transform.up * speed;
        ColliderSetup();
    }

    // Setup collider to ignore Hero
    void ColliderSetup()
    {
        Collider2D p = GameObject.Find("Hero").GetComponent<Collider2D>();
        Collider2D e = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(p, e);
    }

    void Update()
    {
        // Destroy projectile upon exiting the screen
        if(!r.isVisible)
        {
            Destroy(this.gameObject);
            Controller.eggCount--;
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        Destroy(this.gameObject);
        
        if(c.tag == "Plane")
            PlaneCollision(c);
        if(c.tag == "Waypoint")
            WaypointCollision(c);
    }

    void PlaneCollision(Collider2D c)
    {
        var p = c.gameObject.GetComponent<Plane>();
        p.hp--;

        p.currentC.a -= 0.25f;
        p.GetComponent<Renderer>().material.color = p.currentC;
        Controller.eggCount--;
    }

    void WaypointCollision(Collider2D c)
    {
        c.GetComponent<Waypoint>().ProjectileHit();
    }
}
