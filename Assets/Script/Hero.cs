using UnityEngine;

public class Hero : MonoBehaviour
{
    private Rigidbody2D rd;
    public float speed = 20f;
    public static bool mouseControl = true;
    private Camera c;
    public float speedMulti = 1f;
    public float speedMultiMax = 5f;
    public GameObject projectile;
    public float fireRate = 0.2f;
    private float fireCD = 0f;


    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        c = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Detect inputs both available in mouse and keyboard control
        UniversalControl();

        // Moves this object based on control
        if(mouseControl)
            MouseMovement();
        else
            KeyboardMovement();

        if(Input.GetKey(KeyCode.Space))
            Shoot();
    }

    void UniversalControl()
    {
        // Switch betweem mouse control
        if(Input.GetKeyDown(KeyCode.M))
            mouseControl = !mouseControl;
        
        // Detect key for rotation
        if(Input.GetKey(KeyCode.A))
            transform.Rotate(new Vector3(0, 0, 45 * Time.deltaTime));

        if(Input.GetKey(KeyCode.D))
            transform.Rotate(new Vector3(0, 0, -45 * Time.deltaTime));
    }

    void MouseMovement()
    {
        rd.velocity = Vector2.zero; // Stops Hero from moving forward

        // Convert mouse position to screen
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = c.ScreenToWorldPoint(mousePos); 
        worldPos.z = transform.position.z;

        transform.position = worldPos;
    }
    
    void KeyboardMovement()
    {
        rd.velocity = transform.up * speed * speedMulti;

        if(Input.GetKey(KeyCode.W))
            speedMulti += 0.25f * Time.deltaTime;
        
        if(Input.GetKey(KeyCode.S))
            speedMulti -= 0.25f * Time.deltaTime;

        speedMulti = Mathf.Clamp(speedMulti, 0, speedMultiMax);
    }

    void Shoot()
    {
        var t = Time.time;
        if(t < fireCD)
            return;
        
        Instantiate(projectile, transform.position, transform.rotation);
        fireCD = t + fireRate;
        Controller.eggCount++;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.tag == "Plane")
            PlaneCollision(c);

        if(c.tag == "Waypoint")
            WaypointCollision(c);
    }

    void PlaneCollision(Collider2D c)
    {
        c.gameObject.GetComponent<Plane>().SelfDestruct();
        Controller.heroCollisionCount++;
    }

    void WaypointCollision(Collider2D c)
    {
        c.gameObject.GetComponent<Waypoint>().RenewWaypoint();
    }
}
