using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace SampleDialogue.Runtime
{
  /// <summary>
  /// Manages the loading of dialogue trees from XML files.
  /// </summary>
  public abstract class DialogueLoader
  {
    /// <summary>
    /// Loads a dialogue tree from the specified XML file.
    /// </summary>
    /// <param name="file">the TextAsset loaded from unity</param>
    /// <returns>The loaded <see cref="DialogueTree"/> object.</returns>
    public static DialogueTree LoadDialogue(TextAsset file)
    {
      XmlSerializer serializer = new(typeof(DialogueTree));
      using StringReader stringReader = new(file.text);
      return (DialogueTree)serializer.Deserialize(stringReader);
    }
  }
}