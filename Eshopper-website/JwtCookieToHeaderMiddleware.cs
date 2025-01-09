namespace Eshopper_website
{
	public class JwtCookieToHeaderMiddleware
	{
		//private readonly RequestDelegate _next;

		//public JwtCookieToHeaderMiddleware(RequestDelegate next)
		//{
		//	_next = next;
		//}

		//public async Task Invoke(HttpContext context)
		//{
		//	// Check if the "Authorization" header is present
		//	if (!context.Request.Headers.ContainsKey("Authorization") && context.Request.Cookies.TryGetValue("UserToken", out string jwtToken))
		//	{
		//		// Add "Authorization" along with the jwt stored on the cookies
		//		context.Request.Headers.Add("Authorization", $"Bearer {jwtToken}");
		//	}

		//	// call next middleware in the pipeline
		//	await _next(context);
		//}
	}
}
