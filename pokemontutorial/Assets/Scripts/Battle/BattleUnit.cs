using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Pokemon Pokemon { get; set; }

    Image image;
    Vector3 originalPos;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        originalColor = image.color;
    }

    public void Setup()
    {
        Pokemon = new Pokemon(_base, level);
        if(isPlayerUnit)
        {
            image.sprite = Pokemon.Base.BackSprite;
        }
        else
        {
            image.sprite = Pokemon.Base.FrontSprite;
        }

        image.color = originalColor;
        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        if(isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(560f, originalPos.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(-560f, originalPos.y);
        }

        image.transform.DOLocalMoveX(originalPos.x, 2f);
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();
        //determines if + or - if player unit because of location on screen, enemy unit is opposite, so we move in opposite directions for attack
        int transVal = isPlayerUnit ? 1 : -1;


            sequence.Append(image.transform.DOLocalMoveY(originalPos.y + (transVal * 20f), 0.35f))
                    .Join(image.transform.DOLocalMoveX(originalPos.x + (transVal * 50f), 0.35f));
        

            //flips the translation direction to go back
            transVal *= -1;


            sequence.Append(image.transform.DOLocalMoveY(originalPos.y, 0.35f))
                    .Join(image.transform.DOLocalMoveX(originalPos.x, 0.35f));

    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(image.DOColor(Color.gray, 0.1f));
        sequence.Append(image.DOColor(originalColor, 0.1f));
        
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f))
                .Join(image.DOFade(0f, 0.5f));
    }

}
