using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsDb
{
    public static void Init()
    {
        foreach(var kvp in Conditions)
        {
            var conditionId = kvp.Key;
            var condition = kvp.Value;

            condition.Id = conditionId;
        }
    }

    public static Dictionary<ConditionID, Condition> Conditions { get; set; } = new Dictionary<ConditionID, Condition>()
    {
        { 
            ConditionID.psn,
            new Condition()
            {
                Name = "Poison",
                StartMessage = "has been poisoned",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.MaxHP / 8);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt itself due to poison!");
                }
            }
        },
        {
            ConditionID.brn,
            new Condition()
            {
                Name = "Burn",
                StartMessage = "has been burned",
                OnAfterTurn = (Pokemon pokemon) =>
                {
                    pokemon.UpdateHP(pokemon.MaxHP / 16);
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} hurt itself due to burn!");
                }
            }
        },
        {
            ConditionID.par,
            new Condition()
            {
                Name = "Paralyzed",
                StartMessage = "has been Paralyzed",
                OnBeforeMove  = (Pokemon pokemon) =>
                {
                    if(Random.Range(1,4) == 1)
                    {
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is paralyzed and can't move!");
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            ConditionID.frz,
            new Condition()
            {
                Name = "Frozen",
                StartMessage = "has been Frozen",
                OnBeforeMove  = (Pokemon pokemon) =>
                {
                    if(Random.Range(1,4) == 1)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is frozen and can't move!");
                        return true;
                    }
                    return false;
                }
            }

        },
        {
            ConditionID.slp,
            new Condition()
            {
                Name = "Sleep",
                StartMessage = "has fallen asleep",
                OnStart = (Pokemon pokemon) =>
                {
                    //sleep for 1-3 turns
                    pokemon.StatusTime = Random.Range(1,4);
                    Debug.Log($"Will be asleep for {pokemon.StatusTime} moves");
                },
                OnBeforeMove  = (Pokemon pokemon) =>
                {
                    if(pokemon.StatusTime <= 0)
                    {
                        pokemon.CureStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} woke up!");
                        return true;

                    }
                    pokemon.StatusTime--;
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is sleeping!");
                    return false;
                }
            }

        },
        
        //volatile status conditions
        {
            ConditionID.confusion,
            new Condition()
            {
                Name = "Confusion",
                StartMessage = "has become confused",
                OnStart = (Pokemon pokemon) =>
                {
                    //confused for 1-4 turns
                    pokemon.VolatileStatusTime = Random.Range(1,5);
                    Debug.Log($"Will be confused for {pokemon.VolatileStatusTime} moves");
                },
                OnBeforeMove  = (Pokemon pokemon) =>
                {
                    if(pokemon.VolatileStatusTime <= 0)
                    {
                        pokemon.CureVolatileStatus();
                        pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} kicked out of confusion!");
                        return true;

                    }
                    pokemon.VolatileStatusTime--;

                    //50% chance to do a move
                    if(Random.Range(1,3) == 1)
                    {
                        return true;
                    }
                    
                    //hurt by confusion
                    pokemon.StatusChanges.Enqueue($"{pokemon.Base.Name} is confused!");
                    pokemon.UpdateHP(pokemon.MaxHP / 8);
                    pokemon.StatusChanges.Enqueue($"It hurt itself due to confusion");
                    return false;
                }
            }

        }
    };

    public static float GetStatusBonus(Condition condition)
    {
        if (condition == null)
            return 1f;
        else if(condition.Id == ConditionID.slp || condition.Id == ConditionID.frz)
        {
            return 2f;
        }
        else if (condition.Id == ConditionID.par || condition.Id == ConditionID.psn || condition.Id == ConditionID.brn)
        {
            return 1.5f;
        }


        return 1f;
    }
}

public enum ConditionID
{
    none, psn, brn, slp, par, frz,
    confusion
}
