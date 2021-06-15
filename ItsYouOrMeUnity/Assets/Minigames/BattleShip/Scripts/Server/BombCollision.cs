using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCollision : MonoBehaviour
{
    [SerializeField] ParticleSystem splash, waterRing;
    [SerializeField] Rigidbody rb;

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * rb.mass);
    }

    private void OnTriggerEnter(Collider other)
    {
        splash.Play();
        waterRing.Play();
        StartCoroutine(DestroyObj());
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(2);
        Destroy(transform.parent.gameObject);
    }

}
