using System.ComponentModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Shared.Base.Token;

public sealed class FromTokenModelBinder(string claimType) : IModelBinder
{
	private readonly string _claimType = claimType ?? throw new ArgumentNullException(nameof(claimType));

	public Task BindModelAsync(ModelBindingContext ctx)
	{
		if (ctx == null) throw new ArgumentNullException(nameof(ctx));

		var identity = ctx.HttpContext.User?.Identity as ClaimsIdentity;
		if (identity is not { IsAuthenticated: true })
		{
			ctx.Result = ModelBindingResult.Failed();
			return Task.CompletedTask;
		}

		var claim = identity.FindFirst(_claimType);
		if (claim == null)
		{
			ctx.ModelState.AddModelError(ctx.ModelName, $"Claim '{_claimType}' not found.");
			ctx.Result = ModelBindingResult.Failed();
			return Task.CompletedTask;
		}

		object value = claim.Value;
		try
		{
			if (ctx.ModelType != typeof(string))
			{
				var conv = TypeDescriptor.GetConverter(ctx.ModelType);
				value = conv.ConvertFromInvariantString(claim.Value)!;
			}
			ctx.Result = ModelBindingResult.Success(value);
		}
		catch
		{
			ctx.ModelState.AddModelError(ctx.ModelName, $"Cannot convert claim '{_claimType}' to {ctx.ModelType.Name}.");
			ctx.Result = ModelBindingResult.Failed();
		}

		return Task.CompletedTask;
	}
}