using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform[] ammoPoints;

    public GameObject ammoPickup;
    public GameObject healthPickup;
    
    public float ammoSpawnTime;
    private float ammoCounter;

    public float checkInterval;
    private float checkCounter;

    public GameObject levelExit;

    // Start is called before the first frame update
    void Start()
    {
        ammoCounter = ammoSpawnTime;
        checkCounter = checkInterval;

        AudioManager.instance.PlayBossMusic();
    }

    // Update is called once per frame
    void Update()
    {
        checkCounter -= Time.deltaTime;
        if(checkCounter <= 0)
        {
            checkCounter = checkInterval;
            if(FindFirstObjectByType<EnemyController>() == null)
            {
                gameObject.SetActive(false);
                levelExit.SetActive(true);

                AudioManager.instance.PlayLevelMusic();
            }
        }
        ammoCounter -= Time.deltaTime;
        if(ammoCounter <= 0)
        {
            ammoCounter = ammoSpawnTime;
            int value = UnityEngine.Random.Range(0, 2);
            if (value == 0)
                Instantiate(ammoPickup, ammoPoints[Random.Range(0, ammoPoints.Length)].position, Quaternion.identity);
            else
                Instantiate(healthPickup, ammoPoints[Random.Range(0, ammoPoints.Length)].position, Quaternion.identity);
        }
    }
}
