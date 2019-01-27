using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WallBuilder : EditorWindow
{
    LineRenderer LineRender;

    [MenuItem("Window/WallGenerator")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(WallBuilder));
    }

    private void Awake()
    {
        LineRender = new GameObject().AddComponent<LineRenderer>();
    }

    private GameObject _spawnPrefab;
    float _distance;

    void OnGUI()
    {
        // The actual window code goes here
        _spawnPrefab = (GameObject)EditorGUILayout.ObjectField(_spawnPrefab, typeof(GameObject), true);
        _distance = EditorGUILayout.FloatField("Distance", _distance);

        EditorGUILayout.LabelField("Press button when finish!");
        if(GUILayout.Button("Generate!"))
        {
            Vector3[] LineDots = new Vector3[LineRender.positionCount];
            LineRender.GetPositions(LineDots);

            Debug.Log("Dots: " + LineDots.Length);

            //Schleife
            float currentDistance = 0;
            float DistanceBetweenDots;
            GameObject lastObj = null;

            for(int i = 0; i < LineDots.Length-1;)
            {
                DistanceBetweenDots = Vector3.Distance(LineDots[i], LineDots[i + 1]);

                float val = currentDistance / DistanceBetweenDots;

                //Instantiate
                GameObject wall = Instantiate(_spawnPrefab);
                wall.transform.parent = LineRender.transform;

                //Pos
                wall.transform.position = Vector3.Lerp(LineDots[i], LineDots[i + 1], val);

                //Rot
                if (lastObj != null)
                lastObj.transform.LookAt(wall.transform.position);

                lastObj = wall;
                //wall.transform.LookAt(LineDots[i + 1]);

                currentDistance += _distance;
                if(currentDistance > DistanceBetweenDots)
                {
                    i++;
                    currentDistance -= DistanceBetweenDots;
                }
            }
            //Dots Auswerten, länge bestimmen, Rotation festlegen, Wände erzeugen
        }
    }
}
