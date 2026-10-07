namespace GUI_2.Data
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        { 
            //Username aus Request Header holen
            var user = context.Request.Headers["X-User"].FirstOrDefault();

            //Wenn kein User, dann system nutzen
            if (string.IsNullOrEmpty(user))
                user = "system";

            //In HttpContext speichern
            context.Items["CurrentUser"] = user;

            await _next(context);
        }
    }
}
