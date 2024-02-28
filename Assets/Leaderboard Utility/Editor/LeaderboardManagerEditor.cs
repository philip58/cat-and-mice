using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LeaderboardManager))]
public class LeaderboardManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LeaderboardManager myLeaderboard = (LeaderboardManager)target;
        if (GUILayout.Button("Delete All Highscores"))
        {
            myLeaderboard.DeleteHighscores();
        }
    }
}
