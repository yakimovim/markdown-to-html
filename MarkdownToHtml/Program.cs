using MarkdownToHtml;

var content = File.OpenText(@"d:\Data\Private GitHub Repo\Статьи\Мой опыт работы с OData\article.eng.md").ReadToEnd();

var visitor = new MarkdownConverter(new CodeProjectHtmlBuilder());

var html = visitor.BuildHtml(content);

Console.WriteLine(html);

using (var stream = File.OpenWrite(@"d:\temp\codeproject.html"))
using (var writer = new StreamWriter(stream))
{
    writer.Write(html);
}

Console.Write("Press ENTER to exit: ");
Console.ReadLine();