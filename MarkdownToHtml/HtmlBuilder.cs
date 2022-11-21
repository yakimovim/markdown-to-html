using System.CodeDom.Compiler;
using System.Text;
using System.Text.Encodings.Web;

namespace MarkdownToHtml
{
    public abstract class HtmlBuilder
    {
        private StringBuilder _builder = new();

        protected IndentedTextWriter Writer;

        public HtmlBuilder()
        {
            Writer = new(new StringWriter(_builder));
        }

        public string GetHtml() => _builder.ToString();

        public virtual void StartHeading(int level)
        {
            Writer.Write($"<h{level}>");
        }

        public virtual void EndHeading(int level)
        {
            Writer.WriteLine($"</h{level}>");
        }

        public virtual void StartParagraph()
        {
            Writer.Write("<p>");
        }

        public virtual void EndParagraph()
        {
            Writer.WriteLine("</p>");
        }

        public abstract void StartCode(string? info);

        public abstract void EndCode(string? info);

        public virtual void WriteCodeLines(string[] lines)
        {
            foreach(var line in lines)
            {
                Writer.WriteLine(EncodeCodeLine(line));
            }
        }

        protected virtual string EncodeCodeLine(string line)
        {
            return HtmlEncoder.Default.Encode(line);
        }

        public virtual void WriteText(object text)
        {
            Writer.Write(text);
        }

        public virtual void WriteInlineCode(object content)
        {
            Writer.Write($"<i>{content}</i>");
        }

        public virtual void StartLink(string? url)
        {
            Writer.Write($"<a href='{url}' rel='nofollow' target='_blank'>");
        }

        public virtual void EndLink()
        {
            Writer.Write("</a>");
        }

        public virtual void StartSpan(char delimiterChar, int delimiterCount)
        {
            switch((delimiterChar, delimiterCount))
            {
                case ('*', 1):
                case ('_', 1):
                    {
                        StartItalic();
                        break;
                    }
                case ('*', 2):
                case ('_', 2):
                    {
                        StartBold();
                        break;
                    }
                case ('~', 2):
                    {
                        StartStrikethrough();
                        break;
                    }
                default:
                    {
                        Console.WriteLine($"Unable to start span for {delimiterCount} '{delimiterChar}'");
                        break;
                    }
            }
        }

        protected virtual void StartItalic()
        {
            Writer.Write("<i>");
        }

        protected virtual void StartBold()
        {
            Writer.Write("<b>");
        }

        protected virtual void StartStrikethrough()
        {
            Writer.Write("<s>");
        }
        public virtual void EndSpan(char delimiterChar, int delimiterCount)
        {
            switch ((delimiterChar, delimiterCount))
            {
                case ('*', 1):
                case ('_', 1):
                    {
                        EndItalic();
                        break;
                    }
                case ('*', 2):
                case ('_', 2):
                    {
                        EndBold();
                        break;
                    }
                case ('~', 2):
                    {
                        EndStrikethrough();
                        break;
                    }
                default:
                    {
                        Console.WriteLine($"Unable to close span for {delimiterCount} '{delimiterChar}'");
                        break;
                    }
            }
        }

        protected virtual void EndItalic()
        {
            Writer.Write("</i>");
        }

        protected virtual void EndBold()
        {
            Writer.Write("</b>");
        }

        protected virtual void EndStrikethrough()
        {
            Writer.Write("</s>");
        }

        public virtual void StartList(bool isOrdered)
        {
            Writer.WriteLine(isOrdered ? "<ol>" : "<ul>");
            Writer.Indent++;
        }

        public virtual void EndList(bool isOrdered)
        {
            Writer.Indent--;
            Writer.WriteLine(isOrdered ? "</ol>" : "</ul>");
        }

        public virtual void StartListItem()
        {
            Writer.Write("<li>");
        }

        public virtual void EndListItem()
        {
            Writer.WriteLine("</li>");
        }

        public virtual void WriteLineBreak()
        {
            Writer.WriteLine("<hr/>");
        }
    }
}
