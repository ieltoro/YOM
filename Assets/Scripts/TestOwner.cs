using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOwner : MonoBehaviour
{
    [SerializeField] float speed;
    private void Update()
    {
        transform.position = transform.position + new Vector3(0, 0, speed * Time.deltaTime);
    }

}
