using Application.Interfaces;
using Application.RequestHandlers.GetUserCurrencies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserCurrenciesController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromServices]
        IRequestHandler<GetUserCurrenciesRequest, GetUserCurrenciesResponse> getUserCurrenciesRequestHandler,
        CancellationToken cancellationToken)
    {
        var result =
            await getUserCurrenciesRequestHandler.Handle(new GetUserCurrenciesRequest(UserId), cancellationToken);

        return Ok(result);
    }
}
