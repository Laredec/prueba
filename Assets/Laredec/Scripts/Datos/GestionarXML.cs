using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;

public class GestionarXML : MonoBehaviour
{
    // UserData is our custom class that holds our defined objects we want to store in XML format
    public class UserData
    {
        // We have to define a default instance of the structure
        public DatosMovil datos;
        // Default constructor doesn't really do anything at the moment
    }

    public bool guardarOk = false;
    public bool cargarOk = true;
    public bool borrarOk = false;
    static public bool cargadoOk = false;
    // This is our local private members

    private string _FileLocation;
    private string _FileName = "Data.xml";
    private UserData myData;
    private string _data;

    // When the EGO is instansiated the Start will trigger
    // so we setup our initial values for our local members
    //function Start () {
    void Awake()
    {

        // Where we want to save and load to and from
#if UNITY_EDITOR  ||  UNITY_ANDROID
        _FileLocation = Application.persistentDataPath;
#else
        _FileLocation = Application.dataPath;
#endif

        // we need soemthing to store the information into
        myData = new UserData();
        //Cargar_Datos();
        //SA_Datos_Usuario.Usuario.Num_Monedas = 5000;
        //Guardar_Datos();

    }





    void Update()
    {
        //cargadoOk = false;
        if (borrarOk)
        {
            Borrar_Datos();
            borrarOk = false;
        }
        if (guardarOk)
        {
            Guardar_Datos();
            guardarOk = false;
        }
        if (cargarOk)
        {
            Cargar_Datos();
            cargarOk = false;
        }
        
    }

    /* The following metods came from the referenced URL */
    //string UTF8ByteArrayToString(byte[] characters)
   string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString  = encoding.GetString(characters);
        return (constructedString);
    }

    //byte[] StringToUTF8ByteArray(string pXmlString)
    byte[] StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray  = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    // Here we serialize our UserData object of myData
    //string SerializeObject(object pObject)
    string SerializeObject(object pObject)
    {
        string XmlizedString = null;
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(UserData));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }

    // Here we deserialize it back into its original form 
    object DeserializeObject(string pXmlizedString)
    {
        XmlSerializer xs = new XmlSerializer(typeof(UserData));
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        //XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        return xs.Deserialize(memoryStream);
    }

    // Finally our save and load methods for the file itself
    void CreateXML()
    {
        StreamWriter writer;
        //FileInfo t = new FileInfo(_FileLocation+"\\"+ _FileName);
        FileInfo t = new FileInfo(_FileLocation + "/" + _FileName);

        if (!t.Exists)
        {
            writer = t.CreateText();
        }
        else
        {
            t.Delete();
            writer = t.CreateText();
        }

        writer.Write(_data);
        writer.Close();
        Debug.Log("File written.");
    }

    void LoadXML()
    {
        StreamReader r;
        string _info;
        //StreamReader r = File.OpenText(_FileLocation+"\\"+ _FileName);
        if (File.Exists(_FileLocation + "/" + _FileName))
        {
            r = File.OpenText(_FileLocation + "/" + _FileName);
            _info = r.ReadToEnd();
            r.Close();
            _data = _info;
            Debug.Log("File Read");
        }
        else
        {
            Debug.Log("File NOT Read");
            Guardar_Datos();

            r = File.OpenText(_FileLocation + "/" + _FileName);
            _info = r.ReadToEnd();
            r.Close();
            _data = _info;
            Debug.Log("File Read");
        }

    }



    public void Cargar_Datos()
    {
        LoadXML();
        if (_data.ToString() != "")
        {
            myData = (UserData)DeserializeObject(_data);
            IniciarDatos.Instance.datos = myData.datos;
            Debug.Log(_data);
            cargadoOk = true; //bool que usaremos para ciertas cosas en otros scripts (lo volveremos false alli)
        }
    }



    public void Guardar_Datos()
    {
        myData.datos = IniciarDatos.Instance.datos;

        // Time to creat our XML!
        _data = SerializeObject(myData);
        // This is the final resulting XML from the serialization process
        CreateXML();
        //Debug.Log(_data);
    }


    public void Borrar_Datos()
    {
        GetComponent<IniciarDatos>().Construir();
    }


}
