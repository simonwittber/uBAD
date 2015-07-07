using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BAD
{
    [CustomEditor(typeof(BADReactor))]
    public class BADReactorEditor : Editor
    {
        static BADReactor activeReactor;



        public override void OnInspectorGUI ()
        {
            base.OnInspectorGUI ();
            if (Application.isPlaying) {
                GUILayout.BeginHorizontal ();
                GUILayout.FlexibleSpace ();
                var reactor = target as BADReactor;
                if (GUILayout.Button (reactor.pause ? "Resume" : "Pause", GUILayout.MinWidth (64))) {
                    reactor.pause = !reactor.pause;
                }
                if (GUILayout.Button ("Step", GUILayout.MinWidth (64))) {
                   reactor.step = true;
                }
                if(GUILayout.Button ("Watch", GUILayout.MinWidth(64))) {
                    var window = (BADViewerWindow)EditorWindow.GetWindow (typeof(BADViewerWindow));
                    if(activeReactor != null) activeReactor.debug = false;
                    reactor.debug = true;
                    activeReactor = reactor;
                    window.reactor = reactor;
                    window.Show();
                }
                GUILayout.FlexibleSpace ();
                GUILayout.EndHorizontal ();
				foreach(var i in reactor.blackboard.Items) {
					GUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel(i.Key);
					GUILayout.Label(i.Value.ToString(), "box", GUILayout.Width(128));
					GUILayout.EndHorizontal();
				}
            }
        }
    }
}