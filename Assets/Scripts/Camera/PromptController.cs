using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptController : MonoBehaviour
{
    protected Canvas canvas;
    protected Camera camera;
    protected GameObject interactionPrompt;
    protected GameObject timeText;
    protected Transform lastInteractPoint = null;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        camera = GetComponentInChildren<Camera>();
        interactionPrompt = transform.GetChild(0).GetChild(0)?.gameObject;
        timeText = transform.GetChild(0).GetChild(1)?.gameObject;

        HideInteraction();
    }

    public void ShowInteractionOnObject(Transform interactPoint)
    {
        // Convert the world point to screen point
        Vector3 screenPoint = camera.WorldToScreenPoint(interactPoint.position);

        // Check if the point is in front of the camera
        if (screenPoint.z > 0)
        {
            // Convert screen point to canvas space
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPoint, canvas.worldCamera, out canvasPos);

            // Set the position of the interact button
            interactionPrompt.GetComponent<RectTransform>().anchoredPosition = canvasPos;

            // Optionally, make the interact button active
            interactionPrompt.SetActive(true);
        }
    }

    public void ShowTimeOnObject(Transform interactPoint, float time)
    {

        Vector3 screenPoint = camera.WorldToScreenPoint(interactPoint.position);
        if (screenPoint.z > 0)
        {
            // Convert screen point to canvas space
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPoint, canvas.worldCamera, out canvasPos);

            // Set the position of the interact button
            timeText.GetComponent<RectTransform>().anchoredPosition = canvasPos;
            timeText.GetComponent<TextMeshProUGUI>().text = Mathf.Round(time).ToString();

            if (time < 0.1f)
            {
                timeText.SetActive(false);
            }
            else
            {
                timeText.SetActive(true);
            }
        }
    }


    public void SetInteractPoint(Transform interactPoint = null)
    {
        lastInteractPoint = interactPoint;
    }

    public void HideInteraction()
    {
        interactionPrompt.SetActive(false);
        timeText.SetActive(false);
    }

}
