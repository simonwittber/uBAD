using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BAD
{
    public class BADViewerWindow : EditorWindow
    {
        static BADViewerWindow window;
        public BADReactor reactor;
        Vector2 scrollPosition;
        Rect cursor;
        List<Connector> connectors = new List<Connector> ();

        void DrawNode (Node node)
        {
            GUI.color = node.running ? Color.yellow : Color.white;
            if (node.state != null) {
                var icon = cursor;
                icon.x -= 14;
                icon.width = 14;
                var color = GUI.color;
                GUI.color = node.state == NodeResult.Success ? Color.green : node.state == NodeResult.Continue ? Color.yellow : Color.red;
                GUI.Label (icon, "", EditorStyles.radioButton);
                GUI.color = color;
                
            }
            var text = string.Format ("{0}", node);
            cursor.width = GUI.skin.button.CalcSize (new GUIContent (text)).x;

            GUI.Label (cursor, text, "button");
        }

        void DrawDecoratorNode (Node node)
        {
            DrawNode (node);
        }

        void DrawBranchNode (Node node)
        {
            DrawNode (node);
        }

        void DrawGraph (Node node, Vector2 parentPosition)
        {
            var midY = Mathf.Lerp (cursor.yMax, cursor.yMin, 0.5f);
            var x = parentPosition.x;
            var A = new Vector2 (cursor.xMin, midY);
            var B = new Vector2 (x, midY);
            var C = parentPosition;
            if (node.running) {
                connectors.Add (new Connector (A, B, C));
            } else {
                Handles.color = Color.white;
                Handles.DrawPolyLine (A, B, C);
            }


            if (node is Decorator) {
                var decorator = node as Decorator;
                DrawDecoratorNode (node);
                parentPosition = new Vector2 (cursor.xMax, Mathf.Lerp (cursor.yMin, cursor.yMax, 0.5f));
                var indent = cursor.width + 16; 
                cursor.x += indent;
                foreach (var child in decorator.children) {
                    DrawGraph (child, parentPosition);
                }
                cursor.x -= indent;
            } else if (node is Branch) {
                var branch = node as Branch;
                DrawBranchNode (node);
                parentPosition = new Vector2 (Mathf.Lerp (cursor.xMin, cursor.xMax, 0.25f), cursor.yMax);
                var indent = (cursor.width / 4) + 16;
                cursor.x += indent;
                cursor.y += cursor.height + 4;
                foreach (var child in branch.children) {
                    DrawGraph (child, parentPosition);
                }
                cursor.x -= indent;
            } else {
                DrawNode (node);
                cursor.y += cursor.height + 4;
            }

        }

        void OnDrawGUI ()
        {
            cursor = new Rect (20, 20, 160, 18);
            connectors.Clear ();
            if (reactor != null) {
                foreach (var node in reactor.runningGraphs) {
                    DrawGraph (node, new Vector2 (20, 20));
                }
            }
            foreach (var c in connectors) {
                Handles.color = Color.green;
                Handles.DrawPolyLine (c.A, c.B, c.C);
            }
        }

        void OnGUI ()
        { 
            GUILayout.BeginHorizontal ();
            GUILayout.FlexibleSpace ();
            EditorGUILayout.ObjectField (reactor, typeof(BADReactor), true);
            GUILayout.EndHorizontal ();
            OnDrawGUI ();

        }

        void Update ()
        {
            if (reactor != null && Application.isPlaying) {
                Repaint ();
            }
        }

        class Connector
        {
            public Vector2 A, B, C;

            public Connector (Vector2 A, Vector2 B, Vector2 C)
            {
                this.A = A;
                this.B = B;
                this.C = C;
            }
        }

    }

}