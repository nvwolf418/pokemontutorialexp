using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;

    public LayerMask solidObjectsLayer;
    public LayerMask interactablesLayer;
    public LayerMask grassLayer;

    public event Action OnEncountered;

    private CharacterAnimator animator;

    private Vector2 input;



    public void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    public void handleUpdate()
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
                animator.MoveX = input.x;
                animator.MoveY = input.y;

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(new Vector3(targetPos.x - 0.35f, targetPos.y - 0.55f)))
                {
                    StartCoroutine(Move(targetPos));
                }
            }
        }

        animator.IsMoving = isMoving;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }


    }


    public void Interact()
    {
        //get direction
        var facingDir = new Vector3(animator.MoveX, animator.MoveY);
        var interactPos = transform.position + facingDir;

        Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

        //see if object from interactable layer in front
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactablesLayer);

        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
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
       return Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer | interactablesLayer) == null ? true : false;

    }

    private void CheckForEncounters(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null)
        {
            if(UnityEngine.Random.Range(1, 101) <= 90)
            {
                animator.IsMoving = false;
                OnEncountered();
            }
        }
    }

}
