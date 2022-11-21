namespace MarkdownToHtml
{
    public class GoogleBloggerHtmlBuilder : HtmlBuilder
    {
        private int _indentBeforeCode;

        public override void StartHeading(int level)
        {
            Writer.Write($"<h{level} style='text-align: left;'>");
        }

        public override void StartCode(string? info)
        {
            if(string.IsNullOrWhiteSpace(info))
                info = "text";

            Writer.WriteLine("<pre>");
            Writer.Indent++;
            Writer.WriteLine($"<code lang='{info}'>");
            _indentBeforeCode = Writer.Indent;
            Writer.Indent = 0;
        }

        public override void EndCode(string? info)
        {
            Writer.Indent = _indentBeforeCode;
            Writer.WriteLine("</code>");
            Writer.Indent--;
            Writer.WriteLine("</pre>");
        }

        public override void StartParagraph()
        {
            Writer.Write("<p style='text-align: justify;'>");
        }
    }
}
