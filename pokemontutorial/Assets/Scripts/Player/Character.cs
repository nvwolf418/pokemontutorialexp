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

        if (!IsWalkable(targetPos))
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

    private bool IsWalkable(Vector3 targetPos)
    {
        //checks to see if solidobjects variable, pulled from LayerMask from interface to check if it is part of that layer
        return Physics2D.OverlapCircle(targetPos, 0.1f, GameLayers.i.SolidLayer | GameLayers.i.InteractablesLayer) == null ? true : false;

    }

    public CharacterAnimator Animator
    {
        get => animator;
    }
}
