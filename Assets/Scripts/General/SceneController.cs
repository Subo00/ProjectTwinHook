using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour{

    public static SceneController Instance;

   // [SerializeField] private string firstSceneName = "TestingScene";

    private void Awake(){

        if (Instance != null){
            Destroy(this.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneID){
        SceneManager.LoadScene(sceneID);
    }

    public void LoadStartMenu(){
        SceneManager.LoadScene(0);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public string GetCurrentSceneName(){
        return SceneManager.GetActiveScene().name;
    }

    public void OnNewGameClicked(){
        //DataPersistenceManager.Instance.NewGame();
       // SceneManager.LoadSceneAsync(firstSceneName);
    }

    public void OnContinueGameClicked(){
      //  SceneManager.LoadSceneAsync(firstSceneName);
    }
}