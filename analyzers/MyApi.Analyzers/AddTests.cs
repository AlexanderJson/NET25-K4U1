namespace MyApi.Analyzers;

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;


[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class IfStatementAnalyzer : DiagnosticAnalyzer
{

public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
    => ImmutableArray.Create(Rule);
private static readonly DiagnosticDescriptor Rule = new(
    "TEST001",
    "Debug If",
    "Condition: {0}",
    "Debug",
    DiagnosticSeverity.Warning,
    true);
public override void Initialize(AnalysisContext context)
{
    context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    context.EnableConcurrentExecution();
    context.RegisterSyntaxNodeAction(AnalyzeIfStatement, SyntaxKind.IfStatement);
}

private static void AnalyzeIfStatement(SyntaxNodeAnalysisContext context)
{
    var ifBranch = (IfStatementSyntax)context.Node;
    var ifCondition = ifBranch.Condition;

    var text = ifCondition.ToString();

    var diagnostic = Diagnostic.Create(
        Rule,
        ifBranch.GetLocation(),
        text);

    context.ReportDiagnostic(diagnostic);
}
}