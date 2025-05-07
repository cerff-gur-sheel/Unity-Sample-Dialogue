using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace SampleDialogue.Assets.Runtime
{
  /// <summary>
  /// Represents a dialogue option that can be selected by the player.
  /// </summary>
  public class DialogueOption
  {
    /// <summary>
    /// Gets or sets the text displayed for the dialogue option.
    /// </summary>
    [XmlAttribute("Text")]
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the ID of the next dialogue node to navigate to when this option is selected.
    /// </summary>
    [XmlAttribute("NextNodeID")]
    public int NextNodeID { get; set; }
  }

  /// <summary>
  /// Manages the loading of dialogue trees from XML files.
  /// </summary>
  public abstract class DialogueLoader
  {
    /// <summary>
    /// Loads a dialogue tree from the specified XML file.
    /// </summary>
    /// <param name="filePath">The file path to the XML file containing the dialogue tree.</param>
    /// <returns>The loaded <see cref="DialogueTree"/> object.</returns>
    public static DialogueTree LoadDialogue(string filePath)
    {
      XmlSerializer serializer = new(typeof(DialogueTree));
      using FileStream fileStream = new(filePath, FileMode.Open);
      return (DialogueTree)serializer.Deserialize(fileStream);
    }

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