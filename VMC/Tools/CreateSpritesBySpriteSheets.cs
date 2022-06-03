#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace VMC.Tools
{
    public class CreateSpritesBySpriteSheets : ScriptableWizard
    {
        public Texture[] _oldTextures;
        private List<Sprite> _sprites = new List<Sprite>();
        [MenuItem("VMC/Split SpriteSheets")]
        static void CreateWizard()
        {
            var createSprites = ScriptableWizard.DisplayWizard<CreateSpritesBySpriteSheets>("Split SpriteSheets", "Split");
            createSprites._oldTextures = Selection.gameObjects.OfType<Texture>().ToArray();
        }
        void OnWizardCreate()
        {
            _sprites.Clear();
            for (int i = 0; i < _oldTextures.Length; i++)
            {
                _sprites.AddRange(AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(_oldTextures[i]))
                    .OfType<Sprite>().ToArray());
            }
            //Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToArray();

            foreach (Sprite sprite in _sprites)
            {
                string _path = Application.dataPath + "/" + sprite.texture.name;
                if (!File.Exists(_path))
                {
                    System.IO.Directory.CreateDirectory(_path);
                }
                Texture2D tex = sprite.texture;
                Rect r = sprite.textureRect;
                Texture2D subtex = tex.CropTexture((int)r.x, (int)r.y, (int)r.width, (int)r.height);
                byte[] data = subtex.EncodeToPNG();
                string fileName = _path + "/" + sprite.name + ".png";
                File.WriteAllBytes(fileName, data);
            }
        }
    }

    static class Texture2DExtensions
    {
        public static Texture2D CropTexture(this Texture2D pSource, int left, int top, int width, int height)
        {
            if (left < 0)
            {
                width += left;
                left = 0;
            }
            if (top < 0)
            {
                height += top;
                top = 0;
            }
            if (left + width > pSource.width)
            {
                width = pSource.width - left;
            }
            if (top + height > pSource.height)
            {
                height = pSource.height - top;
            }

            if (width <= 0 || height <= 0)
            {
                return null;
            }

            Color[] aSourceColor = pSource.GetPixels(0);

            //*** Make New
            Texture2D oNewTex = new Texture2D(width, height, TextureFormat.RGBA32, false);

            //*** Make destination array
            int xLength = width * height;
            Color[] aColor = new Color[xLength];

            int i = 0;
            for (int y = 0; y < height; y++)
            {
                int sourceIndex = (y + top) * pSource.width + left;
                for (int x = 0; x < width; x++)
                {
                    aColor[i++] = aSourceColor[sourceIndex++];
                }
            }

            //*** Set Pixels
            oNewTex.SetPixels(aColor);
            oNewTex.Apply();

            //*** Return
            return oNewTex;
        }
    }
}
#endif