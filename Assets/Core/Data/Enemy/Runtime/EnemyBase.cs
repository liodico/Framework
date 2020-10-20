using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class EnemyBase
{
  [SerializeField]
  EnemyID enemyid;
  public EnemyID ENEMYID { get {return enemyid; } set { this.enemyid = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { this.name = value;} }
  
  [SerializeField]
  string[] attacktype = new string[0];
  public string[] Attacktype { get {return attacktype; } set { this.attacktype = value;} }
  
  [SerializeField]
  Race race;
  public Race RACE { get {return race; } set { this.race = value;} }
  
  [SerializeField]
  Element element;
  public Element ELEMENT { get {return element; } set { this.element = value;} }
  
  [SerializeField]
  RangeType rangetype;
  public RangeType RANGETYPE { get {return rangetype; } set { this.rangetype = value;} }
  
  [SerializeField]
  Target target;
  public Target TARGET { get {return target; } set { this.target = value;} }
  
  [SerializeField]
  string ai;
  public string AI { get {return ai; } set { this.ai = value;} }
  
  [SerializeField]
  int lv;
  public int Lv { get {return lv; } set { this.lv = value;} }
  
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
  float invisible;
  public float Invisible { get {return invisible; } set { this.invisible = value;} }
  
  [SerializeField]
  float coindrop;
  public float Coindrop { get {return coindrop; } set { this.coindrop = value;} }
  
  [SerializeField]
  float gemdrop;
  public float Gemdrop { get {return gemdrop; } set { this.gemdrop = value;} }
  
}