using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Tilemaps;

//mountain covers sun from offset .36 - .46

public class GameController : MonoBehaviour {

    [Header("Object References")]
    public Grid grid;
    public TileBase ground;
    public GameObject powerupPrefab;
    public GameObject platformPrefab;
    public Transform platformSpawn;
    public BackgroundScroller[] backgroundLayers;

    public GameObject[] cactiList;
    public GameObject[] otherObstaclesList;
    public GameObject[] airObstacles;

    [Header("UI References")]
    public GameObject gameOverScreen;
    public TextMeshProUGUI scoreText;

    [Header("Platform Settings")]
    public Vector2Int platformHeightRange;
    public Vector2Int platformWidthRange;
    public Vector2Int platformDistanceRange;

    [Header("Ground Obstacle Settings")]
    public int cactusChance;
    public Vector2Int numCactiPerGroup;
    public Vector2Int groundObstacleGapRange;
    public Vector2Int numRibsPerCage;

    [Header("Air Obstacle Settings")]
    public Vector2Int airObstacleHeightRange;
    public Vector2Int airObstacleSpawnDelayRange;
    public float vultureFlockChance;

    [Header("Powerup Settings")]
    public Sprite[] powerupSpriteList;
    public float powerupChance;
    public float powerupSpawnDelay;

    //Private References
    private Tilemap tileMap;

    //Private Variables
    private int score = 0;
    private int numPlatforms = 0;
    private int maxJumpHeight = 2; //Used so that there are no jumps that are too high for the player

    private int platformWidth;
    private int platformHeight = 3; //Height of the starting platform

    private bool spawnPowerup = false;

    // Start is called before the first frame update
    void Start() {

        try {

            tileMap = platformPrefab.GetComponent<Tilemap>();
            GameObject.Find("StartingPlatform").GetComponent<PlatformScroller>().enabled = true;
            StartCoroutine(IncrementScore());
            StartCoroutine(SpawnAirObstacles());
            StartCoroutine(SpawnPowerups());

            foreach (BackgroundScroller x in backgroundLayers) {
                x.enabled = true;
            }

        } catch (System.Exception e) {
            Debug.Log("Error in GameController.Start: " + e.Message);
        }
    }

