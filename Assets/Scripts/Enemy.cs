using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed modifier.")]
    [SerializeField] private float speed = 3;
    private Node currentNode;
    private Vector3 currentDir;
    private bool playerCaught = false;
    public Player player;
    public Node node;

    public delegate void GameEndDelegate();
    public event GameEndDelegate GameOverEvent = delegate { };


    private bool safetyBreak = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeAgent();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCaught == false)
        {
            if (currentNode != null)
            {
                //If within 0.25 units of the current node.
                if (Vector3.Distance(transform.position, currentNode.transform.position) > 0.25f)
                {
                    transform.Translate(currentDir * speed * Time.deltaTime);
                }
                else
                {
                    if (safetyBreak == false)
                    {
                        safetyBreak = true;
                        DFSearch();

                    }
                }
            }
            else
            {
                Debug.LogWarning($"{name} - No current node");
            }

            Debug.DrawRay(transform.position, currentDir, Color.cyan);
        }
    }

    //Called when a collider enters this object's trigger collider.
    //Player or enemy must have rigidbody for this to function correctly.
    private void OnTriggerEnter(Collider other)
    {
        if (playerCaught == false)
        {
            if (other.tag == "Player")
            {
                playerCaught = true;
                GameOverEvent.Invoke(); //invoke the game over event
            }
        }
    }

    void InitializeAgent()
    {
        currentNode = GameManager.Instance.Nodes[0];
        currentDir = currentNode.transform.position - transform.position;
        currentDir = currentDir.normalized;
    }

    //Implement DFS algorithm method here

    private void DFSearch()
    {
        //Debug.Log("Pathfinding has started");

        //Set the required variables
        Node searchingNode;
        bool targetFound;
        Node playerCurrent;
        Node playerTarget;

        //Define the vraiables
        targetFound = false;

        searchingNode = GameManager.Instance.Nodes[0];

        List<Node> theStack = new List<Node> { searchingNode };
        List<Node> children = new List<Node> { };

        playerCurrent = player.CurrentNode;
        playerTarget = player.TargetNode;


        // Create the Loop
        while (targetFound == false)
        {

            // Ensure there are nodes in the list
            if (theStack.Count == 0)
            {
                //If none, break the loop
                break;
            }
            //Set the node being searched to the last node in the list
            searchingNode = theStack[theStack.Count() - 1];

            // Check if the current node being searched is the required node
            if (searchingNode == playerCurrent | playerTarget)
            {
                currentNode = searchingNode;
                currentDir = currentNode.transform.position - transform.position;
                currentDir = currentDir.normalized;
                targetFound = true;
                break;
            }
            else // Add the node's children to the list
            {
                if (searchingNode.Children.Count() > 0)
                {
                    children.AddRange(searchingNode.Children);
                    foreach (Node child in children)
                    {
                        theStack.Add(child);
                    }
                }      
                
            }
            //Remove the searched node from the list
            theStack.Remove(searchingNode);
        }
    }

      
    ///<pathfinder2>
    ///Variable for node being searched
    ///Bool for target found
    ///list of nodes for unsearched nodes (the stack)
    ///
    /// Set target found to false
    /// Use game manager.instance.nodes[0] assign to unsearched nodes list
    /// 
    /// start LOOP
    /// While target found is false -> go
    /// 
    /// if list empty, break
    /// 1. take last item in list, assign to item being searched
    /// 
    /// 
    /// 2. check if node is either
    ///    - Target node of player
    ///    - Current node of player
    ///    
    /// if true, 
    ///    - assign node being searched to current node
    ///    - target found = true
    ///    
    /// if false, 
    ///    - continue
    /// 
    /// 3. Use for loop to add each child of node to unsearched node list
    /// 
    /// 4. Remove node being searched from unsearched node list
    /// 5. Return to start of list
    /// 
    /// </pathfinder2>
}
