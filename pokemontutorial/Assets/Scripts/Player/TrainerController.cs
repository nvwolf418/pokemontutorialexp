using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainerController : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] Sprite sprite;
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject exclamation;
    [SerializeField] GameObject fov;
    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        SetFovRotation(character.Animator.DefaultDirection);
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
            GameController.Instance.StartTrainerBattle(this);
        }));
    }

    public void SetFovRotation(FacingDirection dir)
    {
        float angle = 0f;

        switch (dir)
        {
            case FacingDirection.Right:
                angle = 90f;
                break;
            case FacingDirection.Up:
                angle = 180f;
                break;
            case FacingDirection.Left:
                angle = 270f;
                break;
            default:
                Debug.Log("something went wrong with direction for trainer!");
                break;
        }

        //this eulernagles is a property used to tset the rotation of the vector
        fov.transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    public string Name
    {
        get => name;
    }

    public Sprite Sprite
    {
        get => sprite;
    } 
        
}
