using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("bulletPosition").GetComponent<Transform>();
        Destroy(gameObject,2.0f);
        GetComponent<Rigidbody>().AddForce(playerTransform.forward * 20f,ForceMode.Impulse);
    }
}
