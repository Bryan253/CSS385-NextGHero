using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public int hp = 4;
    private Color originalC;
    private Color currentC;
    
    // Start is called before the first frame update
    void Start()
    {
        originalC = GetComponent<SpriteRenderer>().material.color;
        currentC = GetComponent<SpriteRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ProjectileHit()
    {
        if(--hp == 0)
            RenewWaypoint();
        else
        {
            currentC.a -= 0.25f;
            GetComponent<SpriteRenderer>().material.color = currentC;
        }
    }

    public void RenewWaypoint()
    {
        hp = 4;
        var pos = transform.position;
        var newX = pos.x + Random.Range(-15f, 15f);
        var newY = pos.y + Random.Range(-15f, 15f); // TODO: within 90% screen
        var newPos = new Vector3(newX, newY, 0);
        transform.position = newPos;
        GetComponent<SpriteRenderer>().material.color = originalC;
    }
}
