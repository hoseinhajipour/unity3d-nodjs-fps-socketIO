using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{

    public Transform firepos;
    public Rigidbody bullet;
    public string guntype;
    public float bulletSpeed = 10;
    public bool active = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && active==true)
        {
            Rigidbody bulletClone =  Instantiate(bullet, firepos.position, firepos.rotation) as Rigidbody;
            bulletClone.velocity = transform.forward * bulletSpeed;

            GameObject.Find("SocketIO").GetComponent<connection>().Shooting(firepos, firepos.rotation, guntype, bulletSpeed);
        }
    }
   
}
