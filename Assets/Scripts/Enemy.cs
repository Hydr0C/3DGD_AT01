using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed modifier.")]
    [SerializeField] private float speed = 3;
    private Node currentNode;
    private Vector3 currentDir;
    private bool playerCaught = false;

    public delegate void GameEndDelegate();
    public event GameEndDelegate GameOverEvent = delegate { };

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
                //else
                //{FindPath();}
                //Implement path finding here
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

    /// <summary>
    /// Sets the current node to the first in the Game Managers node list.
    /// Sets the current movement direction to the direction of the current node.
    /// </summary>
     
    ///<findPathPsuedo>
    ///private void FindPath()
    ///{
    ///access gmae manager
    ///add nodes
    ///add GameManager.Instance.Nodes[0] onto list of unsearched nodes
    ///check if node = GameManager.Instacne.Player.TargetNode/CurrentNode
    ///if same that is destination
    ///else add children to list
    ///remove checked node
    ///assign most recently added node to checking
    ///repeat from 72
    ///} 
    /// </findPathPsuedo>

    void InitializeAgent()
    {
        currentNode = GameManager.Instance.Nodes[0];
        currentDir = currentNode.transform.position - transform.position;
        currentDir = currentDir.normalized;
    }

    //Implement DFS algorithm method here


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
    /// if list empty, break
    /// 1. take last item in list, assign to item being searched
    /// 2. check if node is either
    ///    - Target node of player
    ///    - Current node of player
    /// if true, 
    ///    - assign node being searched to current node
    ///    - target found = true
    ///    
    /// if false, 
    ///    - continue
    ///    
    /// 3. Use for loop to add each child of node to unsearched node list
    /// 4. Remove node being searched from unsearched node list
    /// 5. Return to start of list
    /// 
    /// </pathfinder2>
}
