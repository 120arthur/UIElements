

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class PresetWindow : EditorWindow
{
    [MenuItem("RW/Preset Window")]
    public static void ShowExample()
    {
        PresetWindow wnd = GetWindow<PresetWindow>();
        wnd.titleContent = new GUIContent("PresetWindow");
    }


    private PresetObject presetManager;
    private SerializedObject presetManagerSerialized;
    private Preset selectedPreset;
    private ObjectField presetObjectField;

    public void OnEnable()
    {        
        //Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        //Getting a reference to the UXML Document and adding to the root
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/RW/Editor/PresetWindow.uxml");
        VisualElement uxmlRoot = visualTree.CloneTree();
        root.Add(uxmlRoot);

        //Getting a reference to the USS Document and adding stylesheet to the root
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/RW/Editor/PresetWindow.uss");
        var preMadeStyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/RW/Editor/PresetTemplate.uss");
        
        root.styleSheets.Add(styleSheet);
        root.styleSheets.Add(preMadeStyleSheet);

        //Getting a reference to a preset object
        presetObjectField = root.Q<ObjectField>("ObjectField");
        presetObjectField.objectType = typeof(PresetObject);

        //Everytime the object field changes in value, assign the presetManager objec
        presetObjectField.RegisterCallback<ChangeEvent<Object>>(e =>
        {
            if (presetObjectField.value != null)
            {
                presetManager = (PresetObject)presetObjectField.value;
                presetManagerSerialized = new SerializedObject(presetManager);
            }

            PopulatePresetList();

        });


        PopulatePresetList();
        SetupControls();

    }

    private void SetupControls()
    {
        //Finding the buttons
        Button newButton = rootVisualElement.Q<Button>("NewButton");
        Button clearButton = rootVisualElement.Q<Button>("ClearButton");
        Button deleteButton = rootVisualElement.Q<Button>("DeleteButton");

        //Registering actions
        newButton.clickable.clicked += () =>
        {
            if (presetManager != null)
            {
                Preset newPreset = new Preset();
                presetManager.presets.Add(newPreset);

                EditorUtility.SetDirty(presetManager);

                PopulatePresetList();
                BindControls();
            }
        };

        clearButton.clickable.clicked += () =>
        {
            if (presetManager != null && selectedPreset != null)
            {
                selectedPreset.color = Color.black;
                selectedPreset.animationSpeed = 1;
                selectedPreset.objectName = "Unnamed Preset";
                selectedPreset.isAnimating = true;
                selectedPreset.rotation = Vector3.zero;
                selectedPreset.size = Vector3.one;
            }
        };

        deleteButton.clickable.clicked += () =>
        {
            if (presetManager != null && selectedPreset != null)
            {
                presetManager.presets.Remove(selectedPreset);
                PopulatePresetList();
                BindControls();
            }
        };
    }

    private void PopulatePresetList()
    {
        ListView list = rootVisualElement.Q<ListView>("ListView");
        list.Clear();

        //Keeping track of what button links to each preset index
        Dictionary<Button, int> buttonDictionary = new Dictionary<Button, int>();

        if (presetManager == null)
        {
            return;
        }

        for (int i = 0; i < presetManager.presets.Count; i++)
        {

            VisualElement listContainer = new VisualElement();
            listContainer.name = "ListContainer";

            Label listLabel = new Label(presetManager.presets[i].objectName);

            //Applying a CSS class to an element
            listLabel.AddToClassList("ListLabel");

            Button listButton = new Button();
            listButton.AddToClassList("ListButton");
            listButton.text = "L";


            listContainer.Add(listLabel);
            listContainer.Add(listButton);

            //Inserting element into list
            list.Insert(list.childCount, listContainer);

            //Keep track of what index the button is pressing
            if (!buttonDictionary.ContainsKey(listButton))
            {
                buttonDictionary.Add(listButton, i);
            }


            //Assigning action to the button
            listButton.clickable.clicked += () =>
            {
                if (presetObjectField.value != null)
                {
                    LoadPreset(buttonDictionary[listButton]);
                }
            };

            if (selectedPreset == presetManager.presets[buttonDictionary[listButton]])
            {

                listButton.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f, 1f));

            }

        }
    }

    private void LoadPreset (int elementID)
    {
        if (presetManager != null)
        {
            selectedPreset = presetManager.presets[elementID];
            presetManager.currentlyEditing = selectedPreset;

            PopulatePresetList();

            if (selectedPreset != null)
            {
                BindControls();
            }
        }
        else
        {
            PopulatePresetList();
        }
    }

    private void BindControls ()
    {
        //Finding the properties in the selected preset manager
        SerializedProperty objectName = presetManagerSerialized.FindProperty("currentlyEditing.objectName");
        SerializedProperty objectColor = presetManagerSerialized.FindProperty("currentlyEditing.color");
        SerializedProperty objectSize = presetManagerSerialized.FindProperty("currentlyEditing.size");
        SerializedProperty objectRotation = presetManagerSerialized.FindProperty("crrentlyEditing.rotation");
        SerializedProperty objectAnimationSpeed = presetManagerSerialized.FindProperty("curreuntlyEditing.animationSpeed");
        SerializedProperty objectIsAnimating = presetManagerSerialized.FindProperty("currentlyEditing.isAnimating");

        //Binding those properties to the corresponding element
        rootVisualElement.Q<TextField>("ObjectName").BindProperty(objectName);
        rootVisualElement.Q<ColorField>("ColorField").BindProperty(objectColor);
        rootVisualElement.Q<Vector3Field>("SizeField").BindProperty(objectSize);
        rootVisualElement.Q<Vector3Field>("RotationField").BindProperty(objectRotation);
        rootVisualElement.Q<FloatField>("AnimationSpeedField").BindProperty(objectAnimationSpeed);
        rootVisualElement.Q<Toggle>("IsAnimatingField").BindProperty(objectIsAnimating);
    }

    private void OnGUI()
    {
        //Set the container height to the window
        rootVisualElement.Q<VisualElement>("Container").style.height = new StyleLength(position.height);

    }

}