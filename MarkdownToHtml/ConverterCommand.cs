using System.ComponentModel.DataAnnotations;
using McMaster.Extensions.CommandLineUtils;

namespace MarkdownToHtml
{
    internal enum TargetTypes
    {
        Google,
        CodeProject
    }

    internal class ConverterCommand
    {
        private readonly IConsole _console;

        [Argument(0)]
        [Required]
        public string InputMarkdownFile { get; }

        [Option("-o|--output")]
        [Required]
        public string OutputHtmlFile { get; set; }

        [Option]
        [Required]
        public TargetTypes Target { get; set; }

        public ConverterCommand(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        private void OnExecute()
        {
            if(!File.Exists(InputMarkdownFile))
            {
                _console.WriteLine($"Input file '{InputMarkdownFile}' does not exist.");
                return;
            }

            if(File.Exists(OutputHtmlFile))
            {
                if(!Prompt.GetYesNo($"Output file '{OutputHtmlFile}' already exists. Do you want to override it?", false))
                {
                    return;
                }
            }

            HtmlBuilder? htmlBuilder = Target switch
            {
                TargetTypes.Google => new GoogleBloggerHtmlBuilder(),
                TargetTypes.CodeProject => new CodeProjectHtmlBuilder(),
                _ => null
            };

            if(htmlBuilder == null)
            {
                _console.WriteLine($"Unknown target type '{Target}'.");
                return;
            }

            var converter = new MarkdownConverter(htmlBuilder);

            var markdown = File.ReadAllText(InputMarkdownFile);

            var html = converter.BuildHtml(markdown);

            File.WriteAllText(OutputHtmlFile, html);
        }
    }
}
