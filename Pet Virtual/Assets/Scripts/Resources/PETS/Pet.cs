using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public enum TIPO
{
    FOGO,
    AGUA,
    PLANTA
}

public enum PETANIM
{
    OXL,
    DOG,
    CAT
}

public enum MOVES 
{
    RUSH,
    BITE,
    SCRATCH,
    FIREBALL,
    WATERSPIT,
    TRUNKBLOW
}

public struct PetGerado
{
    public string nome;
    //status
    public int health;
    public int stamina;
    public int atk;
    public int spd;
    public int using_move;
    public List<MOVES> learnedMoves;

}

[CreateAssetMenu(fileName = "New Pet", menuName = "Pet")]
public class Pet : ScriptableObject
{
    //aparencia e estilo
    public Sprite aparencia;
    public TIPO Tipo;
    public PETANIM anim;
    //status
    public int baseHea;
    public int baseStm;
    public int baseAtq;
    public int baseSpd;
    public List<MOVES> learned_moves;
    public int using_move;

    public PetGerado GeneratePet(string Petnome)
    {
        PetGerado petG = new PetGerado();
        int level = 1;
        //status
        int health = (int)(math.floor(0.01* (2 * this.baseHea) * level) + level + 10);
        int stamina = this.baseStm;
        int atk = (int)(math.floor(0.01 * (2 * this.baseAtq) * level) + 5);
        int spd = (int)(math.floor(0.01 * (2 * this.baseSpd) * level) + 5);
        int using_move = 0;
        List<MOVES> learnedMoves = new List<MOVES>();
        if(this.anim == PETANIM.OXL)
        {
            learnedMoves.Add(MOVES.RUSH);
        }
        else if (this.anim == PETANIM.DOG)
        {
            learnedMoves.Add(MOVES.BITE);
        }
        else if (this.anim == PETANIM.CAT)
        {
            learnedMoves.Add(MOVES.SCRATCH);
        }
        //cria a estrutura
        petG.nome = Petnome;
        petG.health = health;
        petG.stamina = stamina;
        petG.atk = atk;
        petG.spd = spd;
        petG.using_move = using_move;
        petG.learnedMoves = learnedMoves;
        return petG;
    }
}
/*
 --status base
            self.hp = math.floor(0.01 * (2 * self.hp_base) * self.level) + self.level + 10
            self.atk = math.floor(0.01 * (2 * self.atk_base) * self.level) + 5
            self.specialA = math.floor(0.01 * (2 * self.specialA_base) * self.level) + 5
            self.def = math.floor(0.01 * (2 * self.def_base) * self.level) + 5
            self.specialD = math.floor(0.01 * (2 * self.specialD_base) * self.level) + 5
            self.speed = math.floor(0.01 * (2 * self.speed_base) * self.level) + 5
*/
