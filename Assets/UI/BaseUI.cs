using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseUI<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    public GameObject transformObject;

    protected Canvas canvas;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this as T;
            canvas = GetComponent<Canvas>();
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Toggle(bool toggle)
    {
        canvas.enabled = toggle;
    }

    public virtual GameObject GetFirstButton()
    {
        GameObject buttonObject = GetComponentInChildren<Button>().gameObject;
        return buttonObject;
    }
}