using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public float speed = 5f;
     
    private void Update()
    {
        transform.position += -transform.forward * Time.deltaTime * speed;
    }
}
