using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AtributosTropas : MonoBehaviour
{
    public StatsIniciales[] statsIniciales;
    public TextAsset csv2;
    public string Texto;

    private float speedProyectilMelee=10f;
    private float sizeProyectilMelee = 1;

    public void Start()
    {

        Texto = csv2.text;

        if (Texto != null)
        {
            Debug.Log("Texto: " + Texto);
            Texto = EliminarHuecos();
            Debug.Log("Texto: " + Texto);

            RellenarStats(Texto);
            Debug.Log("Texto: " + Texto);

        }
    }


    string EliminarHuecos()
    {
        while (Texto.IndexOf(",\"\"") != -1)
        {
            int num = Texto.IndexOf(",\"\"");
            //Debug.Log("se mete " + num);
            Texto = Texto.Remove(num, 3);
        }
        while (Texto.IndexOf("\"\",") != -1)
        {
            int num = Texto.IndexOf("\"\",");
            //Debug.Log("se mete " + num);
            Texto = Texto.Remove(num, 3);
        }
        while (Texto.IndexOf("\"\"") != -1)
        {
            int num = Texto.IndexOf("\"\"");
            //Debug.Log("se mete " + num);
            Texto = Texto.Remove(num, 2);
        }
        return Texto;
    }



    void RellenarStats(string texto)
    {
        //FALTA  HACER LA LECTURA
        string[] vTextos = SepararTextoStats(texto, "ID");
        statsIniciales = new StatsIniciales[vTextos.Length];

        
        for (int i=0; i<vTextos.Length; i++)
        {
            //Debug.Log("i: " + i);

            //Debug.Log("v_textos : " + vTextos[i]);
            string[] vStrAux;


            //statsIniciales[i] = new StatsIniciales();

            statsIniciales[i] = gameObject.AddComponent<StatsIniciales>();

            statsIniciales[i].ID = i;

            statsIniciales[i].name = ObtenerValoresLinea(vTextos[i], "Name")[1];

            vStrAux = ObtenerValoresLinea(vTextos[i], "Pictures");
            statsIniciales[i].iconName = vStrAux[1];
            statsIniciales[i].bigIcon = vStrAux[2];

            statsIniciales[i].movSpeed = float.Parse(ObtenerValoresLinea(vTextos[i], "Movement speed")[1]);

            statsIniciales[i].size = float.Parse(ObtenerValoresLinea(vTextos[i], "Size")[1]);

            vStrAux = ObtenerValoresLinea(vTextos[i], "Health");
            statsIniciales[i].health = new float[vStrAux.Length-1];
            for (int j = 0; j < vStrAux.Length-1; j++)
                statsIniciales[i].health[j] = float.Parse(vStrAux[j+1]);

            vStrAux = ObtenerValoresLinea(vTextos[i], "Armor");
            statsIniciales[i].armor = new float[vStrAux.Length-1];
            for (int j = 0; j < vStrAux.Length-1; j++)
                statsIniciales[i].armor[j] = float.Parse(vStrAux[j+1]);

            vStrAux = ObtenerValoresLinea(vTextos[i], "Attack speed");
            statsIniciales[i].attackSpeed = new float[vStrAux.Length-1];
            for (int j = 0; j < vStrAux.Length-1; j++)
                statsIniciales[i].attackSpeed[j] = float.Parse(vStrAux[j+1]);

            vStrAux = ObtenerValoresLinea(vTextos[i], "Range attack");
            statsIniciales[i].rangeAttack = new float[vStrAux.Length-1];
            for (int j = 0; j < vStrAux.Length-1; j++)
                statsIniciales[i].rangeAttack[j] = float.Parse(vStrAux[j+1]);

            vStrAux = ObtenerValoresLinea(vTextos[i], "Armor penetration");
            statsIniciales[i].armorPenetration = new float[vStrAux.Length - 1];
            for (int j = 0; j < vStrAux.Length - 1; j++)
                statsIniciales[i].armorPenetration[j] = float.Parse(vStrAux[j + 1]);

            vStrAux = ObtenerValoresLinea(vTextos[i], "Range vision");
            statsIniciales[i].rangeVision = new float[vStrAux.Length-1];
            for (int j = 0; j < vStrAux.Length-1; j++)
                statsIniciales[i].rangeVision[j] = float.Parse(vStrAux[j+1]);


            vStrAux = ObtenerValoresLinea(vTextos[i], "Critico");
            statsIniciales[i].critico = new float[vStrAux.Length - 1];
            for (int j = 0; j < vStrAux.Length - 1; j++)
                statsIniciales[i].critico[j] = float.Parse(vStrAux[j + 1]);


            vStrAux = ObtenerValoresLinea(vTextos[i], "Melee");
            statsIniciales[i].melee = (vStrAux[1] == "No") ? false : true;
            if (!statsIniciales[i].melee)
            {
                statsIniciales[i].prefabProyectil = Resources.Load("BigBox/Prefabs/Proyectiles/" + vStrAux[2]) as GameObject;
                if (!statsIniciales[i].prefabProyectil)
                    statsIniciales[i].prefabProyectil = Resources.Load("BigBox/Prefabs/Proyectiles/default") as GameObject;
                statsIniciales[i].speedProyectil = float.Parse(vStrAux[3]);
                statsIniciales[i].sizeProyectil = float.Parse(vStrAux[4]);
            }
            else //AQUI METEMOS LA BAlA QUE ENVIAREMOS CUANDO ES MELEE
            {
                statsIniciales[i].prefabProyectil = Resources.Load("BigBox/Prefabs/Proyectiles/default") as GameObject;
                statsIniciales[i].speedProyectil = speedProyectilMelee;
                statsIniciales[i].sizeProyectil = sizeProyectilMelee;
            }

            statsIniciales[i].groupID = int.Parse(ObtenerValoresLinea(vTextos[i], "Group")[1]);

            vStrAux = ObtenerValoresLinea(vTextos[i], "Group strong");
            statsIniciales[i].groupsStrong = new int[vStrAux.Length/2];
            for (int j = 1; j <= statsIniciales[i].groupsStrong.Length; j++)
                statsIniciales[i].groupsStrong[j - 1] = int.Parse(vStrAux[j * 2 - 1]);

            vStrAux = ObtenerValoresLinea(vTextos[i], "Strong dmg");
            statsIniciales[i].strongDMG = new float[vStrAux.Length-1];
            for (int j = 0; j < vStrAux.Length-1; j++)
                statsIniciales[i].strongDMG[j] = float.Parse(vStrAux[j+1]);

            vStrAux = ObtenerValoresLinea(vTextos[i], "Ignore groups");
            statsIniciales[i].groupsIgnore = new int[vStrAux.Length / 2];
            for (int j = 1; j <= statsIniciales[i].groupsIgnore.Length; j++)
                statsIniciales[i].groupsIgnore[j - 1] = int.Parse(vStrAux[j * 2 - 1]);

            /* vStrAux = ObtenerValoresLinea(vTextos[i], "Parry");
             statsIniciales[i].parry = new float[vStrAux.Length - 1];
             for (int j = 0; j < vStrAux.Length - 1; j++)
                 statsIniciales[i].parry[j] = float.Parse(vStrAux[j + 1]);

             vStrAux = ObtenerValoresLinea(vTextos[i], "Block");
             statsIniciales[i].block = new float[vStrAux.Length - 1];
             for (int j = 0; j < vStrAux.Length-1; j++)
                 statsIniciales[i].block[j] = float.Parse(vStrAux[j+1]);*/

            /*  vStrAux = ObtenerValoresLinea(vTextos[i], "Charge");
             statsIniciales[i].charge = (vStrAux[1] == "No") ? false : true;
             if (statsIniciales[i].charge)
             {
                 statsIniciales[i].chargeDMG = float.Parse(vStrAux[2]);
                 statsIniciales[i].chargeExtraSpeed = float.Parse(vStrAux[3]);
                 statsIniciales[i].segTimeToCharge = float.Parse(vStrAux[4]);
             }*/

            vStrAux = ObtenerValoresLinea(vTextos[i], "Physical damage");
            statsIniciales[i].physicalDamage = new float[vStrAux.Length - 1];
            for (int j = 0; j < vStrAux.Length-1; j++)
                statsIniciales[i].physicalDamage[j] = float.Parse(vStrAux[j+1]);

            vStrAux = ObtenerValoresLinea(vTextos[i], "Magical damage");
            statsIniciales[i].magicalDamage = new float[vStrAux.Length - 1];
            for (int j = 0; j < vStrAux.Length-1; j++)
                statsIniciales[i].magicalDamage[j] = float.Parse(vStrAux[j+1]);


        }


    }




    public void CrearNuevo(int IDPredeterminada,GameObject PersonajePadre, int nivel)
    {

        Atacar scriptAtaque = PersonajePadre.GetComponentInChildren<Atacar>();

        scriptAtaque.stats.ID=IDPredeterminada;
        scriptAtaque.stats.name= statsIniciales[IDPredeterminada].name;
        scriptAtaque.stats.movSpeed= statsIniciales[IDPredeterminada].movSpeed;
        scriptAtaque.stats.iconName = statsIniciales[IDPredeterminada].iconName;
        scriptAtaque.stats.bigIcon = statsIniciales[IDPredeterminada].bigIcon;
        scriptAtaque.stats.size= statsIniciales[IDPredeterminada].size;


        scriptAtaque.stats.health= statsIniciales[IDPredeterminada].health[nivel];
        scriptAtaque.stats.armor= statsIniciales[IDPredeterminada].armor[nivel];
        scriptAtaque.stats.attackSpeed= statsIniciales[IDPredeterminada].attackSpeed[nivel];
        scriptAtaque.stats.rangeAttack= statsIniciales[IDPredeterminada].rangeAttack[nivel];
        scriptAtaque.stats.rangeVision= statsIniciales[IDPredeterminada].rangeVision[nivel];
        scriptAtaque.stats.armorPenetration= statsIniciales[IDPredeterminada].armorPenetration[nivel];


        scriptAtaque.stats.melee= statsIniciales[IDPredeterminada].melee;
        scriptAtaque.stats.prefabProyectil= statsIniciales[IDPredeterminada].prefabProyectil;
        scriptAtaque.stats.speedProyectil= statsIniciales[IDPredeterminada].speedProyectil;
        scriptAtaque.stats.sizeProyectil= statsIniciales[IDPredeterminada].sizeProyectil;
        scriptAtaque.stats.groupID= statsIniciales[IDPredeterminada].groupID;


        int contAux = statsIniciales[IDPredeterminada].groupsStrong.Length;
        scriptAtaque.stats.groupsStrong = new int[contAux];
        for (int i=0;i<contAux;i++)
            scriptAtaque.stats.groupsStrong[i]= statsIniciales[IDPredeterminada].groupsStrong[i];

        contAux = statsIniciales[IDPredeterminada].strongDMG.Length;
        scriptAtaque.stats.strongDMG = new float[contAux];
        for (int i = 0; i < contAux; i++)
            scriptAtaque.stats.strongDMG[i]= statsIniciales[IDPredeterminada].strongDMG[i];

        contAux = statsIniciales[IDPredeterminada].groupsIgnore.Length;
        scriptAtaque.stats.groupsIgnore = new int[contAux];
        for (int i = 0; i < contAux; i++)
            scriptAtaque.stats.groupsIgnore[i]= statsIniciales[IDPredeterminada].groupsIgnore[i];


        //scriptAtaque.stats.parry= statsIniciales[IDPredeterminada].parry[nivel];
        //  scriptAtaque.stats.block= statsIniciales[IDPredeterminada].block[nivel];

        scriptAtaque.stats.critico = statsIniciales[IDPredeterminada].critico[nivel];

        /*  scriptAtaque.stats.charge= statsIniciales[IDPredeterminada].charge;
          scriptAtaque.stats.chargeDMG= statsIniciales[IDPredeterminada].chargeDMG;
          scriptAtaque.stats.chargeExtraSpeed= statsIniciales[IDPredeterminada].chargeExtraSpeed;
          scriptAtaque.stats.segTimeToCharge = statsIniciales[IDPredeterminada].segTimeToCharge;*/
        scriptAtaque.stats.physicalDamage= statsIniciales[IDPredeterminada].physicalDamage[nivel];
        scriptAtaque.stats.magicalDamage= statsIniciales[IDPredeterminada].magicalDamage[nivel];

        ModificarScriptsDelGOEnFuncionDeEstasVariables(PersonajePadre);

    }




    void ModificarScriptsDelGOEnFuncionDeEstasVariables(GameObject PersonajePadre)
    {
        Atacar scriptAtaque = PersonajePadre.GetComponentInChildren<Atacar>();
        Text vida = null;
        if (PersonajePadre.transform.GetChild(0).GetChild(0)  && PersonajePadre.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>())
            vida = PersonajePadre.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        GameObject areaAtaque = PersonajePadre.transform.GetChild(1).gameObject;

        if (!scriptAtaque.soyTower_ok)//Puesto que las torres no tienen área vision ni moverGO
        {
            GameObject areaVision = PersonajePadre.transform.GetChild(2).gameObject;
            // MoverGO movimiento = PersonajePadre.GetComponent<MoverGO>();

            areaVision.GetComponent<ModificarRadio>().metros = scriptAtaque.stats.rangeVision;
            // movimiento.vel = scriptAtaque.stats.movSpeed;

        }
        if (vida)
            vida.text = scriptAtaque.stats.health.ToString();
        areaAtaque.GetComponent<ModificarRadio>().metros = scriptAtaque.stats.rangeAttack;

    }



    string[] SepararTextoStats(string str, string substr) //esta funcion cuenta el número de veces que se encuentra la cadena substr dentro de la cadena str
    {
        Debug.Log("str: " + str);
        string aux = str;
        int index = str.IndexOf(substr);
        int acc = 0;

        while (index > -1  &&  acc <1000) //le ponemos maximo de 1000 para que no pete en caso de error
        {
            acc++;
            aux = aux.Substring(index+1);
            index = aux.IndexOf(substr);
        }

        string[] vStr = new string[acc];

        aux = str;
        for (int i=0; i<acc-1; i++)
        {
            index = aux.IndexOf(substr, 2);
            vStr[i] = aux.Substring(0, index);
            aux = aux.Substring(index);
            //Debug.Log(vStr[0]);
        }
        if (acc>0)
            vStr[acc-1] = aux.Substring(0);
        //Debug.Log(acc);
        //Debug.Log(vStr[acc-1]);


        return vStr;
    }


    string[] ObtenerValoresLinea (string texto, string clave)
    {
        //Debug.Log("texto: " + texto);
        //Debug.Log("clave: " + clave);
        string[] valores;
        int index = texto.IndexOf(clave) - 1;
        int index2 = texto.IndexOf("\n", index + 1);
        string linea = texto.Substring(index, index2 - index);
        valores = linea.Split(","[0]);
       

        for (int i = 0; i < valores.Length; i++)
        {
            valores[i] = valores[i].Substring(1, valores[i].Length - 2);
            int index3 = valores[i].IndexOf("\"");
            if (index3 > -1)
                valores[i] = valores[i].Substring(0, index3);
        }

        //Debug.Log(linea);
        return valores;
    }

}
