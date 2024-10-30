using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Points : MonoBehaviour
{
    [SerializeField] private GameHandler gameHandler;

    private void OnCollisionEnter(Collision collision) //This will run upon collision
    {
        if (collision.gameObject.tag == "Player")
        {
            gameHandler.score++; //Increases the score by 1
            gameHandler.CheckScore(); //Checks if the score is 5
            Destroy(gameObject);
        }
    }
}
