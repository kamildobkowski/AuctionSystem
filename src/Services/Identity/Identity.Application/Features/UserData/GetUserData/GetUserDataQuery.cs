using Identity.Application.Features.RegisterUser.Common.Models;
using Identity.Domain.Repositories;
using Shared.Base.Cqrs.Queries;
using Shared.Base.Errors;

namespace Identity.Application.Features.UserData.GetUserData;

public sealed record GetUserDataQuery(Guid Id) : IQuery;

internal sealed class GetUserDataQueryHandler(IUserRepository repository) 
	: IQueryHandler<GetUserDataQuery, GetUserDataQueryResponse>
{
	public async Task<IQueryResult<GetUserDataQueryResponse>> HandleAsync(
		GetUserDataQuery query, CancellationToken cancellationToken = default)
	{
		var user = await repository.Get(x => x.Id == query.Id);
		if (user is null)
			return QueryResult.Failure<GetUserDataQueryResponse>(ErrorResult.NotFoundError);
		return QueryResult.Success(new GetUserDataQueryResponse(user.Email, user.IsCompany,
			user.IsCompany ? null 
				: new PersonalUserData(
					user.PersonalDetails!.FirstName, 
					user.PersonalDetails.LastName, 
					user.PersonalDetails.PhoneNumber?.ToString()),
			user.IsCompany ? 
				new CompanyUserData(
					user.CompanyDetails!.Name, 
					new AddressModel(user.CompanyDetails.Address), 
					user.CompanyDetails.Nip, 
					user.CompanyDetails.PhoneNumber.ToString()) : null));
	}
}