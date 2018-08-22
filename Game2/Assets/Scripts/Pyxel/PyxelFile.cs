using System;
using System.IO;
using UnityEngine;

namespace Game.Pyxel
{
  [Serializable]
  public class PyxelFile
  {
    public string version;
    public string name;
    public PyxelCanvas canvas;
    public PyxelTileset tileset;
    public PyxelPalette palette;
    public PyxelAnimations animations;
    public PyxelSettings settings;

    public static string Clean(string contents)
    {
      for (var x = 0; x < 128; x++)
      {
        // This is incredibly ghetto but Unity does not support mapping json key names to fields.
        // So if you have a key name that is a reserved keyword or an invalid C# identifier, you're stuck.
        // This maps fields with integer names such as `"0":` to be `"_0":`
        contents = contents.Replace("\"" + x + "\":", "\"_" + x + "\":");
      }

      return contents;
    }

    public static PyxelFile Read(Stream stream)
    {
      using (var reader = new StreamReader(stream))
      {
        stream.Seek(0, SeekOrigin.Begin);
        var contents = Clean(reader.ReadToEnd());
        // Debug.Log(contents);
        return JsonUtility.FromJson<PyxelFile>(contents);
      }
    }
  }
}
