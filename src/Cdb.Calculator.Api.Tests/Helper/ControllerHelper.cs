using Microsoft.AspNetCore.Mvc;

namespace Cdb.Calculator.Api.Tests.Helper;

public static class ControllerHelper
{
    const string routeAttributeName = "RouteAttribute";

    public static string GetRoute<T>(string methodName)
        where T : ControllerBase
    {
        var routeController = (typeof(T).CustomAttributes
            .FirstOrDefault(c => c.AttributeType.Name == routeAttributeName)?
            .ConstructorArguments[0].Value?.ToString() ?? string.Empty) + "/";

        var method = typeof(T).GetMethod(methodName);
        var route = method.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name == routeAttributeName);
        route ??= method.CustomAttributes.FirstOrDefault(c => c.AttributeType.Name.Contains("Http"));

        return route?.ConstructorArguments.Any() ?? false
            ? routeController + route?.ConstructorArguments[0].Value?.ToString() ?? string.Empty
            : routeController;
    }
}