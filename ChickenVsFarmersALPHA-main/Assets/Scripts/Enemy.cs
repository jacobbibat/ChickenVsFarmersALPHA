using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static float speed = 11.0f;
    private Rigidbody enemyRb;
    private GameObject player;

    public GameObject experiencePoints;
    public GameObject rareItem;
    private float rareDr = 0.2f; //originally 0.1f, but for ALPHA purposes

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized; 
        enemyRb.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void OnDestroy()
    {
        Debug.Log("Enemy Destroyed");
        Instantiate(experiencePoints, transform.position, transform.rotation);

        float randomValue = Random.value;

        if (randomValue <= rareDr)
        {
            Instantiate(rareItem, transform.position, transform.rotation);
        }
    }
}