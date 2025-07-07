using System;
using UnityEngine;

namespace UnityPackage.Editor
{
    [Serializable]
    public class PackageDependency : ScriptableObject
    {
        [SerializeField] private string dependencyName;
        [SerializeField] private string dependencyVersion;

        public PackageDependency()
        {
            DependencyName = "";
            DependencyVersion = "";
        }

        public string DependencyName
        {
            get => dependencyName;
            set => dependencyName = value;
        }

        public string DependencyVersion
        {
            get => dependencyVersion;
            set => dependencyVersion = value;
        }
    }
}