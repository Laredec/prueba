using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class StatsIniciales :MonoBehaviour
{
    public int ID;
    public string name;
    public float movSpeed;
    public string iconName;
    public string bigIcon;
    public float size;
    public float[] health;
    public float[] armor;
    public float[] attackSpeed;
    public float[] rangeAttack;
    public float[] rangeVision;
    public float[] armorPenetration;
    public bool melee;
        public GameObject prefabProyectil;
        public float speedProyectil;
        public float sizeProyectil;
    public int groupID;
    public int[] groupsStrong;//variable que dice contra quienes es fuerte esta unidad
        public float[] strongDMG;//Esta variable nos dice cuanto de fuerte es la variable de arriba
    public int[] groupsIgnore;
    public float[] critico;
  /*  public float[] parry;
    public float[] block;
    public bool charge;
        public float chargeDMG;
        public float chargeExtraSpeed;
        public float segTimeToCharge;*/
    public float[] physicalDamage;
    public float[] magicalDamage;


}