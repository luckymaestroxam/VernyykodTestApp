using Application.Interfaces;
using Application.RequestHandlers.AddUserCurrency;
using Application.RequestHandlers.GetUserCurrencies;
using Application.RequestHandlers.RemoveUserCurrency;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/user-currencies")]
public class UserCurrenciesController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromServices]
        IRequestHandler<GetUserCurrenciesRequest, GetUserCurrenciesResponse> getUserCurrenciesRequestHandler,
        CancellationToken cancellationToken)
    {
        var request = new GetUserCurrenciesRequest(UserId);

        var result = await getUserCurrenciesRequestHandler.Handle(request, cancellationToken);

        return Ok(result);
    }

    [HttpPost("currency/{currencyId}")]
    public async Task<IActionResult> Add(
        [FromServices] IRequestHandler<AddUserCurrencyRequest, AddUserCurrencyResponse> addUserCurrencyRequestHandler,
        [FromRoute] string currencyId, CancellationToken cancellationToken)
    {
        var request = new AddUserCurrencyRequest(UserId, currencyId);

        var result = await addUserCurrencyRequestHandler.Handle(request, cancellationToken);

        return Created(string.Empty, result);
    }

    [HttpDelete("currency/{currencyId}")]
    public async Task<IActionResult> Remove(
        [FromServices]
        IRequestHandler<RemoveUserCurrencyRequest, RemoveUserCurrencyResponse> removeUserCurrencyRequestHandler,
        [FromRoute] string currencyId, CancellationToken cancellationToken)
    {
        var request = new RemoveUserCurrencyRequest(UserId, currencyId);

        await removeUserCurrencyRequestHandler.Handle(request, cancellationToken);

        return NoContent();
    }
}
