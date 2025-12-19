using Lines.Infrastructure.Context;

namespace Lines.Presentation.Middlewares;

public class TransactionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TransactionMiddleware> _logger;

    public TransactionMiddleware(RequestDelegate next, ILogger<TransactionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ApplicationDBContext dbContext)
    {
        // ✅ Check if the request should be transactional:
        //   - Any data-modifying HTTP method (POST, PUT, PATCH, DELETE)
        //   - OR the URL path contains "register-google" (case-insensitive)
        if (!IsTransactionalRequest(context))
        {
            await _next(context);
            return;
        }

        // ✅ Begin transaction
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        _logger.LogInformation("----- Begin DB transaction for request {Method} {Path}", context.Request.Method, context.Request.Path);

        try
        {
            // Continue through the middleware pipeline
            await _next(context);

            // Save pending changes to DB
            await dbContext.SaveChangesAsync();
            _logger.LogInformation("----- SaveChanges completed for request {Method} {Path}", context.Request.Method, context.Request.Path);

            // Commit the transaction
            await transaction.CommitAsync();
            _logger.LogInformation("----- Commit DB transaction for request {Method} {Path}", context.Request.Method, context.Request.Path);
        }
        catch (Exception ex)
        {
            // Rollback on any unhandled exception
            _logger.LogError(ex, "----- Rollback DB transaction for request {Method} {Path} due to an unhandled exception.", context.Request.Method, context.Request.Path);
            await transaction.RollbackAsync();
            throw; // Re-throw for global error handler
        }
    }

    /// <summary>
    /// Determines whether the current HTTP request should be wrapped in a DB transaction.
    /// </summary>
    private static bool IsTransactionalRequest(HttpContext context)
    {
        var method = context.Request.Method;
        var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;

        // Transactional if:
        // 1️⃣ The HTTP method modifies data (POST, PUT, PATCH, DELETE)
        // 2️⃣ The URL path includes "register-google"
        return HttpMethods.IsPost(method)
            || HttpMethods.IsPut(method)
            || HttpMethods.IsPatch(method)
            || HttpMethods.IsDelete(method)
            || (path.Contains("register") && path.Contains("google"));
    }
}
