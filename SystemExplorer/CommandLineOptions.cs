using CommandLine.Text;
using CommandLine;

namespace SystemExplorer;

public class CommandLineOptions
{
    [Option('i', "incognito", Required = false, HelpText = "Private mode")]
    public bool IsIncognitoModeEnabled { get; set; }
}
