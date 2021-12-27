using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    CharacterAnimator animator;
    public bool IsMoving { get; private set; }

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver=null)
    {
        //set values of animator
        animator.MoveX =  Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY =  Mathf.Clamp(moveVec.y, -1f, 1f);

        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y +=  moveVec.y;

        if (!IsPathClear(targetPos))
            yield break;

        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;

        IsMoving = false;

        OnMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;   
    }

    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position;
        var dir = diff.normalized;//length of 1, but direction

        return !Physics2D.BoxCast(transform.position + dir, new Vector2(0.2f, 0.2f), 0f, diff.normalized, diff.magnitude - 1, GameLayers.i.SolidLayer | GameLayers.i.InteractablesLayer | GameLayers.i.PlayerLayer);
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        //checks to see if solidobjects variable, pulled from LayerMask from interface to check if it is part of that layer
        return Physics2D.OverlapCircle(targetPos, 0.1f, GameLayers.i.SolidLayer | GameLayers.i.InteractablesLayer | GameLayers.i.PlayerLayer) == null ? true : false;

    }


    public void LookTowards(Vector3 targetPos)
    {
        var xDiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var yDiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        if(xDiff == 0 || yDiff == 0)
        {
            animator.MoveX = Mathf.Clamp(xDiff, -1f, 1f);
            animator.MoveY = Mathf.Clamp(yDiff, -1f, 1f);
        }
        else
        {
            Debug.LogError("Error in move, x or y should be 0");
        }
    }

    public CharacterAnimator Animator
    {
        get => animator;
    }
}
