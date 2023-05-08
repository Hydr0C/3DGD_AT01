using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    //Define delegate types and events here

    public Buttons up, down, left, right;
    

    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }

    [SerializeField] private float speed = 4;
    public bool moving = false;
    private Vector3 currentDir;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Node node in GameManager.Instance.Nodes)
        {
            if(node.Parents.Length > 2 && node.Children.Length == 0)
            {
                CurrentNode = node;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving == false)
        {
            //Implement inputs and event-callbacks here
            if (Input.GetButton("Horizontal"))
            {
                if(Input.GetAxis("Horizontal") > 0)
                {
                    StartCoroutine(right.Green());
                    FindNodes(transform.right);
                }
                if (Input.GetAxis("Horizontal") < 0)
                {
                    StartCoroutine(left.Green());
                    FindNodes(-transform.right);
                    
                }
            }
            if(Input.GetButton("Vertical"))
            {
                if (Input.GetAxis("Vertical") > 0)
                {
                    StartCoroutine(up.Green());
                    FindNodes(transform.forward);
                }
                if (Input.GetAxis("Vertical") < 0)
                {
                    StartCoroutine(down.Green());
                    FindNodes(-transform.forward);
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, TargetNode.transform.position) > 0.25f)
            {
                transform.Translate(currentDir * speed * Time.deltaTime);
            }
            else
            {
                moving = false;
                CurrentNode = TargetNode;
            }
        }
    }

    /// <summary>
    /// Sets the players target node and current directon to the specified node.
    /// </summary>
    /// <param name="node"></param>
    public void MoveToNode(Node node) 
    {
        if (moving == false)
        {
            TargetNode = node;
            currentDir = TargetNode.transform.position - transform.position;
            currentDir = currentDir.normalized;
            moving = true;
        }
    }

    public void FindNodes(Vector3 dir) //Takes in a variable that determines what direction everything will go
    {
        //Define the raycast
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.TransformDirection(dir));

        if(Physics.Raycast(ray, out hit, 10)) //Checks if the raycast hit something within 10 units
        {
            if(hit.collider.gameObject.TryGetComponent<Node>(out Node node)) //Checks if the thing hit was a node
            {
                TargetNode = node; //Sets the target to the node the raycast hit
                MoveToNode(TargetNode); //starts the MoveToNode function
            }
        }
    }
}
