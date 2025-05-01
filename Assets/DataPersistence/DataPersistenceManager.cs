using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;


public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private FileDataHandler dataHandler;
    //Create a class by copying the GameData.txt in Data folder
    private GameData gameData; 
    private List<IDataPersistence> dataPersistenceObjects;
    public IDataPersistence PlayerOne;
    public IDataPersistence PlayerTwo;
    public static DataPersistenceManager Instance {  get; private set; }

    private void Awake(){
        if(Instance != null)
        {
            Debug.LogError("There are more than one DataPersistenceManagers in the scene");
            Destroy(this.gameObject);
        }

        Instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }


    private void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable(){
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }


    public void NewGame(){
        this.gameData = new GameData();
    }

    public void LoadGame(){

        this.gameData = dataHandler.Load();


        if(this.gameData == null){
            NewGame();
        }

        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects){
            dataPersistenceObj.LoadData(gameData);
        }

    }

    public void SaveGame(){
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects){
            dataPersistenceObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit(){
       // SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects(){
        IEnumerable<IDataPersistence> dataPersistenceObjects = 
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>(); 

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode){
        this.dataHandler = new FileDataHandler("C:\\Unity Projects\\Test Save", fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene){
        SaveGame();
    }

    private void Start(){
        //this.dataHandler = new FileDataHandler("C:\\Unity Projects\\Test Save", fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
        
    }

    public void LoadPlayerPos(bool isOne){
        if (isOne){
            PlayerOne.LoadData(gameData);
        }
        else{
            PlayerTwo.LoadData(gameData);
        }
    }

    public void SaveCheckPoint(Vector3 position)
    {
        this.gameData.playerPosition = position;
        dataHandler.Save(gameData);
    }
}
