using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace MarkdownToHtml
{
    public class MarkdownConverter
    {
        private readonly HtmlBuilder _htmlBuilder;
        private readonly MarkdownPipeline _pipeline;

        public MarkdownConverter(HtmlBuilder htmlBuilder)
        {
            _htmlBuilder = htmlBuilder ?? throw new ArgumentNullException(nameof(htmlBuilder));

            _pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();
        }

        public string BuildHtml(string markdown)
        {
            var markdownDocument = Markdown.Parse(markdown, _pipeline);

            foreach(var block in markdownDocument)
            {
                ProcessBlock(block);
            }

            return _htmlBuilder.GetHtml();
        }

        private void ProcessBlock(Block block, bool isListItemBlock = false)
        {
            switch (block)
            {
                case BlankLineBlock: breakingLineBlock:
                    break;
                case ThematicBreakBlock thematicBreakBlock:
                    _htmlBuilder.WriteLineBreak();
                    break;
                case ListBlock listBlock:
                    _htmlBuilder.StartList(listBlock.IsOrdered);
                    foreach (var item in listBlock)
                    {
                        ProcessBlock(item);
                    }
                    _htmlBuilder.EndList(listBlock.IsOrdered);
                    break;
                case ListItemBlock listItemBlock:
                    _htmlBuilder.StartListItem();
                    foreach (var itemBlock in listItemBlock)
                    {
                        ProcessBlock(itemBlock, true);
                    }
                    _htmlBuilder.EndListItem();
                    break;
                case HeadingBlock headingBlock:
                    _htmlBuilder.StartHeading(headingBlock.Level);
                    foreach (var inline in headingBlock.Inline)
                    {
                        ProcessInline(inline);
                    }
                    _htmlBuilder.EndHeading(headingBlock.Level);
                    break;
                case ParagraphBlock paragraphBlock:
                    if(!isListItemBlock)
                        _htmlBuilder.StartParagraph();
                    foreach (var inline in paragraphBlock.Inline)
                    {
                        ProcessInline(inline);
                    }
                    if(!isListItemBlock)
                        _htmlBuilder.EndParagraph();
                    break;
                case FencedCodeBlock fencedCodeBlock:
                    _htmlBuilder.StartCode(fencedCodeBlock.Info);
                    _htmlBuilder.WriteCodeLines(
                        fencedCodeBlock.Lines.Lines
                        .Take(fencedCodeBlock.Lines.Count)
                        .Select(l => l.ToString())
                        .ToArray());
                    _htmlBuilder.EndCode(fencedCodeBlock.Info);
                    break;
                case LinkReferenceDefinitionGroup:
                    break; // We do not support it
                default:
                    Console.WriteLine($"Unable to process block of type '{block.GetType().Name}'");
                    break;
            }
        }

        private void ProcessInline(Inline inline)
        {
            switch(inline)
            {
                case LinkInline linkInline:
                    {
                        _htmlBuilder.StartLink(linkInline.Url);
                        foreach (var internalInline in linkInline)
                        {
                            ProcessInline(internalInline);
                        }
                        _htmlBuilder.EndLink();
                        break;
                    }
                case EmphasisInline emphasisInline:
                    {
                        _htmlBuilder.StartSpan(emphasisInline.DelimiterChar, emphasisInline.DelimiterCount);
                        foreach (var internalInline in emphasisInline)
                        {
                            ProcessInline(internalInline);
                        }
                        _htmlBuilder.EndSpan(emphasisInline.DelimiterChar, emphasisInline.DelimiterCount);
                        break;
                    }
                case LiteralInline literalInline:
                    {
                        _htmlBuilder.WriteText(literalInline.Content);
                        break;
                    }
                case CodeInline codeInline:
                    {
                        _htmlBuilder.WriteInlineCode(codeInline.Content);
                        break;
                    }
                case ContainerInline containerInline:
                    {
                        foreach(var internalInline in containerInline)
                        {
                            ProcessInline(internalInline);
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine($"Unable to process inline of type '{inline.GetType().Name}'");
                        break;
                    }
            }
        }
    }
}
