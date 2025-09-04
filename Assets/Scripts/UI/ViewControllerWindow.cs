using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ViewControllerWindow : MonoBehaviour
{
    public static ViewControllerWindow Instance;


    private VisualElement _root;
    private Label _debugText;
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    private void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _debugText = _root.Q<Label>("DebugLabelText");
    }

    public void SetInfoText(string text)
    {
        _debugText.text = text;
    }
}
