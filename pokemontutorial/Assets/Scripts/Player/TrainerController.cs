using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject exclamation;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public IEnumerator TriggerTrainerBattle(PlayerController player)
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        exclamation.SetActive(false);

        var diff = player.transform.position - transform.position;
        var moveVec = diff - diff.normalized;

        //guaranteees wthis is always an integer
        moveVec = new Vector2(Mathf.Round(moveVec.x), Mathf.Round(moveVec.y));

        //normalized retruns vector of size 1
        yield return character.Move(moveVec);

        //show dialog 
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () =>
        {
            Debug.Log("Start training battle");
        }));
    }
}
