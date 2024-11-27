using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Font customFont;
    public int score = 0;
    public bool isGameOver = false, isWin = false;
    [SerializeField] private List<GameObject> orbs = new List<GameObject>();
    private float orbDistance = Mathf.Infinity, minDistance = Mathf.Infinity;
    [SerializeField] private GameObject player;

    public void CheckScore()
    {
        if(score == 5)
        {
            isWin = true;
        }
    }

    void Update()
    {
        NearestOrb();
    }

    private void NearestOrb()
    {
        minDistance = Mathf.Infinity;
        foreach(GameObject orb in orbs)
        {
            if(orb != null)
            {
                float distance = Vector3.Distance(orb.transform.position, player.transform.position);
                if(distance < minDistance)
                {
                    minDistance = distance;
                }
            }
            else
            {
                orbs.Remove(orb);
                break;
            }
        }
        orbDistance = minDistance;
    }

    private void OnGUI()
    {
        GUIStyle headStyle = new GUIStyle();
        GUIStyle scoreStyle = new GUIStyle();
        headStyle.font = customFont;
        scoreStyle.font = customFont;
        if(isGameOver)
        {
            headStyle.fontSize = 200; 
            headStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(Screen.width / 2-500, Screen.height / 2-250, 1000, 500), "Game Over", headStyle);
            scoreStyle.fontSize = 100; 
            scoreStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(Screen.width / 2-450, Screen.height / 2, 1000, 500), "Orbs Collected: " + score + "/5", scoreStyle);
        }
        else if(isWin)
        {
            headStyle.fontSize = 200; 
            headStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(Screen.width / 2-500, Screen.height / 2-250, 1000, 500), "You Win!", headStyle);
        }
        else
        {
            scoreStyle.fontSize = 100; 
            scoreStyle.normal.textColor = Color.white;
            GUI.Label(new Rect(10, 10, 100, 50), "Orbs: " + score + "/5", scoreStyle);
            GUI.Label(new Rect(10, 100, 100, 50), "Nearest Orb: " + Math.Ceiling(orbDistance) + "m", scoreStyle);
        }
        
    }

}
