using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Buttons : MonoBehaviour, IPointerClickHandler
{
    //Define the things
    public Player playerScript; //This links to the player script
    [SerializeField] Vector3 buttonDirection; //This determines the direction the button will lead
     private bool canPress = false; //This is to make sure the player can go this way
    public Image thisImage;

    private void Start()
    {
        thisImage.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if player is moving
        if(!playerScript.moving)
        {
            //Debug.Log("Player isnt moving");
            StartCoroutine(SearchForNodes(buttonDirection));
        }
                
        if(!canPress)
        {
            //Debug.Log("grEy");
            thisImage.color = Color.grey;
        }
    }

    IEnumerator SearchForNodes(Vector3 dir)
    {
        //Debug.Log("Coroute started");

        //Define the raycast
        RaycastHit hit;
        Ray ray = new Ray(playerScript.transform.position, playerScript.transform.TransformDirection(dir));

        //Send the rays
        if (Physics.Raycast(ray, out hit, 10))
        {
            //Check if it hit a node
            if (hit.collider.gameObject.TryGetComponent<Node>(out Node node))
            {
                //Debug.Log("node in direction");
                //if it did, the button can be pressed
                canPress = true;
                thisImage.color = Color.white;
                //Debug.Log("can go " + dir);
            }
            else
            {
                ///Debug.Log("not node in direction");
                canPress = false;
            }
        }
        else
        {
            //Debug.Log("nothing in direction");
            canPress = false;
        }
        //end the routine
        yield return null;
    }

    //On Click
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if(canPress)
        {
            playerScript.FindNodes(buttonDirection);
            StartCoroutine(Green());
        }
    }

    public IEnumerator Green()
    {
        thisImage.color = Color.green;

        yield return new WaitForSeconds(0.1f);

        thisImage.color = Color.white;
    }
}
