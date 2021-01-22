using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PLayer_Controller : MonoBehaviour
{
    public float velocity;
    public NavMeshAgent agent;
    Joystick player_controller;
    Rigidbody rb;
    Vector3 target;
    
    private void Start()
    {
        player_controller = FindObjectOfType<Joystick>();
        rb = transform.GetComponent<Rigidbody>();
    }
    void Update()
    {
        float x = player_controller.Horizontal;
        float z = player_controller.Vertical;
        
        var movement = new Vector3(x,0,z) * velocity;
        
        rb.velocity = movement;   
    }

    public void GOTO(Vector3 pos)
    {    
        agent.enabled = true;
        agent.SetDestination(pos);                
    }

    public void Agent_TURNOFF()
    {
        agent.enabled = false;
    }
}
