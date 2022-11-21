using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace MarkdownToHtml
{
    public class CodeProjectHtmlBuilder : HtmlBuilder
    {
        private readonly static HtmlEncoder _htmlEncoder;

        private int _indentBeforeCode;

        static CodeProjectHtmlBuilder()
        {
            //var settings = new TextEncoderSettings();
            //settings.AllowRange(UnicodeRanges.All);
            //settings.ForbidCharacter('<');
            //settings.ForbidCharacter('>');

            _htmlEncoder = HtmlEncoder.Default;
        }

        public override void WriteInlineCode(object content)
        {
            Writer.Write($"<code>{content}</code>");
        }

        public override void EndCode(string? info)
        {
            Writer.Indent = _indentBeforeCode;
            Writer.WriteLine("</pre>");
        }

        public override void StartCode(string? info)
        {
            if (string.IsNullOrWhiteSpace(info))
                info = "text";

            Writer.WriteLine($"<pre lang='{info}'>");
            _indentBeforeCode = Writer.Indent;
            Writer.Indent = 0;
        }

        protected override string EncodeCodeLine(string line)
        {
            return _htmlEncoder.Encode(line);
        }
    }
}
