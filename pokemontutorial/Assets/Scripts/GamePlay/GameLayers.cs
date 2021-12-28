using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLayers : MonoBehaviour
{
    [SerializeField] LayerMask solidObjectsLayer;
    [SerializeField] LayerMask interactablesLayer;
    [SerializeField] LayerMask grassLayer;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask fovLayer;


    public static GameLayers i { get; set; }

    private void Awake()
    {
        i = this;
    }

    public LayerMask SolidLayer
    {
        get => solidObjectsLayer;
    }

    public LayerMask InteractablesLayer
    {
        get => interactablesLayer;
    }

    public LayerMask GrassLayer
    {
        get => grassLayer;
    }

    public LayerMask PlayerLayer
    {
        get => playerLayer;
    }
    public LayerMask FovLayer
    {
        get => fovLayer;
    }
}
