using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EasyAI.Navigation.Nodes
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "Easy-AI/Lookup Table", fileName = "Lookup Table", order = 0)]
    public class LookupTable : ScriptableObject
    {
        public NavigationLookup[] Read => data?.ToArray();
        
        [Tooltip("Navigation data.")]
        [SerializeField]
        private NavigationLookup[] data;

        public void Write(IEnumerable<NavigationLookup> write)
        {
            data = write.ToArray();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}