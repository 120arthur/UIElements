using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

public class weapon : EditorWindow
{
    [MenuItem("Chose weapon/weapon")]
    public static void ShowExample()
    {
        weapon wnd = GetWindow<weapon>();
        wnd.titleContent = new GUIContent("weapon");
    }

    private PresetWeaponObject queryPrefabWithPresetWeaponScript;
    private PresetWeaponObject presetWeaponManager;
    private PresetWeapon selectedWeaponPreset;


    private TextField nameWeapon;
    private ObjectField ObjectImageReference;
    private ObjectField ObjectToInstantiate;
    private ObjectField PivotInstantiate;
    private Button instantiateButton;

  
    private TextElement NameOfObject;
    private VisualElement ImageContainer;

    private int weaponSelected;
    private bool isEditingAPresset;

    private bool gameObjectIsEmpty = true;
    private bool pivotIsEmpty = true;

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;


        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/weapons UIElement/weapon.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var PressetTemplate = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/weapons UIElement/PresetTemplate.uss");
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/weapons UIElement/weapon.uss");

        root.styleSheets.Add(PressetTemplate);
        root.styleSheets.Add(styleSheet);

        queryPrefabWithPresetWeaponScript = (PresetWeaponObject)AssetDatabase.LoadAssetAtPath("Assets/Editor/Prefab/pressetWeapon.prefab", typeof(PresetWeaponObject));

