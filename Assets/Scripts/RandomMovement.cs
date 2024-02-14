using System.Collections;
using UnityEngine;


public class RandomMovement : MonoBehaviour
{

    public Transform anchorRight;
    public Transform anchorLeft;
    public Transform destinationDebug;

    public Animator animator;
    public int directionState = 0;
    public float walkSpeed = 5f;
    public float horizontalDestination;
    public bool walk = false;
    public bool goRight = false;
    public bool canRandomizedDirection = true;
    public SpriteRenderer spriteRenderer;

    public Vector3 destination;

    private void Update()
    {
        if (walk)
        {
            animator.SetBool("isMoving", true);
            if (goRight)
            {
                if (transform.position.x < destination.x)
                {
                    transform.Translate(destination.normalized * walkSpeed * Time.deltaTime);
                }
                else
                {
                    StartCoroutine(StopPatrol());
                }
            }
            else
            {
                if (transform.position.x > destination.x)
                {
                    transform.Translate(destination.normalized * walkSpeed * Time.deltaTime);
                }
                else
                {
                    StartCoroutine(StopPatrol());
                }
            }
        }
    }

    private void Start()
    {
        pickDestination();
    }

    void pickDestination()
    {
        canRandomizedDirection = true;
        float dist = Vector3.Distance(transform.position, anchorRight.position);
        if (dist < 2)
        {
            Debug.Log("Trop proche de la droite => à gauche");
            goRight = false;
            canRandomizedDirection = false;
        }
        dist = Vector3.Distance(transform.position, anchorLeft.position);
        if (dist < 2)
        {
            Debug.Log("Trop proche de la gauche => à droite");
            goRight = true;
            canRandomizedDirection = false;
        }

        if (canRandomizedDirection)
        {   
            directionState = Random.Range(0, 100);
            if (directionState > 50)
            {
                goRight = true;
            }
            else
            {
                goRight = false;
            }
        }

        if (goRight)
        {
           spriteRenderer.flipX = false;
           horizontalDestination = Random.Range(transform.position.x + 2, anchorRight.position.x);
           destination = new Vector3(horizontalDestination, 0, 0);
        }
        else {
            spriteRenderer.flipX = true;
            horizontalDestination = Random.Range(transform.position.x - 2, anchorLeft.position.x);
            destination = new Vector3(horizontalDestination, 0, 0);
        }
        walk = true;
        destinationDebug.position = new Vector3(horizontalDestination, -2.17f,0);
        
    }

    IEnumerator StopPatrol()
    {
        walk = false;
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(Random.Range(0.2f, 2));
        pickDestination();
    }
}
