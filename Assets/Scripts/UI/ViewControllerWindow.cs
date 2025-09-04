using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class ViewControllerWindow : MonoBehaviour
{
    public static ViewControllerWindow Instance;
    public event Action<String> OnDropdown;

    [SerializeField] private GeometryManager geometryManager;

    private VisualElement _root;
    private Label _debugText;
    private Button _generateButton;
    private DropdownField _dropdownField;
    private TextField _inputField;
    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    private void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _debugText = _root.Q<Label>("DebugLabelText");
        _generateButton = _root.Q<Button>("GenerateButton");
        _dropdownField =  _root.Q<DropdownField>("DropDown");
        Debug.Log($"IsDropDown null ? {_dropdownField == null}");
        _inputField = _root.Q<TextField>("URLField");
        _dropdownField.choices = new List<string>();
        _dropdownField.choices.Add("Display all");
        _dropdownField.choices.Add("Translucent all");
        _dropdownField.RegisterValueChangedCallback((evt) => OnDropdown?.Invoke(evt.newValue));
        _generateButton.clicked += OnGenerateButtonClicked;
    }

    

    private void OnDestroy()
    {
        _generateButton.clicked -= OnGenerateButtonClicked;
        _dropdownField.UnregisterValueChangedCallback((evt) => OnDropdown?.Invoke(evt.newValue));
    }

    private void OnGenerateButtonClicked()
    {
        
        geometryManager.GenerateFromURL(_inputField.value);
    }

    public void SetInfoText(string text)
    {
        _debugText.text = text;
    }

    public void UpdateDropdown(GameObject gameObject)
    {
        _dropdownField.choices.Add(gameObject.name);
    }
}
