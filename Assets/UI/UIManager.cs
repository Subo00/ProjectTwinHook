using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    protected Canvas canvas;
    protected GameObject interactionPrompt;
    protected GameObject timeText;
    protected Transform lastInteractPoint = null;
    protected MenuUI menuUI;


    public static UIManager Instance;

    private bool isMenuOpen = false;
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

        canvas = GetComponentInChildren<Canvas>();
        interactionPrompt = transform.GetChild(0).GetChild(0)?.gameObject;
        timeText = transform.GetChild(0).GetChild(1)?.gameObject;

        if (!timeText || !interactionPrompt){
            Debug.LogError("GameObjects on UIManager are missing");
        }
    }

    protected virtual void Start(){
        menuUI = MenuUI.Instance;
        HideInteraction();
    }

    public void ShowInteractionOnObject(Transform interactPoint){
        // Convert the world point to screen point
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(interactPoint.position);

        // Check if the point is in front of the camera
        if (screenPoint.z > 0){
            // Convert screen point to canvas space
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPoint, canvas.worldCamera, out canvasPos);

            // Set the position of the interact button
            interactionPrompt.GetComponent<RectTransform>().anchoredPosition = canvasPos;

            // Optionally, make the interact button active
            interactionPrompt.SetActive(true);
        }
    }

    public void ShowTimeOnObject(Transform interactPoint, float time){

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(interactPoint.position);
        if (screenPoint.z > 0){
            // Convert screen point to canvas space
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPoint, canvas.worldCamera, out canvasPos);

            // Set the position of the interact button
            timeText.GetComponent<RectTransform>().anchoredPosition = canvasPos;
            timeText.GetComponent<TextMeshProUGUI>().text = Mathf.Round(time).ToString();

            if (time < 0.1f){
                timeText.SetActive(false);
            }
            else{
                timeText.SetActive(true);
            }
        }
    }


    public void SetInteractPoint(Transform interactPoint = null){
        lastInteractPoint = interactPoint;
    }

    public void HideInteraction(){
        interactionPrompt.SetActive(false);
        timeText.SetActive(false);
    }

    public virtual void ToggleMenu(){
        isMenuOpen = !isMenuOpen;

        if (isMenuOpen)
        {
            menuUI.Toggle(isMenuOpen);
            Time.timeScale = 0; //pause the game
        }
        else
        {
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