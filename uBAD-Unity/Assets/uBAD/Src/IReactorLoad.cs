using UnityEngine;
using System.Collections;

namespace BAD
{
    interface IReactorLoad
    {
        /// <summary>
        /// This method must return a root node.
        /// </summary>
        /// <returns>The root node.</returns>
        Branch LoadRootNode();
    }
}