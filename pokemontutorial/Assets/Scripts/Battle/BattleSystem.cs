using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//variable to store state of battle
public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, busy}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;

    private void Start()
    {
        StartCoroutine(SetUpBattle());
    }

    public IEnumerator SetUpBattle()
    {
        playerUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);
        enemyUnit.Setup();
        enemyHud.SetData(enemyUnit.Pokemon);
        
        
        yield return(dialogBox.TypeDialog($"A wild {enemyUnit.Pokemon.Base.Name} appeared!"));
        yield return new WaitForSeconds(1f);

    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
    }
}
