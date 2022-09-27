using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public Rigidbody bullet;
    public float bulletspeed = 20f;
   

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Fire()
    {
        Rigidbody newBullet = Instantiate(bullet, transform.position, transform.rotation);
        newBullet.velocity = transform.forward * bulletspeed;
    }
}
