namespace Shared.Base.Token;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class FromTokenAttribute(string claimType) : Attribute, IBindingSourceMetadata
{
	public string ClaimType { get; } = claimType;

	public BindingSource BindingSource => BindingSource.Custom;
}