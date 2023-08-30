using UnityEditor;
using UnityEngine;

namespace SilverTau.RoomPlanUnity
{
    public class RoomPlanUnityPackageEditor : Editor
    {
        protected static Texture2D Icon;
        protected static Texture2D IconMini;
        protected static Texture2D Background;
        protected GUIStyle LogoStyle;
        protected GUIStyle BackgroundStyle;
        
        public virtual void Awake()
        {
            Icon = EditorGUIUtility.Load("Packages/com.silvertau.roomplanunitykit/Editor/Images/icon.png") as Texture2D;
            
            if (Icon == null)
            {
                Icon = EditorGUIUtility.Load("Assets/Silver Tau/RoomPlanUnityKit/Editor/Images/icon.png") as Texture2D;
            }
            
            IconMini = EditorGUIUtility.Load("Packages/com.silvertau.roomplanunitykit/Editor/Images/icon_mini.png") as Texture2D;
            
            if (IconMini == null)
            {
                IconMini = EditorGUIUtility.Load("Assets/Silver Tau/RoomPlanUnityKit/Editor/Images/icon_mini.png") as Texture2D;
            }
            
            Background = CreateTexture2D(2, 2, new Color(0.0f, 0.0f, 0.0f, 0.5f));
            
            BackgroundStyle = new GUIStyle
            {
                fixedHeight = 32.0f,
                stretchWidth = true,
                normal = new GUIStyleState
                {
                    background = Background
                }
            };
            
            LogoStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleLeft,
                stretchWidth = true,
                stretchHeight = true,
                fixedHeight = 32.0f,
                fontSize = 21,
                richText = true
            };
        }

        private Texture2D CreateTexture2D(int width, int height, Color col)
        {
            var pix = new Color[width * height];
            for(var i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        protected void BoxLogo(MonoBehaviour behaviour = null, string value = "")
        {
            if (Icon != null)
            {
                if(behaviour) EditorGUIUtility.SetIconForObject(behaviour, Icon);
            }
            
            EditorGUI.BeginChangeCheck();
            GUILayout.BeginHorizontal(BackgroundStyle);
            GUILayout.Label(IconMini, GUILayout.ExpandWidth(false),  GUILayout.ExpandHeight(true));
            GUILayout.Label("<b><color=#1b7fe3>RPU Kit</color></b>" + value, LogoStyle);
		    
            GUILayout.EndHorizontal();
            
            GUILayout.Space(2.5f);
        }
    }
}
