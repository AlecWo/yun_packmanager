using Newtonsoft.Json; 
using UnityEditor; 
using UnityEngine;


namespace UnityPackage.Editor
{

    public class PackageInfo 
    { 
        public string version;
    }

    public class Help : EditorWindow
    {

        Rect textureRect = new Rect(0,10,291 * 0.7F ,96 * 0.7F);
        Texture logo;
         
        private GUIStyle style;

        private string version;

        private void Awake()
        {
            logo = AssetDatabase.LoadAssetAtPath<Texture>("Packages/com.xfkj.xfabmanager/Editor/Texture/logo_web.png");
             
            TextAsset p = AssetDatabase.LoadAssetAtPath<TextAsset>("Packages/com.xfkj.xfgameframework/package.json");
            
            if(p == null)
                p = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/package.json");

           PackageInfo info = JsonConvert.DeserializeObject<PackageInfo>(p.text);

            version = string.Format("Version {0}", info.version);
        }

        private void ConfigStyle() {
            style = new GUIStyle( GUI.skin.label);
            style.richText = true;
            style.normal.textColor =new Color(0.03f, 0.4f, 0.9f, 1);
            style.onHover.textColor = Color.white;
            style.alignment = TextAnchor.MiddleLeft;
            style.fontStyle = FontStyle.Italic;
            //style.onFocused.textColor = Color.red;
        }

        private void Update() {
            //开启窗口的重绘，不然窗口信息不会刷新
            Repaint();
        }

     

        private void OnGUI()
        {

            GUI.DrawTexture(textureRect, logo); 
            GUILayout.Space(textureRect.height + 20);
            GUILayout.BeginHorizontal();
            GUILayout.Space(130);
            GUILayout.Label(version);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

 
            GUILayout.Label("XFGameFramework是一款基于MVC的Unity3D引擎的游戏框架,");
            GUILayout.Label("该框架提供了View管理，UI管理，音频播放，事件系统，");
            GUILayout.Label("Http网络请求，计时器，对象池以及UI适配等功能!"); 
            GUILayout.Label("更多信息可通过点击下方教程链接获取!");
            GUILayout.Space(20);
            if ( style == null ) {
                ConfigStyle();
            }
     
            DrawLink("快速入门:", "https://gitee.com/xianfengkeji/XFGameFramework/blob/master/Documentation~/XFGameFramework%E5%BF%AB%E9%80%9F%E5%85%A5%E9%97%A8.md");
            DrawLink("视频说明:", "https://www.bilibili.com/video/BV19Q4y1g7DW/");
            DrawLink("插件源码:", "https://gitee.com/xianfengkeji/XFGameFramework.git");
            DrawLink("实战案例:", "https://space.bilibili.com/258939476/pugv");
            DrawLink("更多教程:", "https://space.bilibili.com/258939476");
            GUILayout.Space(20);
            GUILayout.Label("XFGameFramework交流群:946441033");

            //GUILayout.Space(20); 
            GUILayout.Label("*弦风课堂制作"); 
        }

        private void DrawLink(string title,string url) {
            GUILayout.BeginHorizontal();
            GUILayout.Label(title,GUILayout.Width(60));

            if (GUILayout.Button(url, style )) {
                Application.OpenURL(url);
            }

            GUILayout.EndHorizontal();
        }


        [MenuItem("Window/XFKT/XFGameFramework/About", false,1000)]
        static void OpenHelp()
        {
            Rect rect = new Rect(0, 0, 550, 370);
            Help window = GetWindowWithRect<Help>(rect, true, "About XFGameFramework");
            window.Show(); 
        }

    }

}

