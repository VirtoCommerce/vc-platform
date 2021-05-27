using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Markdig;
using Markdig.Extensions.CustomContainers;
using Markdig.Helpers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Nuke.Common;

partial class Build : NukeBuild
{
    [Parameter("Shows help for the target", Name = "?")] public static bool HelpParameter = false;
    protected override void OnTargetStart(string target)
    {
        if (HelpParameter)
        {
            Logger.Info(GetHelpForTarget(target));
            Environment.Exit(0);
        }
        else base.OnTargetStart(target);
    }

    private string GetHelpForTarget(string target)
    {
        var pipeline = new MarkdownPipelineBuilder().UseCustomContainers().Build();
        var vcbuildRoot = AppDomain.CurrentDomain.BaseDirectory;
        var docsPath = Path.Combine(vcbuildRoot, "docs", "targets.md");
        var md = Markdown.Parse(File.ReadAllText(docsPath), pipeline);
        var containers = md.Descendants<CustomContainer>();
        var container = containers.FirstOrDefault(c =>
        {
            var heading = c.Descendants<HeadingBlock>().FirstOrDefault();
            if (heading == null)
                return false;

            return target == GetTextContent(heading);
        });
        if (container == null)
        {
            Logger.Error($"Help is not found for the target {target}");
            return string.Empty;
        }
        var descriptionBlock = container.Descendants<ParagraphBlock>().FirstOrDefault();
        var exampleBlock = container.Descendants<FencedCodeBlock>().FirstOrDefault();
        var description = GetTextContent(descriptionBlock);
        var examples = GetFencedText(exampleBlock);
        string result = $"{description}{Environment.NewLine}{examples}";
        return result;
    }

    private string GetTextContent(LeafBlock leaf)
    {
        var inline = leaf?.Inline?.FirstChild;
        if (inline is null)
            return string.Empty;

        var result = new StringBuilder();
        do
        {
            switch (inline)
            {
                case LiteralInline li:
                    var inlineContent = li.Content;
                    result.Append(inlineContent.Text.Substring(inlineContent.Start, inlineContent.Length));
                    break;
                case LineBreakInline _:
                    result.Append(Environment.NewLine);
                    break;
            }

            inline = inline.NextSibling;
        } while (inline != null);
        return result.ToString();
    }

    private string GetFencedText(FencedCodeBlock fencedCodeBlock)
    {
        if (fencedCodeBlock == null)
            return string.Empty;

        var lines = fencedCodeBlock.Lines.Lines.Select(l =>
        {
            var slice = l.Slice;
            if (EqualityComparer<StringSlice>.Default.Equals(slice, default(StringSlice)))
                return string.Empty;

            return slice.Text?.Substring(l.Slice.Start, l.Slice.Length) ?? string.Empty;

        });
        return string.Join(Environment.NewLine, lines);
    }
}
