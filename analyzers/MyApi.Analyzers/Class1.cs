namespace MyApi.Analyzers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using Microsoft.VisualBasic;
/*
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class Class1 : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MY001";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "TODO",
        "TODO",
        "Content",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(Rule);

public override void Initialize(AnalysisContext context)
{
    context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    context.EnableConcurrentExecution();

    context.RegisterSyntaxTreeAction(AnalyzeTree);
}

private static void AnalyzeTree(SyntaxTreeAnalysisContext context)
{
    var root = context.Tree.GetRoot();

    foreach (var trivia in root.DescendantTrivia())
    {
        if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
            trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
        {
            var text = trivia.ToString();

            if (text.Contains("TODO"))
            {
                var diagnostic = Diagnostic.Create(Rule, trivia.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var literal = (LiteralExpressionSyntax)context.Node;
        var text = literal.Token.ValueText;

        if (text.Contains("TODO"))
        {
            var location = literal.GetLocation();
            var lineSpan = location.GetLineSpan();

            var filePath = lineSpan.Path;
            var lineNumber = lineSpan.StartLinePosition.Line + 1;

    
            TodoCollector.Add(text, filePath, lineNumber);

    
            var diagnostic = Diagnostic.Create(Rule, location);
            context.ReportDiagnostic(diagnostic);
        }
    }



}

 
public static class TodoCollector
{
    public static List<TodoItem> Todos { get; } = new();

    public static void Add(string text, string file, int line)
    {
        Todos.Add(new TodoItem
        {
            Text = text,
            File = file,
            Line = line
        });
    }
}

public class TodoItem
{
    public string Text { get; set; } = "";
    public string File { get; set; } = "";
    public int Line { get; set; }
}*/