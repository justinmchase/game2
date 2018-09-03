using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.Animations;
using System.IO;
using Ionic.Zip;
using System.Linq;
using System.Collections.Generic;
using Game;

namespace Game.Pyxel
{
  [ScriptedImporter(1, "pyxel")]
  public class PyxelImporter : ScriptedImporter
  {
    public Vector2 Pivot = Vector2.zero;
    public int PixelsPerUnit = 16;
    public Material SpriteMaterial;
    public Vector2 ColliderOffset = Vector2.zero;
    public Vector2 ColliderSize = Vector2.one;

    public bool AnimatedFramesOnly = true;

    public RigidbodyType2D BodyType = RigidbodyType2D.Static;

    public AnimatorController MapToAnimatorController = null;

    public override void OnImportAsset(AssetImportContext ctx)
    {
      Debug.Log(string.Format("Importing {0}", ctx.assetPath));

      string assetName = Path.GetFileNameWithoutExtension(ctx.assetPath);

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

      flattened.name = string.Format("{0}_texture", assetName);
      ctx.AddObjectToAsset(flattened.name, flattened);

      var tw = pyxel.canvas.tileWidth;
      var th = pyxel.canvas.tileHeight;
      var i = 0;
      var sprites = new Dictionary<int, Sprite>();
      var animationClips = new List<AnimationClip>();

      for (var y = 0; y < h; y += th)
      for (var x = 0; x < w; x += tw)
        {

          var sprite = Sprite.Create(flattened, new Rect(x, h-y-th, tw, th), this.Pivot, this.PixelsPerUnit);
          sprite.name = string.Format("{0}_sprite_{1}", assetName, i);
          if(!this.AnimatedFramesOnly || PyxelImporter.IsAnimatedFrame(i, pyxel.animations)){
            ctx.AddObjectToAsset(sprite.name, sprite);
          }
          sprites.Add(i, sprite);
          i++;
        }
      

      i = 0;
      foreach (var animation in pyxel.animations)
      {
        var anim = new AnimationClip();
      
        anim.name = animation.name;

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

        
        AnimationUtility.SetObjectReferenceCurve(anim, binding, keyFrames);
        if(anim.name.Contains(new[]{"idle", "run"})){
          AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(anim);
          settings.loopTime = true;
          AnimationUtility.SetAnimationClipSettings(anim, settings);
        }
 
        animationClips.Add(anim);
        ctx.AddObjectToAsset(anim.name, anim);
      }

      AnimatorOverrideController animatorController = null;
      if(this.MapToAnimatorController != null){
        animatorController = new AnimatorOverrideController();
        animatorController.name = assetName;
        animatorController.runtimeAnimatorController = this.MapToAnimatorController;
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        var newOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        animatorController.GetOverrides(overrides);
        foreach(var o in overrides){
          var slotName = o.Key.name.ToLower();
          newOverrides.Add(new KeyValuePair<AnimationClip, AnimationClip>(o.Key, GetBestMatch(slotName, animationClips)));
        }
        animatorController.ApplyOverrides(newOverrides);
        
        ctx.AddObjectToAsset(animatorController.name, animatorController);
      }

      var prefab = new GameObject();
      prefab.name = Path.GetFileNameWithoutExtension(ctx.assetPath);
      if (sprites.Count > 0)
      {
        var rigidbody = prefab.AddComponent<Rigidbody2D>();
        rigidbody.bodyType = this.BodyType;

        var collider = prefab.AddComponent<BoxCollider2D>();
        collider.offset = this.ColliderOffset;
        collider.size = this.ColliderSize;

        var renderer = prefab.AddComponent<SpriteRenderer>();
        renderer.sprite = sprites[0];
        renderer.material = this.SpriteMaterial;
        if(this.SpriteMaterial == null){
          renderer.material = AssetDatabase.LoadAssetAtPath("Assets/Materials/Sprite.mat", typeof(Material)) as Material;
        }

        var animator = prefab.AddComponent<Animator>();
       animator.runtimeAnimatorController = animatorController;

        var creatureBehavior = prefab.AddComponent<CreatureBehavior>();
        var syncYZ = prefab.AddComponent<YZSync>();
        syncYZ.offset = -1; //render on top of tiles
      }

      ctx.AddObjectToAsset(prefab.name, prefab);



      

    }

    private static bool IsAnimatedFrame(int i, IEnumerable<PyxelAnimation> animations){
      foreach(var animation in animations){
        var start = animation.baseTile;
        var end = animation.baseTile + animation.length;
        if(i >= start && i < end){
          return true;
        }
      }
      return false;
    }

    private static AnimationClip GetBestMatch(string slotName, IEnumerable<AnimationClip> clips){
      slotName = slotName.ToLower();
      
      //exact match is best match
      foreach(var clip in clips){
        if(slotName == clip.name.ToLower()){
          return clip;
        }
      }

      AnimationClip bestClip = null;
      float bestScore = -1;

      slotName = slotName.ToLower();
      var slotParts = slotName.Split('_');

      foreach(var clip in clips){
        var clipParts = clip.name.ToLower().Split('_');
        var score = 0f;
        for(int i = 0; i < slotParts.Length; i++){
          //most significant name parts are to the left
          score = score * 10;
          if(i >= clipParts.Length) break;
          if(slotParts[i] == clipParts[i]){
            score += 1;
          }

          score += GetAnimationNameSubstitutions(slotParts[i], clipParts[i]);
          score += GetAnimationNameSubstitutions(clipParts[i], slotParts[i]);
        }
        
        if(score > bestScore){
          bestScore = score;
          bestClip = clip;
        }
        
      }

      return bestClip;
    }

    private static float GetAnimationNameSubstitutions(string a, string b){
        if(a == "run"){
          if(b == "walk") return 0.5f;
        }

        return 0f;
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
