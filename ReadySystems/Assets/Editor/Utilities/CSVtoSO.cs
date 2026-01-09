using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

public class CSVtoSO : EditorWindow
{
    //Script for generating ScriptableObjects from a CSV file
    //Usage: Click on "Tools/CSV to ScriptableObjects" in the Unity Editor menu and fill in the fields then click "Generate"
    //You need csv file as TextAsset and a ScriptableObject script to create instances of
    //Make sure the fields in the ScriptableObject script match the order and types of the columns in the CSV file
    //Supported types in so: int, float, string, bool, sprite (for sprite, provide the asset path in the csv) || if you need more types, extend the code in GenerateSO method

    [SerializeField] 
    private TextAsset csv; //CSV file as TextAsset
    [SerializeField] 
    private MonoScript soScript; //ScriptableObject script
    [SerializeField] 
    private string outputFolder = "Assets/Data/Generated"; //Output folder for generated ScriptableObjects
    [SerializeField] 
    private string separator = ";"; //Separator used in the CSV file
    [SerializeField] 
    private bool hasHeader = true; //Whether the CSV file has a header row, if true, the first row will be skipped

    [MenuItem("Tools/CSV to ScriptableObjects")]
    public static void Open() => GetWindow<CSVtoSO>("CSV -> SO");

    private void OnGUI()
    {
        csv = (TextAsset)EditorGUILayout.ObjectField("CSV (TextAsset)", csv, typeof(TextAsset), false);
        soScript = (MonoScript)EditorGUILayout.ObjectField("SO Script", soScript, typeof(MonoScript), false);

        separator = EditorGUILayout.TextField("Separator", separator);
        hasHeader = EditorGUILayout.Toggle("Has Header", hasHeader);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);

        using(new EditorGUI.DisabledScope(csv == null || soScript == null))
        {
            if(GUILayout.Button("Generate"))
            {
                GenerateSO();
            }
        }
    }

    private void GenerateSO()
    {
        string[] lines = csv.text.Split(new[] { "\r\n", "\n", "\r" }, System.StringSplitOptions.None);
        int startIndex = hasHeader ? 1 : 0;

        for(int i = startIndex; i < lines.Length; i++)
        {
            string line = lines[i];
            
            string[] split = line.Split(separator);

            if(string.IsNullOrWhiteSpace(line)) return;

            var so = ScriptableObject.CreateInstance(soScript.GetClass());
            var fields = GetSerializableFieldsInOrder(so);

            for(int j = 0; j < fields.Length && j < split.Length; j++)
            {
                var field = fields[j];
                object value = null;
                if(field.FieldType == typeof(int))
                {
                    if(int.TryParse(split[j], out int intValue))
                    {
                        value = intValue;
                    }
                }
                else if(field.FieldType == typeof(float))
                {
                    if(float.TryParse(split[j], out float floatValue))
                    {
                        value = floatValue;
                    }
                }
                else if(field.FieldType == typeof(string))
                {
                    value = split[j];
                }
                else if(field.FieldType == typeof(bool))
                {
                    if(bool.TryParse(split[j], out bool boolValue))
                    {
                        value = boolValue;
                    }
                }
                else if (field.FieldType == typeof(Sprite))
                {
                    string p = split[j].Trim();
                    var spr = AssetDatabase.LoadAssetAtPath<Sprite>(p);
                    if (spr != null)
                    {
                        value = spr;
                    }
                    else
                    {
                        Debug.LogWarning($"Could not load Sprite at path: {p}");
                    }
                }
                if (value != null)
                {
                    field.SetValue(so, value);
                }
            }
            AssetDatabase.CreateAsset(so, $"{outputFolder}/{split[0]}.asset");
        }
    }

    public static FieldInfo[] GetSerializableFieldsInOrder(ScriptableObject so)
    {
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        return so.GetType()
            .GetFields(flags)
            .Where(f => !f.IsStatic)
            .Where(f => f.IsPublic || f.GetCustomAttribute<SerializeField>() != null)
            .Where(f => f.GetCustomAttribute<NonSerializedAttribute>() == null)
            .OrderBy(f => f.MetadataToken)
            .ToArray();
    }

    public static object GetValue(ScriptableObject so, int index)
    {
        var fields = GetSerializableFieldsInOrder(so);
        if (index < 0 || index >= fields.Length) 
        { 
            return null; 
        }
        return fields[index].GetValue(so);
    }

    public static void SetValue(ScriptableObject so, int index, object value)
    {
        var fields = GetSerializableFieldsInOrder(so);
        if(index < 0 || index >= fields.Length) 
        {
            return;
        }
        fields[index].SetValue(so, value);
    }
}
