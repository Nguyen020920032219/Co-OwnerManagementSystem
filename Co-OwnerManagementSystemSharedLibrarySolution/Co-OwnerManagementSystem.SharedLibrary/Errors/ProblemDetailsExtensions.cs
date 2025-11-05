namespace Co_OwnerManagementSystem.SharedLibrary.Errors;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddStandardProblemDetails(this IServiceCollection s)
    {
        s.AddProblemDetails(o =>
        {
            o.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
            };
        });
        return s;
    }
}