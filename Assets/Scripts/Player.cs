using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    //Define delegate types and events here

    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }

    [SerializeField] private float speed = 4;
    private bool moving = false;
    private Vector3 currentDir;

    bool canGoFwd; 
    bool canGoBkwd; 
    bool canGoLeft; 
    bool canGoRight; 

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
        MouseInput();
        if (moving == false)
        {
            FindNodes();
            //Implement inputs and event-callbacks here
            if (Input.GetButton("Horizontal"))
            {
                if(Input.GetAxis("Horizontal") > 0 && canGoRight)
                {
                    Debug.Log("Right");
                    MoveToNode(TargetNode);
                }
                if (Input.GetAxis("Horizontal") < 0 && canGoLeft)
                {
                    Debug.Log("Left");
                }
            }
            if(Input.GetButton("Vertical"))
            {
                if (Input.GetAxis("Vertical") > 0 && canGoFwd)
                {
                    Debug.Log("Up");
                }
                if (Input.GetAxis("Vertical") < 0 && canGoBkwd)
                {
                    Debug.Log("Down");
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

    //Implement mouse interaction method here
    public void MouseInput()
    {
        // if object in UI is tagged button
        
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

    private void FindNodes()
    {
        //RaycastHit hit;
        //Node node;

        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 bkwd = transform.TransformDirection(Vector3.back);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 left = transform.TransformDirection(Vector3.left);

        if(Physics.Raycast(transform.position, fwd, 10))
        {
            Debug.Log("Thing infront");
            canGoFwd = true;
        }
        else
        {
            canGoFwd = false;
        }

        if(Physics.Raycast(transform.position, bkwd, 10))
        {
            Debug.Log("Thing behind");
            canGoBkwd = true;
        }
        else
        {
            canGoBkwd = false;
        }

        if (Physics.Raycast(transform.position, right, 10))
        {
            Debug.Log("Thing right");
            canGoRight = true;
        }
        else
        {
            canGoRight = false;
        }

        if (Physics.Raycast(transform.position, left, 10))
        {
            Debug.Log("Thing left");
            canGoLeft = true;
        }
        else
        {
            canGoLeft = false;
        }
    }
}
