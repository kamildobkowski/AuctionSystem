using AntDesign;
using AuctionSystem.Contracts.Common;

namespace AuctionSystem.Web.Extensions;

public static class FormValidationExtension
{
	private const string ValidationError = "ValidationError";
	public static void AddValidationErrors<T>(this Form<T> form, ErrorResult errorResult, Action? onValidationAdded = null)
	{
		if (errorResult.ErrorCode != ValidationError)
			return;

		var errorsPerField = errorResult.Errors?.GroupBy(x => x.ErrorField!) ?? [];

		foreach (var i in errorsPerField)
		{
			form.SetValidationMessages(i.Key, i.Select(x => x.ErrorMessage).ToArray());	
		}
		
		onValidationAdded?.Invoke();
	}
}