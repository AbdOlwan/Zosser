namespace API_OnlineStore.Middleware
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Get language from Accept-Language header
            var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();

            // Set normalized language code for consistent use throughout the application
            string normalizedLanguage = "ar"; // Default to English

            if (!string.IsNullOrEmpty(acceptLanguage))
            {
                var languages = acceptLanguage.Split(',');
                if (languages.Length > 0)
                {
                    // Get the first language code (highest priority)
                    var primaryLanguage = languages[0].Split(';')[0].Trim().ToLower();

                    // Check for English
                    if (primaryLanguage.StartsWith("en"))
                    {
                        normalizedLanguage = "en";
                    }
                }
            }

            // Add normalized language to HttpContext.Items for easy access in controllers and services
            context.Items["Language"] = normalizedLanguage;

            // Call the next middleware
            await _next(context);
        }
    }

    // Extension method for easy registration in Startup
    public static class LanguageMiddlewareExtensions
    {
        public static IApplicationBuilder UseLanguageMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LanguageMiddleware>();
        }
    }
}
