using UnityEngine;
using TMPro;

public class Controller : MonoBehaviour
{
    private Camera c;
    [SerializeField] private GameObject spawnCreature;
    public static int spawnCount = 0;
    public static int maxSpawn = 10;
    public static int heroCollisionCount = 0;
    public static int eggCount = 0;
    public static int planeDestroyed = 0;
    public TextMeshProUGUI heroTxt, eggTxt, enemyTxt;
    public static GameObject[] wArray = new GameObject[6];
    private static string[] letter = {"A", "B", "C", "D", "E", "F"};
    private bool isActive = true;
    private bool isRandom = false;

    void Start()
    {
        c = Camera.main;

        // Initialize the GameObject array for waypoint
        for(int i = 0; i < wArray.Length; i++)
            wArray[i] = GameObject.Find(letter[i]);
    }

    void Update()
    {
        while(spawnCount < maxSpawn)
            spawn();

        UpdateText();

        if(Input.GetKeyDown(KeyCode.H))
            HideWaypoint();
        if(Input.GetKeyDown(KeyCode.J))
            isRandom = !isRandom;
    }

    void HideWaypoint()
    {
        foreach(var w in wArray)
            w.SetActive(!isActive);
        isActive = !isActive;
    }

    void spawn()
    {
        // Prevent for spawn than 10
        if(spawnCount >= maxSpawn)
            return;
        
        // Finding area under 90% of main cam to spawn
        var width = c.pixelWidth;
        var height = c.pixelHeight;
        var spawnX = Random.Range(0.1f * width, 0.9f * width);
        var spawnY = Random.Range(0.1f * height, 0.9f * height);
        var screenVec = new Vector2(spawnX, spawnY);
        Vector2 spawnPt = c.ScreenToWorldPoint(screenVec);
        
        Instantiate(spawnCreature, spawnPt, Quaternion.identity);
        spawnCount++;
    }

    void UpdateText()
    {
        heroTxt.text = $"Hero\nControl: {(Hero.mouseControl ? "Mouse" : "Keyboard")}\nEnemy Collision : {heroCollisionCount}";
        eggTxt.text = $"Egg\nCount: {eggCount}\nWaypoint: {(isRandom ? "Seq" : "Random")}";
        enemyTxt.text = $"Enemy\nCount: {spawnCount}\nTotal Destroyed: {planeDestroyed}";
    }

    public static Vector3 getWayPointPos(int index)
    {
        return wArray[index].transform.position;
    }
}
