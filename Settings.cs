﻿namespace Plugin_Gitmoji {

  /// <summary>
  /// An emoji within the gitmoji convention
  /// </summary>
  public class Gitmoji {
    /// <summary>
    /// The code used to include the emoji within a commit message
    /// </summary>
    public string code { get; set; }
    /// <summary>
    /// The use for the emoji
    /// </summary>
    public string description { get; set; }
  }

  /// <summary>
  ///   The settings for this (Gitmoji) plugin
  /// </summary>
  public class Settings {
    /// <summary>
    /// The command to see all emojis within the gitmoji convention (defaults to "gitmoji")
    /// </summary>
    public string AllEmojiSpecialCommand { get; set; } = "gitmoji";
    /// <summary>
    /// The command signifier used to obtain Emojis (defaults to "gitmoji ")<br />
    /// </summary>
    public string GitmojiSignifier { get; set; } = "gitmoji ";
    /// <summary>
    ///   The threshold for when to consider an emoji
    ///   name / description is similar enough to the query for it to be
    ///   displayed (defaults to 4). Currently uses the
    ///   Levenshtein distance; the larger the number, the
    ///   bigger the difference.
    /// </summary>
    public int FuzzySearchThreshold { get; set; } = 4;
    /// <summary>
    /// The list of emojis within the gitmoji convention
    /// </summary>
    public List<Gitmoji> gitmojis { get; set; } = new List<Gitmoji>(new Gitmoji[] { });
  }
}
