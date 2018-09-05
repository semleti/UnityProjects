using UnityEngine;
using System.Collections.Generic;

public class PokemonEditor{
	public PokemonID id;
	//public int level;
	//public int xp;
	public int HPBase=0;
	public int Attack;
	public int Defense;
	public int SPAttack;
	public int SPDefence;
	public int Speed;
	//public int PV;
	public List<Attack> levelingAttacks;
	public List<Attack> TMattacks;
	public List<Attack> breedAttacks;
	public List<TypeElement> types;
	public string name;
	public List<Evolution> evolutions;

}
