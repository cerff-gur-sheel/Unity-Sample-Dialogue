using System.Xml.Serialization;
/*
XML File Structure:
  - DIALOGUETREE
    - NODES
      - NODE
        - ID
        - TEXTS
          - TEXT
            - CHARACTER
            - EMOTION
            - CONTENT
        - OPTIONS
          - OPTION
            - ID
            - TEXT
            - NEXTNODE
*/

namespace SampleDialogue.Assets.Runtime
{
  /// <summary>
  /// Represents a dialogue tree containing multiple dialogue nodes.
  /// </summary>
  [XmlRoot("Dialogue Tree")]
  public class DialogueTree
  {
    /// <summary>
    /// Gets or sets the collection of dialogue nodes in the dialogue tree.
    /// </summary>
    [XmlElement("Node")]
    public DialogueNode[] Nodes { get; set; }
  }

  /// <summary>
  /// Represents a single dialogue node containing texts and options.
  /// </summary>
  public class DialogueNode
  {
    /// <summary>
    /// Gets or sets the unique identifier for the dialogue node.
    /// </summary>
    [XmlAttribute("ID")]
    public int ID { get; set; }

    /// <summary>
    /// Gets or sets the collection of texts associated with the dialogue node.
    /// </summary>
    [XmlArray("Texts")]
    [XmlArrayItem("Text")]
    public Text[] Texts { get; set; }

    /// <summary>
    /// Gets or sets the collection of dialogue options available for the node.
    /// </summary>
    [XmlArray("Options")]
    [XmlArrayItem("Option")]
    public DialogueOption[] Options { get; set; }
  }

  /// <summary>
  /// Represents a piece of dialogue text with associated metadata.
  /// </summary>
  public class Text
  {
    /// <summary>
    /// Gets or sets the character speaking the text.
    /// </summary>
    [XmlAttribute("Character")]
    public string Character { get; set; }

    /// <summary>
    /// Gets or sets the emotion of the character while speaking the text.
    /// </summary>
    [XmlAttribute("Emotion")]
    public string Emotion { get; set; }

    /// <summary>
    /// Gets or sets the content of the dialogue text.
    /// </summary>
    [XmlText]
    public string Content { get; set; }
  }
}