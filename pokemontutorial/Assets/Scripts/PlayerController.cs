using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;

    private Animator animator;

    private Vector2 input;

    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //prevents diaganol movement
            if (input.x != 0)
            {
                input.y = 0;
            }

            if (input != Vector2.zero)
            {
                //set values of animator
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(new Vector3(targetPos.x - 0.35f, targetPos.y - 0.55f)))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;

        CheckForEncounters(targetPos);
    }



    private bool IsWalkable(Vector3 targetPos)
    {
        //checks to see if solidobjects variable, pulled from LayerMask from interface to check if it is part of that layer
       return Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) == null ? true : false;

    }

    private void CheckForEncounters(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if(Random.Range(1, 101) <= 90)
            {
                Debug.Log("Encountered a wild pokemon");
            }
        }
    }

}
