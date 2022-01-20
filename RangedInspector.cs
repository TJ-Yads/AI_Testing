using UnityEditor;
/// <summary>
/// this inspector scipt changes the data presented in the inspector for the CS_Ranged script
/// variables are seperated into base data-- anything that all AI of this type need to have
/// auto find data-- anything that is public but not changed in the inspector, this is also shown for testing purposes
/// extra combat abilites-- any combat actions that go beyond what the AI does by default such as using the SA_Ranged script
/// </summary>
[CustomEditor(typeof(CS_Ranged))]
public class RangedInspector : Editor
{
    public enum DisplayCategory//declare an enum to hold the 3 major categories
    {
        BaseData, AutoFindData, ExtraCombatAbilities
    }


    public DisplayCategory categoryToDisplay;
    //function to run the editor
    public override void OnInspectorGUI()
    {
        //display enum popup
        categoryToDisplay = (DisplayCategory)EditorGUILayout.EnumPopup("Display", categoryToDisplay);

        //create space for popup
        EditorGUILayout.Space();

        //switch statment to manage the swapping of categories
        switch (categoryToDisplay)
        {
            case DisplayCategory.BaseData:
                DisplayBaseData();
                break;

            case DisplayCategory.AutoFindData:
                DisplayAutoData();
                break;

            case DisplayCategory.ExtraCombatAbilities:
                DisplayExtraData();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
    //each function will display the diffrent groups of info
    void DisplayBaseData()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fireInXs"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("accuracy"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("needLOS"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("baseRotationSpeed"));

        EditorGUILayout.Space();

        //an extra variable option for AI if the bool is true then the other variables that correspond to it is shown
        SerializedProperty showRotationProperty = serializedObject.FindProperty("slowAimBeforeFire");
        EditorGUILayout.PropertyField(showRotationProperty);
        if (showRotationProperty.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("slowDuration"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("slowRotationSpeed"));
        }

        EditorGUILayout.Space();

        //declare and present the 3 major bools the relate to what projectile type is firing
        SerializedProperty showMovingProperty = serializedObject.FindProperty("fire_MovingOBJ");
        SerializedProperty showRaycastProperty = serializedObject.FindProperty("fire_Raycast");
        SerializedProperty showDumbProperty = serializedObject.FindProperty("fire_Dumb");

        //when a bool is chosen for how an AI fires the other options are hidden and the related variables to that firing type is shown
        if(!showRaycastProperty.boolValue && !showDumbProperty.boolValue)
        {
            EditorGUILayout.PropertyField(showMovingProperty);
            if (showMovingProperty.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("projectileOBJ"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("projectileLifeTime"));
            }
        }

        EditorGUILayout.Space();

        if(!showMovingProperty.boolValue && !showDumbProperty.boolValue)
        {
            EditorGUILayout.PropertyField(showRaycastProperty);
            if (showRaycastProperty.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rayDistance"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("trailOBJ"));
            }
        }

        EditorGUILayout.Space();

        if(!showMovingProperty.boolValue && !showRaycastProperty.boolValue)
        {
            EditorGUILayout.PropertyField(showDumbProperty);
            if (showDumbProperty.boolValue)
            {

            }
        }

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("projectileSpawnPoint"));
    }

    void DisplayAutoData()//the variables that are found at run time are shown here
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("aimTarget"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("baseAI"));
    }

    void DisplayExtraData()//any variable relative to special combat functions is shown here
    {
        //if the ability to use these attacks is true then it will show what attack options are available
        SerializedProperty showSpecialProperty = serializedObject.FindProperty("useSpecialAttacks");
        EditorGUILayout.PropertyField(showSpecialProperty);
        if (showSpecialProperty.boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("specialChanceRoll"));
            EditorGUILayout.Space();

            //the AI is designed to allow any special combat function to work with others and each can be enabled for a single AI
            SerializedProperty showVolleyProperty = serializedObject.FindProperty("canVolley");
            EditorGUILayout.PropertyField(showVolleyProperty);
            if (showVolleyProperty.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("volleyDelay"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("volleyCooldown"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("volletCount"));
            }

            EditorGUILayout.Space();

            SerializedProperty showBurstProperty = serializedObject.FindProperty("canBurst");
            EditorGUILayout.PropertyField(showBurstProperty);
            if (showBurstProperty.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("burstCount"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("burstCooldown"));
            }
        }
    }
}
