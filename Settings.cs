namespace PluginGitmoji
{

  /// <summary>
  /// An emoji within the gitmoji convention
  /// </summary>
  public class Gitmoji
  {
    /// <summary>
    /// The code used to include the emoji within a commit message
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public string Code { get; set; }

    /// <summary>
    /// The use for the emoji
    /// </summary>
    public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
  }

  /// <summary>
  ///   The settings for this (Gitmoji) plugin
  /// </summary>
  public class Settings
  {
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
    ///   displayed (defaults to 70). The larger the number, the
    ///   more similar it needs to be.
    /// </summary>
    public int FuzzySearchThreshold { get; set; } = 70;
    /// <summary>
    /// The list of emojis within the gitmoji convention
    /// </summary>
    public List<Gitmoji> Gitmojis { get; set; } = new List<Gitmoji>(Array.Empty<Gitmoji>());
  }
}
