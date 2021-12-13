namespace API.Helpers;

public class LogUserActivity : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var resultContext = await next();

		//Return if not authed
		if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

		var userId = resultContext.HttpContext.User.GetUserId();
		var uow = resultContext.HttpContext.RequestServices.GetService<IUnitOfWork>();
		var user = await uow.UserRepository.GetUserByIdAsync(userId);

		user.LastActive = DateTime.UtcNow;

		await uow.Complete();
	}
}