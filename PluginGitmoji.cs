using Newtonsoft.Json;
using Quokka;
using Quokka.ListItems;
using Quokka.PluginArch;
using System.Collections.ObjectModel;
using System.IO;
using WinCopies.Util;

namespace PluginGitmoji
{
  class GitmojiItem : ListItem
  {

    public GitmojiItem(string code, string description)
    {
      Name = code;
      Description = description;
      string filename = code.Substring(1, code.Length - 2);
      Icon = IconCache.GetOrAdd(
        Environment.CurrentDirectory + "\\PlugBoard\\PluginGitmoji\\Plugin\\" + filename + ".png"
      );
    }

    //When item is selected, copy code
    public override void Execute()
    {
      System.Windows.Clipboard.SetText(Name);
      App.Current.MainWindow.Close();
    }
  }

  /// <summary>
  /// The Gitmoji plugin
  /// </summary>
  public class GitmojiPlugin : Plugin
  {
    private static Settings pluginSettings = new();
    internal static Settings PluginSettings { get => pluginSettings; set => pluginSettings = value; }

    /// <summary>
    /// Loads plugin settings
    /// </summary>
    public GitmojiPlugin()
    {
      string fileName = Environment.CurrentDirectory + "\\PlugBoard\\PluginGitmoji\\Plugin\\settings.json";
      PluginSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(fileName))!;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override string PluginName { get; set; } = "Gitmoji";

    private static Collection<ListItem> ProduceItems(string query)
    {
      return new Collection<ListItem>(

      FuzzySearch.SearchAll(query,
      new Collection<string>(pluginSettings.Gitmojis.Select(x => x.Description).ToList()), PluginSettings.FuzzySearchThreshold).Concat(

        FuzzySearch.SearchAll(query,
        new Collection<string>(pluginSettings.Gitmojis.Select(x => x.Code.Replace(":", "")).ToList()), PluginSettings.FuzzySearchThreshold)

        ).Select(x => (ListItem)new GitmojiItem(pluginSettings.Gitmojis[x.Index].Code, pluginSettings.Gitmojis[x.Index].Description)
      ).Distinct().ToList());
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="query"><inheritdoc/></param>
    /// <returns>
    /// Matching emojis
    /// </returns>
    public override Collection<ListItem> OnQueryChange(string query) { return ProduceItems(query); }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>
    /// The GitmojiSignifier from plugin settings
    /// </returns>
    public override Collection<string> CommandSignifiers()
    {
      return new Collection<string>() { PluginSettings.GitmojiSignifier };
    }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="command">The GitmojiSignifier (Since there is only 1 signifier for this plugin), followed by the emoji being searched for</param>
    /// <returns>Collection of emojis that possibly match what is being searched for</returns>
    public override Collection<ListItem> OnSignifier(string command)
    {
      command ??= "";
      command = command.Substring(PluginSettings.GitmojiSignifier.Length);
      return new Collection<ListItem>(FuzzySearch.Sort(command, ProduceItems(command)).ToList());
    }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>The AllEmojiSpecialCommand from plugin settings</returns>
    public override Collection<string> SpecialCommands()
    {
      Collection<string> SpecialCommand = new() {
        PluginSettings.AllEmojiSpecialCommand
      };
      return SpecialCommand;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="command">The AllEmojiSpecialCommand (Since there is only 1 special command for this plugin)</param>
    /// <returns>All emojis within the gitmoji convention sorted alphabetically</returns>
    public override Collection<ListItem> OnSpecialCommand(string command)
    {
      Collection<ListItem> AllList = new();
      foreach (Gitmoji emoji in pluginSettings.Gitmojis)
      {
        AllList.Add(new GitmojiItem(emoji.Code, emoji.Description));
      }
      AllList = new Collection<ListItem>(AllList.OrderBy(x => x.Name).ToList());
      return AllList;
    }

  }
}

