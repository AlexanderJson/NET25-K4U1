using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Globalization;
using System.Text.Json;


/*
public class Program
{
    public static void Main()
    {
        var md = new Metadata();

        var apiPath = md.GetPath("src", "Api");

        if (apiPath == null)
        {
            return;
        }


         var files = md.GetFiles(apiPath);


         var classes = md.GetClasses(files);


         var outputPath = Path.Combine(apiPath, "classes.json");

         var writer = new Writer();
        writer.WriteToJson(classes, outputPath);

        var generator = new TestGenerator();

foreach (var clazz in classes)
{
    if (!clazz.ClassFunctions.Any())
        continue;

    var testCode = generator.GenerateTestClass(clazz);

    var testPath = Path.Combine(apiPath, $"{clazz.ClassName}Tests.cs");


}
    }
}


public class ConditionMetadata
{
    public int Order { get; set; }
    public string Type { get; set; } = ""; 
    public string Value { get; set; } = ""; 
    public string Condition { get; set; } = "";
}

public class FunctionMetadata
{
    public string FuncName { get; set; } = "";
    public int NumConditions { get; set; } = 0;
    public int NumStatements { get; set; } = 0;
    public int NumBranches { get; set; } = 0;
    public string ReturnType { get; set; } = "";
    public List<ParameterMetadata> Parameters { get; set; } = new();
    public List<ConditionMetadata> Outcomes { get; set; } = new(); 




}
public class ParameterMetadata
{
    public string Name { get; set; } = "";
    public string Type { get; set; } = "";
}

public class ClassMetadata
{
    public string ClassName {get; set;} = "";
    public bool HasTestfile {get; set;} = false;
    
    public List<FunctionMetadata> ClassFunctions {get; set;} = [];
    
    public int NumOfFunctions {get; set;} = 0;
    
}
public sealed class Metadata()
{

    public string? GetPath(params string[] parts)
    {
        string root = GetRoot();
        if (root == null)
        {
            return null;
        }
        return Path.Combine([root, .. parts]);
    }
    static string? GetRoot()
    {
        var current = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (current != null)
        {
            Console.WriteLine($"Checking: {current.FullName}");

            if (Directory.Exists(Path.Combine(current.FullName, "src")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }
            return null;
    }

    public List<string> GetFiles(string rootPath)
    {
        var files = Directory.GetFiles(rootPath, "*.cs", SearchOption.AllDirectories);
        var filtered = files
            .Where(f =>
                f.Contains("service", StringComparison.OrdinalIgnoreCase) ||
                f.Contains("repository", StringComparison.OrdinalIgnoreCase))
            .ToList();
        return filtered;
    }

    public List<ClassMetadata> GetClasses(List<string> files)
    {
        var clazz = new List<ClassMetadata>();
        foreach (var file in files)
                    var tree = CSharpSyntaxTree.ParseText(content);
            var root = tree.GetRoot();

            var clazzes = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
            foreach (var c in clazzes)
            {
                var methods = c.DescendantNodes().OfType<MethodDeclarationSyntax>();
                var classMeta = new ClassMetadata
                {
                    ClassName = c.Identifier.Text,
                    ClassFunctions = new List<FunctionMetadata>()
                };

                foreach(var m in methods)
                {
                    var numConditions = m.DescendantNodes().OfType<IfStatementSyntax>().Count();
                    var numStatements = m.DescendantNodes().OfType<StatementSyntax>().Count();
                    var numBranches = numConditions;
                    var returnType = m.ReturnType.ToString();
                    var parameters = m.ParameterList.Parameters
                        .Select(p => new ParameterMetadata
                        {
                            Name = p.Identifier.Text,
                            Type = p.Type?.ToString() ?? "unknown"
                        })
                        .ToList();

                    var outcomes = new List<ConditionMetadata>();
                    var returns = m.DescendantNodes().OfType<ReturnStatementSyntax>();
                    foreach(var r in returns)
                    {
                        var value = r.Expression?.ToString() ?? "";
                        var ifParent = r.Ancestors().OfType<IfStatementSyntax>().FirstOrDefault();
                        string condition;

                        if(ifParent != null)
                        {
                            condition = ifParent.Condition.ToString();

                        }
                        else
                        {
                            condition = "default";
                        }
                            outcomes.Add(new ConditionMetadata
                        {
                            Type = "Return",
                            Value = value,
                            Condition = condition
                        });
                    }



                    classMeta.ClassFunctions.Add(new FunctionMetadata
                    {
                        FuncName = m.Identifier.Text,
                        ReturnType = returnType,
                        Parameters = parameters,
                        NumConditions = numConditions,
                        NumStatements = numStatements,
                        NumBranches = numBranches,
                        Outcomes = outcomes

                    });
                    classMeta.NumOfFunctions = classMeta.ClassFunctions.Count;
                }
                    clazz.Add(classMeta);

            }
        }
        return clazz;
    }
}


public sealed class Writer
{
    public void WriteToJson(List<ClassMetadata> clazzes, string path)
    {
        var json = JsonSerializer.Serialize(clazzes, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(path, json);
    }
}


public sealed class TestGenerator
{
    private string Sanitize(string input)
{
    return input
        .Replace("\"", "")
        .Replace(" ", "")
        .Replace("(", "")
        .Replace(")", "")
        .Replace(".", "")
        .Replace("==", "Equals")
        .Replace("&&", "And")
        .Replace("||", "Or");
}

    private string GenerateArguments(List<ParameterMetadata> parameters, ConditionMetadata outcome)
{
    if (parameters.Count == 0)
        return "";

     return string.Join(", ", parameters.Select(p => GetDefaultValue(p.Type)));
}

private string GetDefaultValue(string type)
{
    return type switch
    {
        "int" => "0",
        "string" => "\"test\"",
        "bool" => "false",
        _ => "null"
    };
}
        private string GenerateTestMethod(string className, FunctionMetadata func, ConditionMetadata outcome)
        {
            var methodName = func.FuncName;
            var expected = outcome.Value;

            var testName = $"{methodName}_Returns_{Sanitize(expected)}";

            var sb = new System.Text.StringBuilder();

            sb.AppendLine("    [Fact]");
            sb.AppendLine($"    public void {testName}()");
            sb.AppendLine("    {");

             sb.AppendLine($"        var service = new {className}();");

             var args = GenerateArguments(func.Parameters, outcome);

             sb.AppendLine($"        var result = service.{methodName}({args});");

             sb.AppendLine($"        Assert.Equal({expected}, result);");

            sb.AppendLine("    }");

            return sb.ToString();
        }
    public string GenerateTestClass(ClassMetadata clazz)
    {
        var sb = new System.Text.StringBuilder();

        var className = clazz.ClassName;
        var testClassName = $"{className}Tests";

        sb.AppendLine("using Xunit;");
        sb.AppendLine();
        sb.AppendLine($"public class {testClassName}");
        sb.AppendLine("{");

        foreach (var func in clazz.ClassFunctions)
        {
            foreach (var outcome in func.Outcomes)
            {
                var test = GenerateTestMethod(className, func, outcome);
                sb.AppendLine(test);
            }
        }

        sb.AppendLine("}");

        return sb.ToString();
    }
}

*/