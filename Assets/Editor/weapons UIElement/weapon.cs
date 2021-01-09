using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class weapon : EditorWindow
{
    private bool gameObjectIsEmpty = true;
    private bool pivotIsEmpty = true;


    [MenuItem("Chose weapon/weapon")]
    public static void ShowExample()
    {
        weapon wnd = GetWindow<weapon>();
        wnd.titleContent = new GUIContent("weapon");
    }

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

        //geting objects
   
        ObjectField ObjectToInstantiate = rootVisualElement.Q<ObjectField>("ObjectToInstantiate");
        ObjectToInstantiate.objectType = typeof(GameObject);

        ObjectField ObjectImageReference = rootVisualElement.Q<ObjectField>("SpriteField");
        ObjectImageReference.objectType = typeof(Texture2D);

        ObjectField PivotInstantiate = rootVisualElement.Q<ObjectField>("PivotField");
        PivotInstantiate.objectType = typeof(Transform);

        Button instantiateButton = rootVisualElement.Q<Button>("ButtonInstance");
        instantiateButton.visible = false;

        //callbacks

        ObjectToInstantiate.RegisterCallback<ChangeEvent<Object>>(e =>
        {
            if (ObjectToInstantiate.value != null)
            {
                gameObjectIsEmpty = false;
                Debug.Log("objeto não esta vazio");
            }
            else
            {
                gameObjectIsEmpty = true;
                Debug.Log("objeto esta vazio");
            }
            ButonVisibility(instantiateButton);
        });

        PivotInstantiate.RegisterCallback<ChangeEvent<Object>>(e =>
        {
            if (PivotInstantiate.value != null)
            {
                pivotIsEmpty = false;
                Debug.Log("objeto não esta vazio");
            }
            else
            {
                pivotIsEmpty = true;
                Debug.Log("objeto esta vazio");
            }
            ButonVisibility(instantiateButton);
        });
        list();
        list();
        list();
        list();
        list();
        list();
    }

    void list()
    {
        ListView list = (ListView)rootVisualElement.Q<ListView>("ListView");
        
        VisualElement listContainer = new VisualElement();
        listContainer.name = "ListContainer";

        Button listButton = new Button();
        listButton.AddToClassList("ListButton");

        TextElement NameOfObject = new TextElement();
        NameOfObject.AddToClassList("ListText");
        NameOfObject.text = "Empty";
        
        VisualElement ImageContainer = new VisualElement();
        ImageContainer.AddToClassList("ImageContainer"); 
        



        listContainer.Add(listButton);
        listContainer.Add(NameOfObject);
        listButton.Add(ImageContainer);
        list.Insert(list.childCount, listContainer);
    }

    private void ButonVisibility(Button instantiateButton)
    {
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