using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor; 
using UnityEditor.IMGUI.Controls;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityPackage.Editor
{

    public class ExpansionPackageDependenciesData 
    {
        public string name;
        public string git_url;
        public string version;
    }

    public class ExpansionPackageDependencies 
    {
        public List<ExpansionPackageDependenciesData> xfgameframework_dependencies;
    }

    public class ExpansionWindow : SplitWindow
    {
        // Fix编码

        #region 字段

        private ExpansionPackages _packages = null;
        //private Rect left;
        private Rect right_bottom;

        //private Vector2 scrollViewLeft;
        private Vector2 scrollViewRight;

        private TreeViewState packageListState;

        private ExpansionPackageListTree packageListTree;
         
        private ListRequest requestList;

        private int index;
        private float timer;

        private ExpansionPackageInfo currentShowInfo;

        private GUIStyle storeStyle;

        private GUIStyle headerStyle;
        
        private GUIStyle nameStyle;

        private GUIStyle linkStyle;

        private GUIStyle desStyle;

        private Dictionary<string, SearchRequest> searchRequests = new Dictionary<string, SearchRequest>();

        private Color color_dark = new Color(0.2470588f, 0.2470588f, 0.2470588f,1);
        private Color color_white = new Color(0.8352942f, 0.8352942f, 0.8352942f, 1);

        private AddRequest requestAdd = null;
        private string requestName = null;

        private RemoveRequest requestRemove = null;

        private Dictionary<string, int> packageCount = new Dictionary<string, int>();

        private StringBuilder versionBuilder = new StringBuilder();

        private int lastRepaintCount = 0;

        private bool in_installation = false;

        private bool in_remove = false;

        private float removeTimer = 0;


        private float successTimer = 0;

        private GUIContent labelContent;

        private GUIContent gameStoreContent;

        #endregion

        #region 属性

        internal ExpansionPackages Packages
        {
            get
            { 
                if (_packages == null)
                    _packages = AssetDatabase.LoadAssetAtPath<ExpansionPackages>
                        ("Packages/com.xfkj.xfgameframework/" +
                         "Editor/Configs/ExpansionPackages.asset");

                if (_packages == null)
                    _packages = AssetDatabase.LoadAssetAtPath<ExpansionPackages>
                        ("Assets/Resources/ExpansionPackages.asset");

                return _packages;
            }
        }

        internal ListRequest RequestList => requestList;

        private Color right_bottom_color {
            get {
                if (!EditorGUIUtility.isProSkin) { 
                    return color_white;
                }
                return color_dark;
            }
        }

        #endregion


        [MenuItem("Tools/Yun/拓展包",false,0)]
        static void Open()
        {
            GetWindow<ExpansionWindow>().Show();
        }

        private void Awake()
        {
            titleContent = new GUIContent("Yun拓展包", "Yun拓展包");
            minSize = new Vector2(500, 300);


        }

        private void Update()
        {
            lastRepaintCount++; 
            if ( lastRepaintCount >= 10) 
            { 
                Repaint();
                lastRepaintCount = 0;
            }

            HandleAdd();

            HandleRemove();
        }


        private void OnEnable()
        {
            horizontal_split_percent = 0.4f;
        }

        protected override void OnGUI()
        {
             
            if (requestList == null)
                Refresh();

            switch (requestList.Status)
            {
                case StatusCode.InProgress:
                    Loading();
                    break;
                case StatusCode.Success:
                    base.OnGUI(); 
                    break;
                case StatusCode.Failure:
                    LoadingFailure();
                    break;
            }



        }

        internal void Refresh()
        { 
            requestList = Client.List();
        }

        private void Loading()
        {
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();


            GUIContent content = EditorGUIUtility.IconContent(string.Format("d_WaitSpin{0:00}", index));


            if (Time.realtimeSinceStartup - timer > 0.1f)
            {
                index++;
                index %= 10;
                timer = Time.realtimeSinceStartup;
            }

            GUILayout.Label(content, "LargeLabel");

            GUILayout.Label("Loading packages...", "LargeLabel");


            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
        }

        private void LoadingFailure()
        {
            //


            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            GUILayout.Label(string.Empty, "CN EntryErrorIcon");

            GUILayout.BeginVertical();

            GUILayout.Space(15);

            if (requestList.Error != null)
            {
                string message = string.Format("errorCode:{0} message:{1}", requestList.Error.errorCode, requestList.Error.message);
                GUILayout.Label(message, "WhiteLargeCenterLabel");
            }


            GUILayout.EndVertical();

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(15);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("重试", GUILayout.Width(100)))
            {
                Refresh();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();

        }


        protected override void DrawGUILeft(Rect rect)
        {
            base.DrawGUILeft(rect);
             
            if (packageListState == null)
            {
                packageListState = new TreeViewState();
            }

            if (packageListTree == null)
            {
                packageListTree = new ExpansionPackageListTree(packageListState, this);
                packageListTree.Reload();
            }
            
            packageListTree.OnGUI(rect); 
        }

        protected override void DrawGUIRight(Rect rect)
        {
            base.DrawGUIRight(rect);

            GUILayout.BeginArea(rect);

            scrollViewRight = GUILayout.BeginScrollView(scrollViewRight);

            GUILayout.BeginHorizontal();

            //GUILayout.Space(left.width + 10);

            GUILayout.BeginVertical();

            GUILayout.Space(10);

            if (currentShowInfo == null && Packages.packages != null && Packages.packages.Count > 0)
                currentShowInfo = Packages.packages[0];

            if (currentShowInfo == null)
                return;

            if (headerStyle == null)
            {
                headerStyle = new GUIStyle("AM HeaderStyle");
                headerStyle.fontSize = 16;
            }

            if (nameStyle == null)
            {
                nameStyle = new GUIStyle("WordWrappedLabel");
                nameStyle.fontSize = 15;
                nameStyle.fontStyle = FontStyle.Italic;
            }
              
            EditorGUILayout.SelectableLabel(currentShowInfo.displayName, "AM MixerHeader2");
            GUILayout.Space(10);

            versionBuilder.Clear();

            if (IsInstalled(currentShowInfo.name) && GetPackage(currentShowInfo.name) != null)
                versionBuilder.Append("Version: ").Append(GetPackage(currentShowInfo.name)?.version);
            else
                versionBuilder.Append("Version: ").Append(currentShowInfo.version);

            versionBuilder.Append("     最低支持 Unity 版本:").Append(currentShowInfo.min_unity_version);

            if (currentShowInfo.preview)
                versionBuilder.Append("   <color=#FFA44E>预览版</color>");

            GUILayout.Label(versionBuilder.ToString(), "ProfilerHeaderLabel");


            GUILayout.Label("名称", headerStyle);
            EditorGUILayout.SelectableLabel(currentShowInfo.name, nameStyle);

            GUILayout.Space(10);

            GUILayout.Label("链接", headerStyle);

            DrawLink("视频介绍", currentShowInfo.video_url);
            DrawLink("快速入门", currentShowInfo.quick_start);
            DrawLink("API文档", currentShowInfo.api);
            DrawLink("CHANGE LOG", currentShowInfo.change_log);
            DrawLink("插件源码", currentShowInfo.gitUrl);
            DrawLink("实战项目", "https://space.bilibili.com/258939476/pugv");
            DrawLink("赞助框架", "https://gitee.com/xianfengkeji/XFGameFramework/blob/master/Documentation~/XFGameFramework_%E6%8D%90%E8%B5%A0.md");

            GUILayout.Label("作者", headerStyle);
            GUILayout.Label(currentShowInfo.author, "ProfilerHeaderLabel");

            GUILayout.Space(10);

            if (desStyle == null)
            {
                desStyle = new GUIStyle("WordWrappedLabel");
                desStyle.fontSize = 14;
            }

            GUILayout.Label(currentShowInfo.description, desStyle);

            GUILayout.Space(10);

            GUILayout.Label("依赖", headerStyle);
            GUILayout.Space(10);
            DrawDependencies();

            GUILayout.Space(20);
            GUILayout.Label("基于该框架的游戏", headerStyle);
            GUILayout.Space(10);
            DrawGames();

            GUILayout.Space(50);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();

            GUILayout.EndArea();

            DrawBottom(rect);
        }

         
        internal bool IsInstalled(string name)
        {
             
            if (RequestList == null)
                return false;

            if (RequestList.Status != StatusCode.Success)
                return false;

            foreach (var item in RequestList.Result)
            { 
                if (item.name == name)
                    return true;
            }

            string dir = string.Format("Packages/{0}", name);  
            return AssetDatabase.IsValidFolder(dir);
        }

        internal UnityEditor.PackageManager.PackageInfo GetPackage(string name) {
            if (RequestList == null)
                return null;

            if (RequestList.Status != StatusCode.Success)
                return null;

            foreach (var item in RequestList.Result)
            {
                if (item.name == name)
                    return item;
            } 
            return null;
        }

        internal void ShowInfo(ExpansionPackageInfo info)
        {
            currentShowInfo = info;  
        }
        private void DrawLink(string title, string url)
        {
            GUILayout.BeginHorizontal();
            //GUILayout.Label(title, GUILayout.Width(60));

            if (linkStyle == null) {
                linkStyle = new GUIStyle(GUI.skin.label);
                linkStyle.richText = true;
                linkStyle.normal.textColor = new Color(0.03f, 0.4f, 0.9f, 1);
                linkStyle.onHover.textColor = Color.white;
                linkStyle.alignment = TextAnchor.MiddleLeft;
                linkStyle.fontStyle = FontStyle.Italic;
            }

            //EditorGUIUtility.AddCursorRect(horizontal_split_rect, MouseCursor.ResizeHorizontal);

            if(labelContent == null)
                labelContent = new GUIContent(title);

            labelContent.text = title;

            Rect rect =  GUILayoutUtility.GetRect(labelContent, linkStyle);

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

            if (GUI.Button(rect,labelContent, linkStyle))
            {
                if (string.IsNullOrEmpty(url)) 
                {
                    if (currentShowInfo.preview)
                    {
                        this.ShowNotification(new GUIContent("预览版本,暂无文档!"));
                    }
                    else {
                        this.ShowNotification(new GUIContent("暂无!"));
                    } 
                    return;
                }

                Application.OpenURL(url);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void DrawDependencies() {

            if (currentShowInfo.dependencies == null || currentShowInfo.dependencies.Count == 0)
                GUILayout.Label("无依赖", "AnimationTimelineTick");
            else
            {
                foreach (var name in currentShowInfo.dependencies)
                {
                    if (string.IsNullOrEmpty(name)) continue;
                    if (!searchRequests.ContainsKey(name))
                    {
                        SearchRequest request = Client.Search(name);
                        searchRequests.Add(name, request);
                    }

                    if (searchRequests.ContainsKey(name) && searchRequests[name].Status == StatusCode.InProgress)
                    {
                        GUILayout.Label("loading...", "AnimationTimelineTick");
                    }

                    if (searchRequests.ContainsKey(name) && searchRequests[name].IsCompleted)
                    {
                        SearchRequest r = searchRequests[name];
                        UnityEditor.PackageManager.PackageInfo packageInfo = null;
                        if (r.Result != null && r.Result.Length > 0)
                        {
                            packageInfo = r.Result[0];
                        }

                        if (packageInfo == null)
                            packageInfo = GetPackage(name); 


                        if (packageInfo != null)
                        {
                            GUILayout.BeginHorizontal();

                            ExpansionPackageInfo info = Packages.Get(name);

                            string displayName = info != null ? info.displayName : packageInfo.displayName;

                            // 查到了 
                            GUILayout.Label(displayName, "AnimationTimelineTick" );
                            GUILayout.FlexibleSpace();
                            if (IsInstalled(name))
                            {
                                GUILayout.Label(packageInfo.version, "AnimationTimelineTick");
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("已安装", "AnimationTimelineTick");
                            }
                            else
                            {
                                GUILayout.Label("未安装", "AnimationTimelineTick");
                            }
                            GUILayout.Space(20);
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            // 没查到判断本地 有没有

                            ExpansionPackageInfo info = Packages.Get(name);

                            if (info != null)
                            {
                                GUILayout.BeginHorizontal();
                                // 查到了 
                                GUILayout.Label(info.displayName, "AnimationTimelineTick", GUILayout.Width(200));

                                if (IsInstalled(name))
                                {
                                    GUILayout.Label(info.version, "AnimationTimelineTick", GUILayout.Width(200));
                                    GUILayout.Label("已安装", "AnimationTimelineTick");
                                }
                                else
                                {
                                    GUILayout.Label("未安装", "AnimationTimelineTick");
                                }

                                GUILayout.EndHorizontal();
                            }
                            //else
                            //{
                            //    GUILayout.BeginHorizontal();
                            //    // 查到了 
                            //    GUILayout.Label(name);
                            //    GUILayout.Label("未知依赖", "AnimationTimelineTick");
                            //    GUILayout.EndHorizontal();
                            //}

                        }
                    }

                }
            }

        }

        private void DrawGames() 
        {
            // foreach (var item in Packages.gameInfos)
            // {
            //     GUILayout.BeginHorizontal();
            //
            //     //GUILayout.Space(20);
            //
            //     GUILayout.Label(item.name, "AnimationTimelineTick");
            //
            //
            //     for (int i = 0; i < item.gameStoreInfos.Count; i++)
            //     {
            //         GUILayout.FlexibleSpace();
            //
            //         if (storeStyle == null) {
            //             storeStyle = new GUIStyle(GUI.skin.label);
            //             storeStyle.fontSize -= 1;
            //             storeStyle.richText = true;
            //             storeStyle.normal.textColor = new Color(0.03f, 0.4f, 0.9f, 1);
            //             storeStyle.onHover.textColor = Color.white;
            //         }
            //
            //         if(gameStoreContent == null)
            //             gameStoreContent = new GUIContent(item.gameStoreInfos[i].name);
            //
            //         gameStoreContent.text = item.gameStoreInfos[i].name;
            //
            //         Rect r = GUILayoutUtility.GetRect(gameStoreContent, storeStyle);
            //         EditorGUIUtility.AddCursorRect(r, MouseCursor.Link);
            //         if (GUI.Button(r,item.gameStoreInfos[i].name, storeStyle)) 
            //         {
            //             Application.OpenURL(item.gameStoreInfos[i].url);
            //         }   
            //     } 
            //
            //     GUILayout.Space(20);
            //
            //     GUILayout.EndHorizontal();
            //}
        }

        private void DrawBottom(Rect right) 
        {

            right_bottom.Set(right.x, right.y + right.height - 30, right.width, 30);

            EditorGUI.DrawRect(right_bottom, right_bottom_color);

            if (IsInstalled(currentShowInfo.name))
            {
                // 更新和移除
                right_bottom.Set(right_bottom.x + right_bottom.width - 75, right_bottom.y + 5, 70, right_bottom.height - 10);

                EditorGUI.BeginDisabledGroup(currentShowInfo.cannot_remove);
                if (UnityEngine.GUI.Button(right_bottom, "移除")) 
                    OnBtnRemoveClick(); 
                EditorGUI.EndDisabledGroup();

                right_bottom.Set(right_bottom.x - 75, right_bottom.y, 70, right_bottom.height);

                if (UnityEngine.GUI.Button(right_bottom, "更新")) {
                    OnBtnInstallClick();
                }

            }
            else {
                // 安装
                right_bottom.Set(right_bottom.x +  right_bottom.width - 75, right_bottom.y + 5, 70, right_bottom.height - 10);
                if (UnityEngine.GUI.Button(right_bottom, "安装"))
                {
                    OnBtnInstallClick();
                }

            }
             
            right_bottom.Set(right.x, right.y + right.height - 30, right.width, 30);
            right_bottom.y -= right_bottom.height;

            UnityEngine.GUI.Box(right_bottom, string.Empty, "CN Box");
        }

        private void HandleAdd() 
        {
             
            if (Packages.need_add_gits.Count > 0 && !in_installation)
            { 
                requestName = Packages.need_add_gits[0];
                 
                Packages.need_add_gits.RemoveAt(0);
                Packages.Save();
                requestAdd = Client.Add(requestName);
                 
                in_installation = true;

                successTimer = 0;
                //Debug.LogFormat("开始安装:{0}", requestName);
            }

            if (!in_installation)  
                return; 

            EditorUtility.DisplayProgressBar("安装拓展包", string.Format("正在安装:{0}", requestName), 1);

            if (requestAdd != null && requestAdd.IsCompleted)
            { 
                if (requestAdd.Status == StatusCode.Success)
                {
                    // 重新加载 Packages 配置
                    _packages = null; 

                    if (successTimer == 0)
                    {
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }

                    successTimer += 0.1f;
                    // 3s之后再处理 因为安装成功之后无法立即获取到 package.json
                    if (successTimer >= 3)
                    {
                        string package_path = string.Format("Packages/{0}/package.json", requestAdd.Result.name); 
                        TextAsset package = AssetDatabase.LoadAssetAtPath<TextAsset>(package_path);  
                        if (package != null)
                        {
                            try
                            {
                                ExpansionPackageDependencies dependencies = JsonConvert.DeserializeObject<ExpansionPackageDependencies>(package.text);
                                foreach (var data in dependencies.xfgameframework_dependencies)
                                {
                                    UnityEditor.PackageManager.PackageInfo info = GetPackage(data.name);

                                    if (info != null) 
                                    {
                                        if (!CompareVersion(data.version, info.version)) {
                                            //Debug.LogFormat("无需安装依赖:{0} version:{1} local_version:{2}", data.git_url,data.version,info.version);
                                            continue; 
                                        }
                                    }

                                    //Debug.LogFormat("需要安装依赖:{0}", data.git_url);

                                    Packages.need_add_gits.Add(data.git_url);
                                }

                                Packages.Save();
                            }
                            catch (Exception)
                            { 
                            }
                             
                        }

                        if (Packages.need_add_gits.Count == 0)
                            Refresh();
                    }
                    else
                        return;

                }
                else if (requestAdd.Status == StatusCode.Failure)
                {
                    string message = string.Format("{0}安装失败,errorCode:{1},message:{2},请稍后再试!", requestName, requestAdd.Error?.errorCode, requestAdd.Error?.message);
                    this.ShowNotification(new GUIContent(message));
                    Debug.LogError(message);  
                }

                requestName = null;
                requestAdd = null;
                in_installation = false;

                // 清空进度
                EditorUtility.ClearProgressBar();
            }
        }

        private void HandleRemove() 
        {
            if (Packages.need_removes.Count > 0 && !in_remove)
            {
                if (Time.realtimeSinceStartup - removeTimer < 1)
                    return;

                string removeName = Packages.need_removes[Packages.need_removes.Count - 1];
                Packages.need_removes.RemoveAt(Packages.need_removes.Count - 1);
                Packages.Save();
                requestRemove = Client.Remove(removeName);
                in_remove = true;
                removeTimer = Time.realtimeSinceStartup;
            }

            if (requestRemove != null && requestRemove.IsCompleted) 
            {  
                if (Packages.need_removes.Count == 0)
                    Refresh(); 
                requestRemove = null;
                in_remove = false;


                removeTimer = Time.realtimeSinceStartup;
            }

        }


        /// <summary>
        /// 判断是否被依赖
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsBeDependentOn(string name,out string displayName) {

            foreach (var item in Packages.packages)
            {
                if (IsInstalled(item.name)) { 
                    foreach (var dependency in item.dependencies)
                    {
                        if (dependency == name) {
                            displayName = item.displayName;
                            return true;
                        }
                    }
                }
            }
            displayName = string.Empty;
            return false;
        }


        private void OnBtnInstallClick() 
        {
            if (IsGreaterEditorVersion(currentShowInfo.min_unity_version))
            {
                string tip = string.Format("当前编辑器版本:{0} 低于该拓展包支持的最低版本:{1},建议您使用更高版本的Unity3D编辑器!",Application.unityVersion,currentShowInfo.min_unity_version);
                bool b = EditorUtility.DisplayDialog("提示", tip, "仍要安装", "取消");
                if (!b) return;
            }
             
            Packages.need_add_gits.Clear();
            packageCount.Clear();
             
            // 只安装当前的包 当前的包安装成功之后 通过 package.json 文件获取到依赖包 然后安装依赖包
            Packages.need_add_gits.Add(currentShowInfo.gitUrl);
            Packages.Save(); 
        }

        private void OnBtnRemoveClick() {
            string d = null;
            if (IsBeDependentOn(currentShowInfo.name, out d))
            {
                string message = string.Format("拓展包:{0}不能移除,{1}依赖于它!", currentShowInfo.name, d);
                EditorUtility.DisplayDialog("提示", message, "确定");
                Debug.LogError(message);
                return;
            }

            // 判断
            Packages.need_removes.Clear(); 
            Packages.need_removes.Add(currentShowInfo.name);
            Packages.Save();
        }

        public List<string> GetPackageName(string name) 
        {

            if (packageCount.ContainsKey(name))
                packageCount[name]++;
            else
                packageCount.Add(name, 1);

            if (packageCount[name] > 100) 
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("循环依赖,下列包:");
                foreach (var packageName in packageCount.Keys)
                {
                    if (packageCount[packageName] > 90)
                    {
                        builder.AppendLine(packageName);    
                    }
                }
                builder.AppendLine("之前形成了循环依赖,请调整后重试!");

                this.ShowNotification(new GUIContent(builder.ToString()));
                throw new System.Exception(builder.ToString());
            }

            if(string.IsNullOrEmpty(name)) 
                return null;

            ExpansionPackageInfo info = Packages.Get(name);

            List<string> packages = new List<string>();

            if (info == null) 
            {
                packages.Add(name);
                return packages;
            }


            foreach (var item in info.dependencies)
            {
                List<string> d = GetPackageName(item);
                if (d != null)
                {
                    foreach (var dependency in d)
                    {
                        if (packages.Contains(dependency))
                            continue;


                        if (IsInstalled(dependency)) 
                         { 
                            // 这里的逻辑需要修改一下 ( 如果已经安装了 ，判断一下当前安装的版本 和 依赖的最低版本进行比较, 如果当前安装的版本更低, 就需要更新 ) TODO



                            continue;
                        }

                        packages.Add(dependency);
                    }
                }
            }
            if(!packages.Contains(name))
                packages.Add(name);

            return packages;
            

        }

        /// <summary>
        /// 判断version 是否大于 当前的编辑器版本
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsGreaterEditorVersion(string version)
        { 
            Version v = InternalEditorUtility.GetUnityVersion(); 
            return CompareVersion(version,string.Format("{0}.{1}.{2}",v.Major,v.Minor,v.Build));
        }

        /// <summary>
        /// 如果 version1 大于 version2 返回true
        /// </summary>
        /// <param name="version1"></param>
        /// <param name="version2"></param>
        /// <returns></returns>
        public static bool CompareVersion(string version1,string version2)
        {
            if (version1.Equals(version2)) return false;

            if (!version1.Contains(".")) return false;
            if (!version2.Contains(".")) return false;

            string[] strs = version1.Split('.');

            string[] strs_version2 = version2.Split('.');



            for (int i = 0; i < strs.Length; i++) 
            {
                // 说明长度不一致 直接break 返回false
                if (i > strs_version2.Length - 1)
                    break;

                int v = 0;
                int.TryParse(strs[i], out v);

                int v1 = 0;
                int.TryParse(strs_version2[i], out v1);

                if (v > v1)
                    return true; // 大于return true
                else if (v < v1)
                    return false;// 小于return false   等于则继续往后面判断
            }


            return false;
        }

    }
}

