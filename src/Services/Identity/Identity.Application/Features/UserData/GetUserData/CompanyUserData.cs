using Identity.Application.Features.RegisterUser.Common.Models;
using Identity.Domain.ValueObjects;

namespace Identity.Application.Features.UserData.GetUserData;

public sealed record CompanyUserData(
	string Name,
	AddressModel Address,
	string Nip,
	string PhoneNumber);