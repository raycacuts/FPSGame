using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    public float moveSpeed = 20f;
    public float damageAmount = 1f;
    public Rigidbody theRB;

    public GameObject impactEffect, damageEffect;

    public float lifeTime = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        theRB.velocity = transform.forward * moveSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Instantiate(damageEffect, transform.position, Quaternion.identity);

            PlayerHealthController.instance.TakeDamage(damageAmount);
        }
        else
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
           Destroy(gameObject);
    }
}
