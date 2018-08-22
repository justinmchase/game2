using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.Animations;
using System.IO;
using Ionic.Zip;
using System.Linq;
using System.Collections.Generic;

namespace Game.Pyxel
{
  [ScriptedImporter(1, "pyxel")]
  public class PyxelImporter : ScriptedImporter
  {
    public Vector2 Pivot = Vector2.zero;
    public int PixelsPerUnit = 16;
    public RigidbodyType2D BodyType = RigidbodyType2D.Static;

    public override void OnImportAsset(AssetImportContext ctx)
    {
      Debug.Log(string.Format("Importing {0}", ctx.assetPath));

      PyxelFile pyxel = null;
      var layers = new List<Texture2D>();
      using (var file = File.OpenRead(ctx.assetPath))
      using (var archive = ZipFile.Read(file))
      {
        foreach (var entry in archive)
        {
          using (var stream = new MemoryStream())
          {
            entry.Extract(stream);
            stream.Seek(0, SeekOrigin.Begin);
            if (entry.FileName == "docData.json")
            {
              pyxel = PyxelFile.Read(stream);
            }
            else if (entry.FileName.StartsWith("layer"))
            {
              var texture = new Texture2D(4, 4);
              var data = stream.ToArray();
              texture.LoadImage(data);
              layers.Insert(0, texture);
            }
          }
        }
      }

      var w = pyxel.canvas.width;
      var h = pyxel.canvas.height;
      var flattened = FlattenTextures(w, h, layers);

      flattened.name = string.Format("{0} Texture", Path.GetFileNameWithoutExtension(ctx.assetPath));
      ctx.AddObjectToAsset(flattened.name, flattened);

      var tw = pyxel.canvas.tileWidth;
      var th = pyxel.canvas.tileHeight;
      var i = 0;
      var sprites = new List<Sprite>();
      for (var x = 0; x < w; x += tw)
        for (var y = 0; y < h; y += th)
        {
          var sprite = Sprite.Create(flattened, new Rect(x, y, tw, th), this.Pivot, this.PixelsPerUnit);
          sprite.name = string.Format("{0} Tile {1}", Path.GetFileNameWithoutExtension(ctx.assetPath), i++);
          ctx.AddObjectToAsset(sprite.name, sprite);
          sprites.Add(sprite);
        }

      i = 0;
      foreach (var animation in pyxel.animations)
      {
        var anim = new AnimationClip();
        anim.wrapMode = WrapMode.Once;
        anim.name = string.Format("{0} Animation {1}", Path.GetFileNameWithoutExtension(ctx.assetPath), i++);
        // animation.baseTile
        // animation.length

        var t = 100; // todo: get from animation
        var keyFrames = new ObjectReferenceKeyframe[animation.length];
        for (var x = 0; x < animation.length; x++)
        {
          keyFrames[x] = new ObjectReferenceKeyframe
          {
            time = (t * x) / 1000f,
            value = sprites[x + animation.baseTile]
          };
        }

        var binding = new EditorCurveBinding
        {
          path = "",
          type = typeof(SpriteRenderer),
          propertyName = "m_Sprite" // m_Sprite
        };

        // AnimationUtility.GetAnimationClipSettings(anim).loopTime = true;
        AnimationUtility.SetObjectReferenceCurve(anim, binding, keyFrames);
        ctx.AddObjectToAsset(anim.name, anim);
      }

      var prefab = new GameObject();
      prefab.name = string.Format("{0}", Path.GetFileNameWithoutExtension(ctx.assetPath), i++);
      if (sprites.Count > 0)
      {
        var rigidbody = prefab.AddComponent<Rigidbody2D>();
        rigidbody.bodyType = this.BodyType;

        var collider = prefab.AddComponent<BoxCollider2D>();

        var renderer = prefab.AddComponent<SpriteRenderer>();
        renderer.sprite = sprites[0];
      }

      ctx.AddObjectToAsset(prefab.name, prefab);
    }

    private static Texture2D FlattenTextures(int w, int h, List<Texture2D> textures)
    {
      Color[] dest = null;
      foreach (var texture in textures)
      {
        var src = texture.GetPixels();
        if (dest == null)
        {
          dest = src;
        }
        else
        {
          for (var i = 0; i < dest.Length; i++)
          {
            dest[i] = Color.Lerp(dest[i], src[i], src[i].a);
          }
        }
      }

      var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
      tex.filterMode = FilterMode.Point;
      tex.SetPixels(dest);
      tex.Apply();
      return tex;
    }
  }
}
