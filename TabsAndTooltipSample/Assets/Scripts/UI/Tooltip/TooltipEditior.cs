using UnityEditor;

// This class is a custom editor allowing creation of and selection
// of different types of tool tips within the unity editor in the
// inspector. This attaches to the 'TooltipTrigger' script and allows
// for fields to be set depending on the selected tool tip type
[CustomEditor(typeof(TooltipTrigger))]
public class TooltipEditior : Editor {
    SerializedProperty tooltipTypeProp;

    // Text tooltip properites
    SerializedProperty headerProp;
    SerializedProperty contentProp;

    // Image tooltip properties
    SerializedProperty nameProp;
    SerializedProperty infoProp;
    SerializedProperty imageProp;

    private void OnEnable() {
        tooltipTypeProp = serializedObject.FindProperty("tooltipType");

        headerProp = serializedObject.FindProperty("header");
        contentProp = serializedObject.FindProperty("content");

        nameProp = serializedObject.FindProperty("componentName");
        infoProp = serializedObject.FindProperty("componentInfo");
        imageProp = serializedObject.FindProperty("componentImage");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(tooltipTypeProp);

        TooltipTrigger.ToolTipType type = (TooltipTrigger.ToolTipType)tooltipTypeProp.enumValueIndex;

        switch (type) {
            case TooltipTrigger.ToolTipType.Text:
                EditorGUILayout.PropertyField(headerProp);
                EditorGUILayout.PropertyField(contentProp);
                break;
            case TooltipTrigger.ToolTipType.Image:
                EditorGUILayout.PropertyField(nameProp);
                EditorGUILayout.PropertyField(infoProp);
                EditorGUILayout.PropertyField(imageProp);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
