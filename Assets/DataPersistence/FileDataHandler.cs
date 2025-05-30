using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private string encryptionCodeWord = "password";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption  = false){
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(){
        //Use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath)){
            try{
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open)){
                    using (StreamReader reader = new StreamReader(stream)){
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption){
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch (Exception e){
                Debug.LogException(e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data){
        //Use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try{
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string datatToStore = JsonUtility.ToJson(data, true);

            if (useEncryption){
                datatToStore = EncryptDecrypt(datatToStore);
            }


            using (FileStream stream = new FileStream(fullPath, FileMode.Create)){
                using (StreamWriter writer = new StreamWriter(stream)){
                    writer.Write(datatToStore);
                }
            }
        }
        catch (Exception e){
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string data){
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++){
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

}