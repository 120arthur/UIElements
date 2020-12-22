using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class Icons : EditorWindow
{
    [MenuItem("Objects/Open _%#T")]
    public static void ShowExample()
    {
        Icons wnd = GetWindow<Icons>();
        wnd.titleContent = new GUIContent("Objects ");
    }

    public void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

     
        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Icons.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        Button botao = root.Q<Button>("Button");

        FloatField flatfield = root.Q<VisualElement>("oi").Q<FloatField>("opa");

        botao.RegisterCallback<ClickEvent>(
            e =>
            {
                Debug.Log("clickou");
            });

        flatfield.RegisterCallback<ChangeEvent<float>>(
            e=> {
                Debug.Log(flatfield.value);
            });

    }

}