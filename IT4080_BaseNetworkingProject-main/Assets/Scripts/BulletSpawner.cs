using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletSpawner : NetworkBehaviour
{
    public Rigidbody bullet;
    public float bulletspeed = 20f;
   

    [ServerRpc]
    public void FireServerRpc()
    {
        Rigidbody newBullet = Instantiate(bullet, transform.position, transform.rotation);
        newBullet.velocity = transform.forward * bulletspeed;
        newBullet.gameObject.GetComponent<NetworkObject>().Spawn();
        Destroy(newBullet, 2);
    } 
  
}
