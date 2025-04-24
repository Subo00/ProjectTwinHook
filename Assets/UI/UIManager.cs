using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    //Cameras
    public PromptController camOne;
    public PromptController camTwo;

    protected MenuUI menuUI;

    private bool isMenuOpen = true;
    private UIControls controls;
    /*
    bool isInventoryOpen = false;
     private enum UIType { None, Inventory, Crafting, Minigame, Menu, Dialog, Tutorial };


    */
    protected virtual void Awake(){
        if (Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new UIControls();
        controls.UI.Menu.performed += ctx => ToggleMenu();
    }

    protected virtual void Start(){
        menuUI = MenuUI.Instance;
        if ( menuUI != null){
            ToggleMenu();
        }
    }

    private void OnEnable()
    {
        controls.UI.Enable();
    }

    private void OnDisable()
    {
        controls.UI.Disable();
    }

    public void ShowInteractionOnObject(Transform interactPoint, bool isPlayerOne = false){

        if (isPlayerOne){
            camOne.ShowInteractionOnObject(interactPoint);
        }
        else{
            camTwo.ShowInteractionOnObject(interactPoint);
        }
    }

    public void ShowTimeOnObject(Transform interactPoint, float time){

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(interactPoint.position);
        
    }

    public void HideInteraction(){
        camOne.HideInteraction();
        camTwo.HideInteraction();
    }

  

    public virtual void ToggleMenu(){
        isMenuOpen = !isMenuOpen;

        if (isMenuOpen){
            menuUI.Toggle(isMenuOpen);
            Time.timeScale = 0; //pause the game
        }
        else{
            menuUI.Toggle(isMenuOpen);
            Time.timeScale = 1f;
        }
    }

    public void ToggleInventory(){
        /* isInventoryOpen = !isInventoryOpen;

         if (isInventoryOpen)
         {
             CloseOtherUIs(UIType.Inventory);
             inventoryUI.Toggle(isInventoryOpen);
             SetCurrentUIType(UIType.Inventory);
             EventSystem.current.SetSelectedGameObject(inventoryUI.GetFirstButton());
         }
         else
         {
             CloseOtherUIs(UIType.None);
         }*/
    }
    /*
    private void CloseOtherUIs(UIType currentUI)
    {
        if (currentUI != UIType.Inventory)
        {
            isInventoryOpen = false;
            inventoryUI.Toggle(isInventoryOpen);
        }

        if (currentUI == UIType.None)
        {
            ToggleCursor(false);
            playerMovement.ToggleUI(false);
            questUI.Toggle(true);
            SetCurrentUIType(UIType.None);
        }
        else
        {
            //ToggleCursor(true);
            playerMovement.ToggleUI(true);
        }
    }

    */

}