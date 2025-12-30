using UnityEngine;
using Microsoft.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


[System.AttributeUsage(System.AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public sealed class SingletonAttribute : PropertyAttribute
{
    public SingletonAttribute(bool persistant = false)
    {
        Persistant = persistant;
    }
    public bool Persistant { get; private set; }
}

[System.AttributeUsage(System.AttributeTargets.Method)]
public sealed class SingletonOnlyAttribute : PropertyAttribute
{
}


[Singleton]
public class Test: MonoBehaviour
{
    private void Awake()
    {
        SingletonRegistry.Set(this);
    }
}

public class Test2: MonoBehaviour
{
    private void Start()
    {
        
    }
}



public static class Diagnostics
{
    public static readonly DiagnosticDescriptor SingletonOnlyViolation =
        new DiagnosticDescriptor(
            id: "SG001",
            title: "Singleton-only API usage",
            messageFormat: "Method '{0}' can only be called from a class marked [Singleton]",
            category: "Architecture",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );
}



[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class SingletonOnlyAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(Diagnostics.SingletonOnlyViolation);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(
            AnalyzeInvocation,
            SyntaxKind.InvocationExpression);
    }

    private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocation = (InvocationExpressionSyntax)context.Node;
        var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation);

        if (symbolInfo.Symbol is not IMethodSymbol method)
            return;

        // Méthode appelée marquée [SingletonOnly] ?
        if (!HasAttribute(method, "SingletonOnlyAttribute"))
            return;

        // Trouver la classe appelante
        var callerClass = invocation
            .Ancestors()
            .OfType<ClassDeclarationSyntax>()
            .FirstOrDefault();

        if (callerClass == null)
            return;

        var callerSymbol = context.SemanticModel.GetDeclaredSymbol(callerClass);

        // Classe appelante marquée [Singleton] ?
        if (HasAttribute(callerSymbol, "SingletonAttribute"))
            return;

        // ❌ Violation
        var diagnostic = Diagnostic.Create(
            Diagnostics.SingletonOnlyViolation,
            invocation.GetLocation(),
            method.Name);

        context.ReportDiagnostic(diagnostic);
    }

    private static bool HasAttribute(ISymbol symbol, string attributeName)
    {
        return symbol.GetAttributes()
            .Any(a => a.AttributeClass?.Name == attributeName);
    }
}


