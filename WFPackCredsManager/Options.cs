using CommandLine;
using CommandLine.Text;

namespace WFPackCredsManager
{
    public class Options
    {
        [Option('l', "login", Required = true, HelpText = "SharePoint user login")]
        public string Login { get; set; }

        [Option('p', "password", Required = true, HelpText = "SharePoint user password")]
        public string Password{ get; set; }

        [Option('u', "url", Required = true, HelpText = "SharePoint root site url")]
        public string Url { get; set; }

        [Option('f', "force", DefaultValue = false, HelpText = "If set it will set default workflow credentials on all child's sites to the same as on the root")]
        public bool Force { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
