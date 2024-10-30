using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Seeker : MonoBehaviour
{

    [SerializeField] private GameObject player;
    public PlayerScript pScript;
    public Animator animator;
    private Collider enemyCollider;

    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float turnSpeed = 1f;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        enemyCollider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Player") == true)
        {
          pScript.speed = 0f;
          pScript.sensitivity = 0f;
          animator.SetBool("Dead", true);
          enemyCollider.enabled = false;
          gameHandler.isGameOver = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
        //rotationHandle(player.transform.position);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void rotationHandle(Vector3 GO)
    {
        Quaternion _lookRotation = Quaternion.LookRotation((GO - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * turnSpeed);
    }

    
}
