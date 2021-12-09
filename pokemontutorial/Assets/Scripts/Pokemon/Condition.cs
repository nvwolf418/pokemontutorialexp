using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Condition
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string StartMessage { get; set; }
    public Action<Pokemon> OnStart { get; set; }
    public Action<Pokemon> OnAfterTurn { get; set; }
    //Regular Action above doesn't use function that return values 
    //a Func is required for that, second param is return type
    public Func<Pokemon, bool> OnBeforeMove { get; set; }
}