        presetWeaponManager = queryPrefabWithPresetWeaponScript;
        SetupControls();
        PopulatePresetList();
    }

    private void SetupControls()
    {
        ObjectToInstantiate = rootVisualElement.Q<ObjectField>("ObjectToInstantiate");
        ObjectToInstantiate.objectType = typeof(GameObject);

        ObjectImageReference = rootVisualElement.Q<ObjectField>("SpriteField");
        ObjectImageReference.objectType = typeof(Sprite);

        PivotInstantiate = rootVisualElement.Q<ObjectField>("PivotField");
        PivotInstantiate.objectType = typeof(Transform);

        nameWeapon = rootVisualElement.Q<TextField>("TextField");


        instantiateButton = rootVisualElement.Q<Button>("ButtonInstance");
        instantiateButton.visible = false;

        Button newObject = rootVisualElement.Q<Button>("NewObject");
        Button delectObject = rootVisualElement.Q<Button>("DelectObject");

        //callbacks
        instantiateButton.clickable.clicked += () =>
        {
            presetWeaponManager.InstantiateWeapon();
        };
        newObject.clickable.clicked += () =>
        {
            if (presetWeaponManager != null)
            {
                PresetWeapon newPressetWeapon = new PresetWeapon();
                presetWeaponManager.presetsWeapon.Add(newPressetWeapon);

                EditorUtility.SetDirty(presetWeaponManager);

                PopulatePresetList();
                BindControls();
            }

        };
        delectObject.clickable.clicked += () =>
        {
            presetWeaponManager.presetsWeapon.Remove(selectedWeaponPreset);
            PopulatePresetList();
            BindControls();

        };
    }

    private void PopulatePresetList()
    {
        ListView list = rootVisualElement.Q<ListView>("ListView");
        list.Clear();

        //Keeping track of what button links to each preset index
        Dictionary<Button, int> buttonDictionary = new Dictionary<Button, int>();

        if (presetWeaponManager == null)
        {
            return;
        }
        for (int i = 0; i < presetWeaponManager.presetsWeapon.Count; i++)
        {


            VisualElement listContainer = new VisualElement();
            listContainer.name = "ListContainer";

            Button listButton = new Button();
            listButton.AddToClassList("ListButton");

            NameOfObject = new TextElement();
            NameOfObject.AddToClassList("ListText");

            ImageContainer = new VisualElement();
            ImageContainer.AddToClassList("ImageContainer");

            if (presetWeaponManager.presetsWeapon[i].weaponName == "")
            {
                NameOfObject.text = "Empty Object";
            }
            else
            {
                NameOfObject.text = presetWeaponManager.presetsWeapon[i].weaponName;
            }

            if (presetWeaponManager.presetsWeapon[i].weaponImage == null)
            {
                ImageContainer.style.backgroundImage = presetWeaponManager.emptyWeapon.texture;
            }
            else
            {
                ImageContainer.style.backgroundImage = presetWeaponManager.presetsWeapon[i].weaponImage.texture;
            }

            listContainer.Add(listButton);
            listContainer.Add(NameOfObject);
            listButton.Add(ImageContainer);

            list.Insert(list.childCount, listContainer);

            //Keep track of what index the button is pressing
            if (!buttonDictionary.ContainsKey(listButton))
            {
                buttonDictionary.Add(listButton, i);
            }


            //Assigning action to the button
            listButton.clickable.clicked += () =>
            {
                if (queryPrefabWithPresetWeaponScript != null)
                {
                    LoadPreset(buttonDictionary[listButton]);
                }
            };

            if (selectedWeaponPreset == presetWeaponManager.presetsWeapon[buttonDictionary[listButton]])
            {

                listButton.style.backgroundColor = new StyleColor(new Color(0.2f, 0.2f, 0.2f, 1f));
                weaponSelected = i;
                isEditingAPresset = true;
            }
            else
            {
                isEditingAPresset = false;
            }

        }
    }

    private void LoadPreset(int elementID)
    {
        if (presetWeaponManager != null)
        {
            selectedWeaponPreset = presetWeaponManager.presetsWeapon[elementID];
            presetWeaponManager.CurrentlyEditing = selectedWeaponPreset;

            PopulatePresetList();

            if (selectedWeaponPreset != null)
            {
                BindControls();
            }
        }
        else
        {
            PopulatePresetList();
        }

    }

    private void BindControls()
    {
        //connecting currentlyEditing with the UI
        nameWeapon.value = presetWeaponManager.CurrentlyEditing.weaponName;
        nameWeapon.RegisterCallback<ChangeEvent<string>>(e =>
        {
            presetWeaponManager.CurrentlyEditing.weaponName = nameWeapon.value;
            if (isEditingAPresset == true)
            {
                NameOfObject.text = presetWeaponManager.presetsWeapon[weaponSelected].weaponName;
            }

        });

        ObjectToInstantiate.value = presetWeaponManager.CurrentlyEditing.weapon;
        ObjectToInstantiate.RegisterCallback<ChangeEvent<Object>>(e =>
        {
            presetWeaponManager.CurrentlyEditing.weapon = (GameObject)ObjectToInstantiate.value;

            if (ObjectToInstantiate.value != null)
            {
                gameObjectIsEmpty = false;
            }
            else
            {
                gameObjectIsEmpty = true;
            }
            ButonVisibility(instantiateButton);

        });

        ObjectImageReference.value = presetWeaponManager.CurrentlyEditing.weaponImage;
        ObjectImageReference.RegisterCallback<ChangeEvent<Object>>(e =>
        {
            presetWeaponManager.CurrentlyEditing.weaponImage = (Sprite)ObjectImageReference.value;
            if (isEditingAPresset == true)
            {

                if (presetWeaponManager.presetsWeapon[weaponSelected].weaponImage != null)
                {

                    ImageContainer.style.backgroundImage = presetWeaponManager.presetsWeapon[weaponSelected].weaponImage.texture;
                }
            }

        });


        PivotInstantiate.value = presetWeaponManager.CurrentlyEditing.weaponTansform;
        PivotInstantiate.RegisterCallback<ChangeEvent<Object>>(e =>
        {
            presetWeaponManager.CurrentlyEditing.weaponTansform = (Transform)PivotInstantiate.value;

            if (PivotInstantiate.value != null)
            {
                pivotIsEmpty = false;
            }
            else
            {
                pivotIsEmpty = true;
            }
            ButonVisibility(instantiateButton);
        });

    }

    private void ButonVisibility(Button instantiateButton)
    {   // Check if the "instantiate" button can be activated
        if (gameObjectIsEmpty == true || pivotIsEmpty == true)
        {
            instantiateButton.visible = false;
        }
        else if (gameObjectIsEmpty == false && pivotIsEmpty == false)
        {
            instantiateButton.visible = true;
        }
    }

    private void OnGUI()
    {
        //Set the container height to the window
        rootVisualElement.Q<VisualElement>("container").style.height = new StyleLength(position.height);

    }
}