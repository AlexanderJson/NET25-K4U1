namespace MyApi.Analyzers;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class Class1 : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MY001";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        "Banned phrase detected",
        "Ta bort detta",
        "Content",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.StringLiteralExpression);
    }

    private static void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        var literal = (LiteralExpressionSyntax)context.Node;

        var text = literal.Token.ValueText;

        if (text.Contains("/*"))
        {
            var diagnostic = Diagnostic.Create(Rule, literal.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }
}