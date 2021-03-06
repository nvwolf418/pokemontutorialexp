using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokemonParty : MonoBehaviour
{
    [SerializeField] List<Pokemon> pokemons;

    public List<Pokemon> Pokemons
    {
        get { return pokemons; }
    }

    private void Start()
    {
       foreach(Pokemon pokemon in pokemons)
        {
            pokemon.Init();
        }
    }

    public Pokemon GetHealthyPokemon()
    {
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
    }

    public void AddPokemon(Pokemon newPokemon)
    {
        if(pokemons.Count < 6)
        {
            pokemons.Add(newPokemon);
        }
        else
        {
            //transfer to our pc TODO: once implemented
        }
    }
}
