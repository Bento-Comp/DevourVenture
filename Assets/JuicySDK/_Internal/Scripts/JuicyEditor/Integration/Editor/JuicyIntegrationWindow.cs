using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace JuicyInternal
{
    public class JuicyIntegrationWindow : EditorWindow
    {
        static List<JuicyIntegrationReport> reports = new List<JuicyIntegrationReport>();
        static int errorAmount;
        static int warningAmount;
        static GUILayoutOption[] standardButton = { GUILayout.Width(100), GUILayout.Height(20) };

        Vector2 scrollPos;

        public static void ShowWindow(bool closeIfNoErrors = false)
        {
            GatherReports();
            if (errorAmount == 0 && warningAmount == 0 && closeIfNoErrors)
                return;
            EditorWindow.GetWindow(typeof(JuicyIntegrationWindow), true, "Juicy Integration Report");
        }

        static Texture2D GetTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        private void OnFocus()
        {
            GatherReports();
        }

        void OnGUI()
        {
            minSize = new Vector2(600, 400);
            maxSize = new Vector2(600, 400);
            DrawHeader();
            EditorGUILayout.Space();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox, GUILayout.Height(280));

            foreach (JuicyIntegrationReport report in reports)
                DrawReport(report);

            EditorGUILayout.EndScrollView();
            EditorGUILayout.Space();
            DrawFooter();
        }

        void DrawHeader()
        {
            EditorGUILayout.Space();

            //Background color
            GUIStyle style = new GUIStyle();
            Texture2D texture;
            if (errorAmount > 0)
                texture = GetTexture(new Color(1, 0, 0, .5f));
            else if (warningAmount > 0)
                texture = GetTexture(new Color(1, .85f, .15f, .5f));
            else
                texture = GetTexture(new Color(0, .8f, 0, .5f));
            style.normal.background = texture;


            GUILayout.BeginVertical(style,GUILayout.Height(50));
            GUILayout.FlexibleSpace();
            if (errorAmount > 0)
            {
                GUILayout.BeginHorizontal(EditorGUIUtility.IconContent("console.erroricon"), new GUIStyle());
                EditorGUILayout.LabelField("            " + errorAmount + " errors", EditorStyles.boldLabel);
                GUILayout.EndHorizontal();
            }

            if (warningAmount > 0)
            {
                GUILayout.BeginHorizontal(EditorGUIUtility.IconContent("console.warnicon"), new GUIStyle());
                EditorGUILayout.LabelField("            " + warningAmount + " warnings", EditorStyles.boldLabel);
                GUILayout.EndHorizontal();
            }
            
            if(warningAmount == 0 && errorAmount == 0)
            {
                GUILayout.BeginHorizontal(EditorGUIUtility.IconContent("Collab"), new GUIStyle());
                EditorGUILayout.LabelField("            Everything is set up properly", EditorStyles.boldLabel);
                GUILayout.EndHorizontal();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        void DrawReport(JuicyIntegrationReport report)
        {
            if (report.IsEmpty)
                return;

            foreach (JuicyIntegrationReportCategory category in report.Categories)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(category.Name, EditorStyles.boldLabel);
                foreach (JuicyIntegrationReportItem item in category.items)
                {
                    GUILayout.BeginHorizontal(EditorGUIUtility.IconContent(item.isError ? "console.erroricon" : "console.warnicon"), new GUIStyle());
                    EditorGUILayout.LabelField(new GUIContent("         " + item.report, item.fix));
                    GUILayout.EndHorizontal();
                }
                EditorGUILayout.Space();
            }
        }

        void DrawFooter()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close", standardButton))
                Close();
            EditorGUILayout.EndHorizontal();
        }

        static void GatherReports()
        {
            reports.Clear();

            reports.Add(new JuicySettingsIntegrationReport());
            reports.Add(new JuicyConfigIntegrationReport());
            reports.Add(new JuicyProjectIntegrationReport());

#if !noJuicyCompilation
            reports.Add(new JuicyMediationIntegrationReport());
#endif

            warningAmount = 0;
            errorAmount = 0;
            foreach(JuicyIntegrationReport report in reports)
            {
                warningAmount += report.warningAmount;
                errorAmount += report.errorAmount;
            }
        }
    }
}
