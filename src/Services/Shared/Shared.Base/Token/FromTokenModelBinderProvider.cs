using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Shared.Base.Token;

public sealed class FromTokenModelBinderProvider : IModelBinderProvider
{
	public IModelBinder? GetBinder(ModelBinderProviderContext context)
	{
        ArgumentNullException.ThrowIfNull(context);

        if (context.Metadata is not DefaultModelMetadata defaultMetadata)
		{
			return null;
		}

		var attr = defaultMetadata.Attributes.ParameterAttributes?.OfType<FromTokenAttribute>().FirstOrDefault()
		           ?? defaultMetadata.Attributes.PropertyAttributes?.OfType<FromTokenAttribute>().FirstOrDefault();

		return attr != null ? new FromTokenModelBinder(attr.ClaimType) : null;
	}
}