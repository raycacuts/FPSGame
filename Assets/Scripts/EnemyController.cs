using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private PlayerController player;

    public float moveSpeed = 3f;
    public Rigidbody theRB;

    public float chaseRange = 15f, stopCloseRange = 4f;

    private float strafeAmount;

    public Animator anim;

    public Transform[] patrolPoints;
    [HideInInspector]
    public int currentPatrolPoint = 0;

    public Transform pointsHolder;
    public float pointWaitTime = 15f;
    private float waitCounter;

    private bool isDead;
    public float currentHealth = 35f;
    public float startingHealth = 35f;

    public float waitToDisappear = 4f;

    public Transform shootPoint;
    public EnemyProjectile projectile;
    public float timeBetweenShots = .2f;
    private float shotCounter;
    public float shotDamage = 10f;

    public bool splitOnDeath;
    public float minSize = .4f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();

        strafeAmount = Random.Range(-.75f, .75f);
        theRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        pointsHolder.SetParent(null);
        waitCounter = Random.Range(.75f, 1.25f) * pointWaitTime;
        shotCounter = timeBetweenShots;

        currentHealth = startingHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            waitToDisappear -= Time.deltaTime;
            if(waitToDisappear <= 0)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.deltaTime);

                if(transform.localScale.x == 0)
                {
                    Destroy(gameObject);

                    if(!splitOnDeath)
                    {
                        Destroy(pointsHolder.gameObject);
                    }
                    
                }
            }

            return;
        }
        float yStore = theRB.velocity.y;
        float distance = Vector3.Distance(transform.position, player.transform.position);

   

        if (distance < chaseRange && !PlayerController.instance.isDead)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

            if(distance > stopCloseRange)
            {
                theRB.velocity = (transform.forward + (transform.right * strafeAmount)) *  moveSpeed;
                anim.SetBool("moving", true);
            }
            else
            {
                theRB.velocity = Vector3.zero;
                anim.SetBool("moving", false);
            }
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                shootPoint.LookAt(player.cam.transform.position);
                EnemyProjectile newProjectile = Instantiate(projectile, shootPoint.position, shootPoint.rotation);
                newProjectile.damageAmount = shotDamage;
                newProjectile.transform.localScale = transform.localScale;

                shotCounter = timeBetweenShots;
                anim.SetTrigger("shooting");
                AudioManager.instance.PlaySFX(2);
            }
            
        }
        else
        {
            if(patrolPoints.Length > 0)
            {
                if(Vector3.Distance(transform.position, new Vector3(patrolPoints[currentPatrolPoint].position.x, transform.position.y, patrolPoints[currentPatrolPoint].position.z)) < 1f)
                {

                    waitCounter -= Time.deltaTime;
                    theRB.velocity = Vector3.zero;
                    anim.SetBool("moving", false);

                    if(waitCounter <= 0)
                    {
                        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
                        waitCounter = Random.Range(.75f, 1.25f) * pointWaitTime;
                    }
                    
                 
                }
                else
                {
                    transform.LookAt(new Vector3(patrolPoints[currentPatrolPoint].position.x, transform.position.y, patrolPoints[currentPatrolPoint].position.z));
                    theRB.velocity = transform.forward * moveSpeed;
                    anim.SetBool("moving", true);
                }
                
            }
            else
            {
                theRB.velocity = Vector3.zero;
                anim.SetBool("moving", false);
            }
            
        }

        theRB.velocity = new Vector3(theRB.velocity.x, yStore, theRB.velocity.z);
        //Debug.Log("velocity = " + theRB.velocity.magnitude);
        
    }
    public void TakeDamage(float damageToTake)
    {
        currentHealth -= damageToTake;
        if(currentHealth <= 0)
        {
            if(splitOnDeath)
            {
                if(transform.localScale.x > minSize)
                {
 
                    startingHealth *= .75f;
                    shotDamage *= .75f;
                    moveSpeed *= .75f;
                    currentHealth = startingHealth;

                    GameObject clone1 = Instantiate(gameObject, transform.position + (transform.right * .5f * transform.localScale.x), Quaternion.identity);
                    GameObject clone2 = Instantiate(gameObject, transform.position + (-transform.right * .5f * transform.localScale.x), Quaternion.identity);

                    clone1.transform.localScale = transform.localScale * .75f;
                    clone2.transform.localScale = transform.localScale * .75f;
                }
                
            }
            anim.SetTrigger("die");
            isDead = true;

            theRB.velocity = Vector3.zero;
            theRB.isKinematic = true;

            GetComponent<Collider>().enabled = false;
        }
        

        //Destroy(gameObject);
    }

}
