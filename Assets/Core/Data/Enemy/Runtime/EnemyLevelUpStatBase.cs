using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class EnemyLevelUpStatBase
{
  [SerializeField]
  EnemyID enemyid;
  public EnemyID ENEMYID { get {return enemyid; } set { this.enemyid = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { this.name = value;} }
  
  [SerializeField]
  int lvup;
  public int Lvup { get {return lvup; } set { this.lvup = value;} }
  
  [SerializeField]
  float hp;
  public float HP { get {return hp; } set { this.hp = value;} }
  
  [SerializeField]
  float hpregen;
  public float Hpregen { get {return hpregen; } set { this.hpregen = value;} }
  
  [SerializeField]
  float damage;
  public float Damage { get {return damage; } set { this.damage = value;} }
  
  [SerializeField]
  float movement;
  public float Movement { get {return movement; } set { this.movement = value;} }
  
  [SerializeField]
  float armor;
  public float Armor { get {return armor; } set { this.armor = value;} }
  
  [SerializeField]
  float attackspeed;
  public float Attackspeed { get {return attackspeed; } set { this.attackspeed = value;} }
  
  [SerializeField]
  float accuracy;
  public float Accuracy { get {return accuracy; } set { this.accuracy = value;} }
  
  [SerializeField]
  float dodge;
  public float Dodge { get {return dodge; } set { this.dodge = value;} }
  
  [SerializeField]
  float critrate;
  public float Critrate { get {return critrate; } set { this.critrate = value;} }
  
  [SerializeField]
  float critdamage;
  public float Critdamage { get {return critdamage; } set { this.critdamage = value;} }
  
  [SerializeField]
  float attackrange;
  public float Attackrange { get {return attackrange; } set { this.attackrange = value;} }
  
  [SerializeField]
  int coindrop;
  public int Coindrop { get {return coindrop; } set { this.coindrop = value;} }
  
  [SerializeField]
  int gemdrop;
  public int Gemdrop { get {return gemdrop; } set { this.gemdrop = value;} }
  
}