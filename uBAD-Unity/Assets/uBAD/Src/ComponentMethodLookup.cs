using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace BAD
{

    public class ComponentMethodLookup
    {
        public string A;
        public string B;
        Component component;
        MethodInfo methodInfo;
        
        public ComponentMethodLookup (string[] parts)
        {
            if (parts.Length != 2)
                throw new System.Exception ("Invalid Lookup syntax: " + string.Join (".", parts));
            A = parts [0];
            B = parts [1];
        }

        public object Invoke ()
        {
            return methodInfo.Invoke(component, null);
        }
        
        public void Resolve (GameObject go)
        {
            var componentType = System.Type.GetType (A, true, true);
            component = go.GetComponent (componentType);
            if (component == null) { 
                Debug.LogWarning ("Adding " + componentType + " to gameobject at runtime.");
                component = go.AddComponent (componentType);
            }
            if(component == null) throw new System.Exception("No component named: " + A);
            methodInfo = componentType.GetMethod (B);
            if(methodInfo == null) throw new System.Exception("No method named: " + B + " on " + A + " (Is it public?)");

        }

        public override string ToString ()
        {
            return string.Format ("{0}.{1}", A, B);
        }
        
    }
}