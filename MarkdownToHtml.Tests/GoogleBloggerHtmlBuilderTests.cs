namespace MarkdownToHtml.Tests
{
    public class GoogleBloggerHtmlBuilderTests
    {
        private readonly MarkdownConverter _converter;

        public GoogleBloggerHtmlBuilderTests()
        {
            _converter = new MarkdownConverter(new GoogleBloggerHtmlBuilder());
        }

        [Fact]
        public void Paragraph()
        {
            var html = _converter.BuildHtml("This is a paragraph text");

            html.Should().Be($"<p style='text-align: justify;'>This is a paragraph text</p>{Environment.NewLine}");
        }
    }
}