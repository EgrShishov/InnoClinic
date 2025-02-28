using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AuthorizationController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly UserManager<Account> _userManager;

    public AuthorizationController(IMediator mediator, IMapper mapper, UserManager<Account> userManager)
    {
        _mediator = mediator;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(SignInRequest request)
    {
        var command = _mapper.Map<SignInCommand>(request);
        var signInResult = await _mediator.Send(command);

        return signInResult.Match(
            response => 
            {
                Response.Cookies.Append("access", response.AccessToken);
                Response.Cookies.Append("refresh", response.RefreshToken);
                return Ok(response);
            },
            errors => Problem(errors));
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(SignUpRequest request)
    {
        var command = _mapper.Map<SignUpCommand>(request);
        var signUpResult = await _mediator.Send(command);

        return signUpResult.Match(
            response =>
            {
                Response.Cookies.Append("access", response.AccessToken);
                Response.Cookies.Append("refresh", response.RefreshToken);
                return Ok(response);
            },
            errors => Problem(errors));
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAccount([FromForm] CreateAccountRequest request)
    {
        var command = new CreateAccountCommand(
            request.Email, 
            request.PhoneNumber, 
            request.Photo, 
            request.CreatedBy,
            request.Role);

        var signUpResult = await _mediator.Send(command);

        return signUpResult.Match(
            response =>
            {
                Response.Cookies.Append("access", response.AccessToken);
                Response.Cookies.Append("refresh", response.RefreshToken);

                return Ok(response);
            },
            errors => Problem(errors));
    }

    [HttpPut("account/{id}")]
    public async Task<IActionResult> EditAccount(int AccountId, EditAccountRequest request)
    {
        var command = new EditAccountCommand(AccountId, request.UpdatedBy, request.PhoneNumber, request.Photo);

        var editAccountResult = await _mediator.Send(command);

        return editAccountResult.Match(
            _ => Ok(),
            errors => Problem(errors));
    }

    [HttpPost("sign-out")]
    public async Task<IActionResult> SignOut()
    {
        Response.Cookies.Delete("refresh");
        return NoContent();
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        var refreshTokenResult = await _mediator.Send(new RefreshTokenCommand(request.AccessToken, request.RefreshToken));
        
        if (refreshTokenResult.IsError)
        {
            return BadRequest(refreshTokenResult.FirstError);
        }

        return refreshTokenResult.Match(
            response =>
            {
                Response.Cookies.Delete("access");
                Response.Cookies.Append("access", response.AccessToken);

                return Ok(response);
            },
            errors => Problem(errors));
    }

    [HttpPost("email-verify")]
    public async Task<IActionResult> VerifyEmail(string link)
    {
        var id = User.GetUserId();

        var verificationResult = await _mediator.Send(new VerifyEmailCommand(id, link));

        return verificationResult.Match(
            response => Ok(),
            errors => Problem(errors));
    }

    [HttpGet("account/{id:int}")]
    public async Task<IActionResult> GetAccount(int id)
    {
        var result = await _mediator.Send(new GetAccountByIdQuery(id));

        return result.Match(
            response => Ok(response),
            errors => Problem(errors));
    }

    [HttpDelete("account/{id:int}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var result = await _mediator.Send(new DeleteAccountCommand(id));

        return result.Match(
            _ => NoContent(),
            errors => Problem(errors));
    }
}
