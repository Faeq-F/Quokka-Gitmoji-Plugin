using Newtonsoft.Json;
using Quokka;
using Quokka.ListItems;
using Quokka.PluginArch;
using System.IO;
using System.Windows.Media.Imaging;
using WinCopies.Util;

namespace Plugin_Gitmoji {
  class GitmojiItem : ListItem {

    public GitmojiItem(string code, string desciption) {
      this.Name = code;
      this.Description = desciption;
      String filename = code.Substring(1, code.Length - 2);
      this.Icon = new BitmapImage(new Uri(
          Environment.CurrentDirectory + "\\PlugBoard\\Plugin_Gitmoji\\Plugin\\" + filename + ".png"));
    }

    //When item is selected, copy code
    public override void Execute() {
      System.Windows.Clipboard.SetText(Name);
      App.Current.MainWindow.Close();
    }
  }

  /// <summary>
  /// The Gitmoji plugin
  /// </summary>
  public class Plugin_Gitmoji : Plugin {
    private static Settings pluginSettings = new();
    internal static Settings PluginSettings { get => pluginSettings; set => pluginSettings = value; }

    /// <summary>
    /// Loads plugin settings
    /// </summary>
    public Plugin_Gitmoji() {
      string fileName = Environment.CurrentDirectory + "\\PlugBoard\\Plugin_Gitmoji\\Plugin\\settings.json";
      PluginSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(fileName))!;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override string PluggerName { get; set; } = "Gitmoji";

    private List<ListItem> ProduceItems(string query) {
      List<ListItem> IdentifiedEmojis = new();
      //filtering emojis
      foreach (Gitmoji emoji in pluginSettings.gitmojis) {
        if (emoji.description.Contains(query, StringComparison.OrdinalIgnoreCase)
          || emoji.code.Contains(query, StringComparison.OrdinalIgnoreCase)
                || FuzzySearch.LD(emoji.description, query) < PluginSettings.FuzzySearchThreshold
                || FuzzySearch.LD(emoji.code, query) < PluginSettings.FuzzySearchThreshold) {
          IdentifiedEmojis.Add(new GitmojiItem(emoji.code, emoji.description));
        }
      }
      return IdentifiedEmojis;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="query"><inheritdoc/></param>
    /// <returns>
    /// Matching emojis
    /// </returns>
    public override List<ListItem> OnQueryChange(string query) { return ProduceItems(query); }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>
    /// The GitmojiSignifier from plugin settings
    /// </returns>
    public override List<string> CommandSignifiers() {
      return new List<string>() { PluginSettings.GitmojiSignifier };
    }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="command">The GitmojiSignifier (Since there is only 1 signifier for this plugin), followed by the emoji being searched for</param>
    /// <returns>List of emojis that possibly match what is being searched for</returns>
    public override List<ListItem> OnSignifier(string command) {
      command = command.Substring(PluginSettings.GitmojiSignifier.Length);
      return ProduceItems(command);
    }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>The AllEmojiSpecialCommand from plugin settings</returns>
    public override List<string> SpecialCommands() {
      List<string> SpecialCommand = new() {
        PluginSettings.AllEmojiSpecialCommand
      };
      return SpecialCommand;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="command">The AllEmojiSpecialCommand (Since there is only 1 special command for this plugin)</param>
    /// <returns>All emojis within the gitmoji convention sorted alphabetically</returns>
    public override List<ListItem> OnSpecialCommand(string command) {
      List<ListItem> AllList = new();
      foreach (Gitmoji emoji in pluginSettings.gitmojis) {
        AllList.Add(new GitmojiItem(emoji.code, emoji.description));
      }
      AllList = AllList.OrderBy(x => x.Name).ToList();
      return AllList;
    }

  }
}

