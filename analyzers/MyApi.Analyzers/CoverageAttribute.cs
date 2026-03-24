using System;
namespace MyWebApi;


[AttributeUsage(AttributeTargets.Method)]
public sealed class CoverageAttribute : Attribute
{
}