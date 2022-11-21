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

        [Theory]
        [InlineData('*')]
        [InlineData('_')]
        public void Italic(char symbol)
        {
            var html = _converter.BuildHtml($"This is a {symbol}paragraph{symbol} text");

            html.Should().Be($"<p style='text-align: justify;'>This is a <i>paragraph</i> text</p>{Environment.NewLine}");
        }

        [Theory]
        [InlineData('*')]
        [InlineData('_')]
        public void Bold(char symbol)
        {
            var html = _converter.BuildHtml($"This is a {symbol}{symbol}paragraph{symbol}{symbol} text");

            html.Should().Be($"<p style='text-align: justify;'>This is a <b>paragraph</b> text</p>{Environment.NewLine}");
        }

        
        [Fact]
        public void Strikethrough()
        {
            var html = _converter.BuildHtml($"This is a ~~paragraph~~ text");

            html.Should().Be($"<p style='text-align: justify;'>This is a <s>paragraph</s> text</p>{Environment.NewLine}");
        }

        [Fact]
        public void InlineCode()
        {
            var html = _converter.BuildHtml("This is a `paragraph` text");

            html.Should().Be($"<p style='text-align: justify;'>This is a <i>paragraph</i> text</p>{Environment.NewLine}");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Headers(int level)
        {
            var html = _converter.BuildHtml(new string('#', level) + " This is a header");

            html.Should().Be($"<h{level} style='text-align: left;'>This is a header</h{level}>{Environment.NewLine}");
        }

        [Fact]
        public void Link()
        {
            var html = _converter.BuildHtml("This is a [link](https://www.example.com) text");

            html.Should().Be($"<p style='text-align: justify;'>This is a <a href='https://www.example.com' rel='nofollow' target='_blank'>link</a> text</p>{Environment.NewLine}");
        }

        [Fact]
        public void UnorderedList()
        {
            var html = _converter.BuildHtml(@"
* Item 1
* Item 2
* Item 3");

            html.Should().Be($@"<ul>
    <li>Item 1</li>
    <li>Item 2</li>
    <li>Item 3</li>
</ul>{Environment.NewLine}");
        }

        [Fact]
        public void OrderedList()
        {
            var html = _converter.BuildHtml(@"
1. Item 1
1. Item 2
1. Item 3");

            html.Should().Be($@"<ol>
    <li>Item 1</li>
    <li>Item 2</li>
    <li>Item 3</li>
</ol>{Environment.NewLine}");
        }

        [Fact]
        public void HorizontalLine()
        {
            var html = _converter.BuildHtml("---");

            html.Should().Be($"<hr/>{Environment.NewLine}");
        }

        [Fact]
        public void CodeBlockCs()
        {
            var html = _converter.BuildHtml(@"```cs
public void AddList(List<string> list)
{
    Console.WriteLine(""List is added"")
}
```");

            html.Should().Be($@"<pre>
    <code lang='cs'>
public void AddList(List&lt;string&gt; list)
{{
    Console.WriteLine(&quot;List is added&quot;)
}}
    </code>
</pre>{Environment.NewLine}");
        }

        [Fact]
        public void CodeBlockText()
        {
            var html = _converter.BuildHtml(@"```
This is an ""error"" message
```");

            html.Should().Be($@"<pre>
    <code lang='text'>
This is an &quot;error&quot; message
    </code>
</pre>{Environment.NewLine}");
        }
    }
}