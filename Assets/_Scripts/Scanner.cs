using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy Detected");
            other.gameObject.GetComponent<Highlighter>().Detected();
        }
    }
}
