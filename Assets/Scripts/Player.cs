using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private AT01_UnityProjectActions _input;

    private Vector2 inputDirections;
    
    //Define delegate types and events here

    public Buttons up, down, left, right;
    

    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }

    [SerializeField] private float speed = 4;
    public bool moving = false;
    private Vector3 currentDir;

    private void OnEnable()
    {
        if (_input == null)
        {
            _input = new AT01_UnityProjectActions();
            _input.Player.Move.performed += _input => inputDirections = _input.ReadValue<Vector2>();
            //_input.Player.Move.performed += SetDirection;
        }
        _input.Player.Enable();
    }

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

       

        //_input.Player.Move.performed += SetDirection;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
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
        else if(!moving)
        {
            //Debug.Log("not moving");
             SetDirection();
        }
       
    }

    private void SetDirection()
    {

        Debug.Log("Dir is set");
        float xDir = inputDirections.x;
        float yDir = inputDirections.y;
        if (!moving)
        {
            if (yDir > 0)
            {
                StartCoroutine(up.Green());
                FindNodes(transform.forward);
            }
            if (yDir < 0)
            {
                StartCoroutine(down.Green());
                FindNodes(-transform.forward);
            }
            if (xDir > 0)
            {
                StartCoroutine(right.Green());
                FindNodes(transform.right);
            }
            if (xDir < 0)
            {
                StartCoroutine(left.Green());
                FindNodes(-transform.right);

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

    private void OnDisable()
    {
        _input.Player.Disable();
    }
}
