using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    //until i finish the project, the variables have to be public, so the FOVEditor script can access them
    
    public float radius;
    [Range(0,360)] //the enemy can't have too big vision range, cause it's just a human
    public float angle = 140;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool playerDetection;

    [SerializeField]
    Transform sphereTarget;

    [SerializeField]
    Transform warningTarget;

    private GameObject sphere;
    private bool sphereCheck = false;

    [SerializeField]
    GameObject warningObject;

    Animator warningAnimator;

    [SerializeField]
    [Range(0,200)]
    public float sphereCheckRadius = 74;

    [SerializeField]
    float sphereHeight;

    [SerializeField]
    float warningHeight = 14;

    [SerializeField]
    float detectionTime;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        warningAnimator = warningObject.GetComponent<Animator>();

        StartCoroutine(FOVRoutine());
        StartCoroutine(SRoutine());
    }

    private void Update()
    {
        SpherePosition();
    }

    //coroutine, so it is less performance heavy - it searches for a player every Nf seconds, instead of the whole time
    private IEnumerator FOVRoutine()
    {
        
        //this method only works inside the coroutine
        WaitForSeconds wait = new WaitForSeconds(0.6f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
            PlayerDetection();
        }
    }

    private IEnumerator SRoutine() //TODO: there should appear only 1 sphere when the player is near, and it should disappear when the player is out of range
    {
        WaitForSeconds wait = new WaitForSeconds(0.6f);


        while (true)
        {
            yield return wait;
            PlayerProximityCheck();
        }
    }

    private IEnumerator WRoutine(float time)
    {
        //warningAnimator.SetBool("isTimeUp", false);
        yield return new WaitForSeconds(time);
        warningAnimator.SetBool("isTimeUp", true);
    }

    private IEnumerator WaitBeforeAlert(float time)
    {
        yield return new WaitForSeconds(time);

        warningAnimator.SetBool("alert", true);
    }

    private void FieldOfViewCheck() //NOTE TO MYSELF IN FUTURE - NIE BÓJ SIÊ ZAJRZEÆ DO VIDEO GOŒCIA PRZY OPISYWANIU PRACKI - sprawdŸ plik "links for sources"
    {

        //don't care what type of collider will be found, it is only going to be known, that it is going to be a collider - it will be looking for a player
        //we're starting this sphere from the center of our enemy (transform.position) (NOTE TO MYSELF: the table in Unity that holds this information in Inspector is called Transform), it will look on the given LayerMask - which in this case is the layer of our player
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            //we take the first instance that it will get, because OverlapSphere returns an array, and we know that there's only player on that layer - pretty clear
            Transform target = rangeChecks[0].transform;
            //the direction from where the enemy is looking to where the player is | normalized to get a value between 0 and 1
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            //checking for the angle | "from" position -> "to" position | transform.position is the current position of our enemy
            // / 2 because we want to narrow it down to make a detailed angle check
            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                //FINAL RAYCAST TO DETERMINE IF WE CAN ACTUALLY SEE THE PLAYER
                //starting the raycast from the center of our enemy (I may change it up a little, so it does it from the eyes, or maybe I'll put this script on a certain object on the head, so it doesn't matter)
                //aim the raycast towards the player | limmiting the raycast to the distance to the target, stop the raycast if it hits anything that obstructs a view
                //its a positive check, if this fails, it doesn't hit any obstructionMask, it CAN see the player
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    //Debug.Log(1);
                    playerDetection = true;
                }
                else playerDetection = false;
            }
            else
                playerDetection = false;
        }
        //if the playere was in the view of the enemy and it no longer is - I need to make sure that the enemies "remember" that
        //if it sees the player, but it isn't in range, it "can't" see the player, thus setting the range
        else if (playerDetection)
        {
            playerDetection = false;
        }
    }
    
    private void PlayerProximityCheck()
    {
        //sphereCheck = false;

        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, sphereCheckRadius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            float maxDistanceToTarget = sphereCheckRadius;

            if (maxDistanceToTarget >= distanceToTarget && sphereCheck == false)
            {
                SphereGeneration();
            }
        }
        if (rangeChecks.Length == 0)
        {
            //Debug.Log(1);
            sphereCheck = false;
            Destroy(sphere);
        }

    }

    private void PlayerDetection()
    {
        warningAnimator.SetBool("isTimeUp", false);

        if (playerDetection == true)
        {
            warningAnimator.SetBool("playerDetection", true);
            StartCoroutine(WaitBeforeAlert(3.2f));

            if (warningAnimator.GetBool("alert"))
                FindObjectOfType<GameManager>().Detection();
        }
        else if (playerDetection == false)
        {
            warningAnimator.SetBool("playerDetection", false);
            warningAnimator.SetBool("alert", false);

            StartCoroutine(WRoutine(12f));
            

        }
    }
    
    object SphereGeneration()
    {
        Vector3 center = sphereTarget.position + Vector3.up * sphereHeight;
        sphere = (GameObject)Instantiate(Resources.Load("TransparentFOVSphere"), center, Quaternion.identity);
        sphereCheck = true;
        return sphereCheck;
    }

    private void SpherePosition()
    {
        if (sphere != null)
            sphere.transform.position = sphereTarget.position;
    }

    //private void WarningPosition()
    //{
    //    Vector3 aboveHead = sphereTarget.position + Vector3.up * warningHeight;

    //    if (warningObject != null)
    //    {
    //        warning.transform.position = aboveHead;
    //        warning.transform.LookAt(playerRef.transform);
    //    }
    //}
}