    //Increments score every second
    IEnumerator IncrementScore() {

        while (true) {
            score += 1;
            scoreText.text = "Score: " + score;
            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// Tries to spawn a powerup every second based on the spawn chance
    /// If one is spawned, a cooldown is set for a certain amount of time before another can spawn
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnPowerups() {
        while (true) {
            if(Random.value < powerupChance / 100) {
                spawnPowerup = true;
                yield return new WaitForSeconds(powerupSpawnDelay);
            } else {
                yield return new WaitForSeconds(1);
            }
        }
    }

    /// <summary>
    /// Continuously spawns vultures after a certain delay
    /// Spawn delay is randomized within a range
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnAirObstacles() {

        while (true) {

            int spawnX = (int)platformSpawn.transform.position.x;
            int spawnY = Random.Range(airObstacleHeightRange.x, airObstacleHeightRange.y + 1);

            SpawnVulture(spawnX, spawnY);

            if (Random.value < (vultureFlockChance / 100)) {
                SpawnVulture(spawnX + 2, spawnY);
                SpawnVulture(spawnX + 4, spawnY);
            }

            int spawnDelay = Random.Range(airObstacleSpawnDelayRange.x, airObstacleSpawnDelayRange.y + 1);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void SpawnVulture(int spawnX, int spawnY) {

        GameObject vulture = Instantiate(airObstacles[0], new Vector2(spawnX, spawnY + .5f), Quaternion.identity);
        vulture.GetComponent<Animator>().SetFloat("CycleOffset", Random.value);
    }

    //Spawns a new platform
    public void SpawnPlatform(){

        try {

            //Sets the high end of the platform height to either the maximum game height or 2 higher than the previous platform
            int maxHeight = Mathf.Min(platformHeight + maxJumpHeight, platformHeightRange.y);

            //Randomly choose width and height
            platformWidth = Random.Range(platformWidthRange.x, platformWidthRange.y + 1);
            platformHeight = Random.Range(platformHeightRange.x, maxHeight + 1);

            //Generates platform at a certain distance from the last one
            GeneratePlatform();
            int platformDistance = Random.Range(platformDistanceRange.x, platformDistanceRange.y + 1);
            GameObject newPlatform = Instantiate(platformPrefab, new Vector2(platformSpawn.position.x + platformDistance, platformSpawn.position.y), Quaternion.identity);
            newPlatform.name = "Platform" + numPlatforms;
            numPlatforms++;

            //Adds obstacles to the platform
            GenerateObstacles(newPlatform);
            newPlatform.transform.SetParent(grid.GetComponent<Transform>());

            //Clears tilemap for next platform generation
            tileMap.ClearAllTiles();

            if (spawnPowerup) {
                SpawnPowerup(newPlatform);
            }

        } catch (System.Exception e) {
            Debug.Log("Error in GameController.SpawnPlatform: " + e.Message);
        }
    }

    //Generates tilemap for new platforms
    public void GeneratePlatform() {
        for (int x = 0; x < platformWidth; x++) {
            for (int y = 0; y < platformHeight; y++) {
                tileMap.SetTile(new Vector3Int(x, y, 0), ground);
            }
        }
    }

    //Places obstacles on newly generated platforms
    public void GenerateObstacles(GameObject newPlatform) {

        try {

            //i keeps track of x value of new obstacles
            int i = Random.Range(0, groundObstacleGapRange.y + 1);

            while (i < platformWidth) {
                
                if (Random.Range(0f, 100f) < cactusChance) { //Spawn cacti

                    int numCacti = Random.Range(numCactiPerGroup.x, numCactiPerGroup.y + 1);
                    int j = 0;

                    while (j < numCacti) {

                        GameObject obstacle = cactiList[Random.Range(0, cactiList.Length)];
                        float obstacleHeight = obstacle.GetComponent<SpriteRenderer>().bounds.extents.y;

                        if (j == 0) {
                            i += (int)(obstacleHeight * 2);
                        }

                        if ((obstacleHeight * 2) + i >= platformWidth) {
                            return;
                        }

                        GameObject cactus = Instantiate(obstacle, new Vector2(newPlatform.transform.position.x + i + .5f, newPlatform.transform.position.y + platformHeight + obstacleHeight), Quaternion.identity);
                        cactus.transform.SetParent(newPlatform.transform);

                        if (Random.value > .5f) {
                            cactus.transform.localScale = new Vector3(-1, 1, 1);
                        }

                        j++;
                        i++;
                    }

                } else { //Spawn other obstacles

                    GameObject obstacle = otherObstaclesList[Random.Range(0, otherObstaclesList.Length)];
                    
                    if (obstacle.name.Equals("Rib")) { //Generate ribcage

                        int numRibs = Random.Range(numRibsPerCage.x, numRibsPerCage.y + 1);

                        if (i + numRibs + 1 <= platformWidth) {

                            int j = 0;
                            GameObject ribcage = new GameObject("Ribcage");

                            while (j < numRibs) {

                                GameObject rib = Instantiate(obstacle, new Vector3(newPlatform.transform.position.x + i + .5f, newPlatform.transform.position.y + platformHeight + 1, -2), Quaternion.identity);
                                GameObject rib2 = Instantiate(obstacle, new Vector3(newPlatform.transform.position.x + i + 1.5f, newPlatform.transform.position.y + platformHeight + 1, 2), Quaternion.identity);
                                rib.transform.SetParent(ribcage.transform);
                                rib2.transform.SetParent(ribcage.transform);
                                //rib2.GetComponent<SpriteRenderer>().sortingOrder = -1;
                                rib2.transform.localScale = new Vector3(-1, 1, 1);
                                i++;
                                j++;
                            }

                            ribcage.transform.SetParent(newPlatform.transform);
                        }

                    } else if (obstacle.name.Contains("Scorpion")) { //Generate scorpions
                                                                     
                        if (i > 4) { //Minimum distance so scorpion doesn't walk off the edge
                            Instantiate(obstacle, new Vector3(newPlatform.transform.position.x + i + .5f, newPlatform.transform.position.y + platformHeight + .5f, -1), Quaternion.identity);
                        }
                        i++;

                    } else { //Generate any other obstacle

                        SpriteRenderer sprite = obstacle.GetComponent<SpriteRenderer>();
                        float obstacleHeight = sprite.bounds.extents.y;
                        float obstacleWidth = sprite.bounds.extents.x;
                        i += (int)(obstacleHeight * 2);

                        if ((obstacleHeight * 2) + obstacleWidth + i >= platformWidth) {
                            return;
                        }

                        GameObject groundObstacle = Instantiate(obstacle, new Vector2(newPlatform.transform.position.x + i + obstacleWidth, newPlatform.transform.position.y + platformHeight + obstacleHeight), Quaternion.identity);
                        groundObstacle.transform.SetParent(newPlatform.transform);
                        i += (int)((obstacleWidth + obstacleHeight) * 2);
                    }
                }

                //Add a gap between obstacle groups
                i += Random.Range(groundObstacleGapRange.x, groundObstacleGapRange.y + 1);
            }

        } catch (System.Exception e) {
            Debug.Log("Error in GameController.GenerateObstacles: " + e.Message);
        }
    }

    public void SpawnPowerup(GameObject platform) {
        try {

            Sprite powerupSprite = powerupSpriteList[Random.Range(0, powerupSpriteList.Length)];

            GameObject powerupObject = powerupPrefab;
            powerupObject.name = powerupSprite.name;
            powerupObject.GetComponent<SpriteRenderer>().sprite = powerupSprite;
            powerupObject = Instantiate(powerupObject, platform.transform);

            float newX = platform.transform.position.x + platformWidth + .5f;
            float newY = platform.transform.position.y + platformHeight + .5f;
            powerupObject.transform.position = new Vector2(newX, newY);

            spawnPowerup = false;

        } catch (System.Exception e) {
            Debug.Log("Error in GameController.SpawnPowerup: " + e.Message);
        }
    }

    //Fires when player dies
    public void GameOver() {
        //StopAllCoroutines();
        gameOverScreen.SetActive(true);
    }

    //Restarts game
    public void ReloadScene() {
        SceneManager.LoadScene("Main");
    }
}