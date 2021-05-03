using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.SpriteLines
{
    public class SpritesLine : ScriptableObject
    {
        public Sprite[] sprites;
        
        [MenuItem("Assets/Create/Skak/Sprites line")]
        public static void CreateSpritesLine()
        {
            SpritesLine asset = CreateInstance<SpritesLine>();
            var sprites = new List<Sprite>();

            foreach (Object selected in Selection.objects)
            {
                if (selected is Sprite s)
                {
                    sprites.Add(s);
                }
            }

            asset.sprites = sprites.ToArray();

            ProjectWindowUtil.CreateAsset(asset, "Sprites Line.asset");
        }
    }
}