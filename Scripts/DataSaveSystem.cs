using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public static class DataSaveSystem
{


    public static void SaveAll(PlayerBag playerBag, Text operationStatus)
    {
        

        operationStatus.text = "Creating formatters..."; //=====STATUS DISPLAY ONLY!
        BinaryFormatter formatter = new BinaryFormatter();
        operationStatus.text = "Creating path..."; //=====STATUS DISPLAY ONLY!
        string path = Application.persistentDataPath + "/SavedData/playerStatus.sav";
        //Directory.CreateDirectory(Application.persistentDataPath + "/SavedData");//DEBUG
        //File.Create(path);//DEBUG
        FileStream stream = new FileStream(path, FileMode.Create);

        operationStatus.text = "Saving player data..."; //=====STATUS DISPLAY ONLY!
        SAVE_PlayerStatus data1 = new SAVE_PlayerStatus(playerBag);

        formatter.Serialize(stream, data1);
        stream.Close();
        operationStatus.text = "Data loaded!"; //=====STATUS DISPLAY ONLY!
    }

    public static SAVE_PlayerStatus LoadPlayerStatus()
    {
        string path = Application.persistentDataPath + "/SavedData/playerStatus.sav";
        if(File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            if(stream.Length > 0)
            {
                SAVE_PlayerStatus data1 = formatter.Deserialize(stream) as SAVE_PlayerStatus;
                stream.Close();
                return data1;
            }

            else
            {
                return null;
            }

            
            //CHANGES:
           //When lodaing, checking the stream lenght first, before deserializing.
            

        }
        else //IF NOT FOUND
        {
            Debug.LogError("Save file not found!");
            return null;
        }


    }


   
}
