using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityPackage.Editor
{
    [Serializable]
    public class ExpansionPackageInfo
    {
        public string name;
        public string displayName;
        public string description;
        public string gitUrl;
        public string version;
        public string video_url;
        public string quick_start;
        public string api;
        public string change_log;
        public string author;

        [Tooltip("这里的依赖仅仅是用来在拓展包列表界面显示,没有其他任何实际作用")]
        public List<string> dependencies;

        // XFABManager 和 XFGameFramework 不能移除
        public bool cannot_remove = false;

        /// <summary>
        /// 最低支持的Unity版本
        /// </summary>
        public string min_unity_version = "2019.4.40";

        /// <summary>
        /// 是否在列表中显示 默认:true
        /// </summary>
        public bool show_in_list = true;

        /// <summary>
        /// 是否是预览包 默认:false
        /// </summary>
        public bool preview;
    }

    // [Serializable]
    // public class GameStoreInfo 
    // {
    //     [Header("商店名称")]
    //     [Tooltip("商店名称")]
    //     public string name;
    //
    //     [Header("商店地址")]
    //     [Tooltip("商店地址")]
    //     public string url;
    // }
    //
    // [Serializable]
    // public class GameInfo
    // {
    //     [Header("游戏名称")]
    //     [Tooltip("游戏名称")]
    //     public string name;
    //
    //     [Header("商店信息")]
    //     [Tooltip("商店信息")]
    //     public List<GameStoreInfo> gameStoreInfos;
    //     
    // }

    public class ExpansionPackages : ScriptableObject
    {
        // Fix编码  
        public List<ExpansionPackageInfo> packages = new List<ExpansionPackageInfo>();

        private Dictionary<int, ExpansionPackageInfo> packages_dic = new Dictionary<int, ExpansionPackageInfo>();

        [HideInInspector] public List<string> need_add_gits = new List<string>();


        [HideInInspector] public List<string> need_removes = new List<string>();


        // public List<GameInfo> gameInfos;

        internal ExpansionPackageInfo Get(int id)
        {
            if (packages_dic.ContainsKey(id))
                return packages_dic[id];

            foreach (ExpansionPackageInfo package in packages)
            {
                if (package.name.GetHashCode() == id)
                {
                    packages_dic.Add(id, package);
                    return package;
                }
            }


            return null;
        }


        internal ExpansionPackageInfo Get(string name)
        {
            return Get(name.GetHashCode());
        }

        internal void Save()
        {
            EditorUtility.SetDirty(this);
        }
    }
}