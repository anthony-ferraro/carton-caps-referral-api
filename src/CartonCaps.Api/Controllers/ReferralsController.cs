using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using CartonCaps.Api.Models;
using CartonCaps.Api.Models.Enums;
using CartonCaps.Api.Models.Responses;
using CartonCaps.Api.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace CartonCaps.Api.Controllers;

/// <summary>
/// Manages referral-related operations including code generation, sharing, and tracking.
/// </summary>
[ApiController]
[Route("v1/referrals")]
[EnableRateLimiting("fixed")]
[SwaggerTag("Manage and track referrals in the Carton Caps app")]
public class ReferralsController : ControllerBase
{
    private readonly ILogger<ReferralsController> _logger;
    private readonly IReferralService _referralService;

    public ReferralsController(ILogger<ReferralsController> logger, IReferralService referralService)
    {
        _logger = logger;
        _referralService = referralService;
    }

    /// <summary>
    /// Retrieves the current user's referral code.
    /// </summary>
    /// <remarks>
    /// Returns the user's unique referral code that can be copied or used to generate referral links.
    /// </remarks>
    /// <response code="200">Returns the referral code</response>
    /// <response code="429">If too many requests have been made</response>
    [HttpGet("my-code")]
    [ProducesResponseType(typeof(ReferralCode), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [SwaggerOperation(
        Summary = "Get user's referral code",
        Description = "Retrieves the current user's referral code."
    )]
    public async Task<ActionResult<ReferralCode>> GetMyReferralCode()
    {
        _logger.LogInformation("Getting referral code for user.");
                
        // In a real implementation, we would get the user ID from the auth token
        var userId = "test-user";
        var result = await _referralService.GetReferralCodeAsync(userId);
        
        return Ok(result);
    }

    /// <summary>
    /// Generates a new referral link for sharing.
    /// </summary>
    /// <remarks>
    /// Creates a deferred deep link that can be shared with potential new users.
    /// The link will direct users to:
    /// - App Store/Play Store for installation
    /// - Custom onboarding experience after installation
    /// 
    /// </remarks>
    /// <param name="shareMethod">The method that will be used to share the link</param>
    /// <response code="201">Returns the generated referral link</response>
    /// <response code="400">If the share method is invalid</response>
    /// <response code="429">If too many links have been generated recently</response>
    [HttpPost("generate-link/{shareMethod}")]
    [ProducesResponseType(typeof(GenerateReferralLinkResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [SwaggerOperation(
        Summary = "Generate a referral link",
        Description = "Creates a new deferred deep link that can be shared with potential new users."
    )]
    public async Task<ActionResult<GenerateReferralLinkResponse>> GenerateReferralLink(ShareMethodType shareMethod)
    {
        if (!Enum.IsDefined(typeof(ShareMethodType), shareMethod))
        {
            return BadRequest(new Error
            {
                Code = ReferralErrorCode.invalid_share_method.ToString(),
                Message = "Invalid share method specified"
            });
        }

        _logger.LogInformation(
            "Generating referral link for share method: {ShareMethod}", 
            shareMethod
        );
        
        var userId = "test-user";
        var result = await _referralService.GenerateReferralLinkAsync(userId, shareMethod);

        return CreatedAtAction(nameof(GetMyReferralCode), result);
    }

    /// <summary>
    /// Retrieves the user's referral history.
    /// </summary>
    /// <remarks>
    /// Returns a list of referrals showing:
    /// - Referred user's name
    /// - Current status
    /// - Which sharing method was used
    /// - When the referral was created
    /// - When the referral was completed (if applicable)
    /// - Which school benefits from the referral
    /// 
    /// </remarks>
    /// <response code="200">Returns the list of referrals</response>
    /// <response code="429">If too many requests have been made</response>
    [HttpGet("history")]
    [ProducesResponseType(typeof(IEnumerable<ReferralHistory>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [SwaggerOperation(
        Summary = "Get referral history",
        Description = "Retrieves a list of the user's referrals with detailed metadata."
    )]
    public async Task<ActionResult<IEnumerable<ReferralHistory>>> GetReferralHistory()
    {
        _logger.LogInformation("Getting referral history.");
                
        // In a real implementation, we would get the user ID from the auth token
        var userId = "test-user";
        var result = await _referralService.GetReferralHistoryAsync(userId);
        
        return Ok(result);
    }

    /// <summary>
    /// Validates a referral code.
    /// </summary>
    /// <remarks>
    /// Used during new user onboarding to:
    /// - Validate the referral code
    /// - Get information to customize the onboarding experience
    /// - Show the referred user which school they can support
    /// 
    /// </remarks>
    /// <param name="code">The referral code to validate</param>
    /// <response code="200">Returns validation result and onboarding information</response>
    /// <response code="404">If the referral code is invalid</response>
    /// <response code="429">If too many validation attempts have been made recently</response>
    [HttpGet("validate/{code}")]
    [ProducesResponseType(typeof(ReferralValidation), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [SwaggerOperation(
        Summary = "Validate a referral code",
        Description = "Validates a referral code and returns information to customize the onboarding experience."
    )]
    public async Task<IActionResult> ValidateReferralCode(string code)
    {
        _logger.LogInformation("Validating referral code: {code}", code);

        try 
        {
            var result = await _referralService.ValidateReferralCodeAsync(code);
            return Ok(result);
        }
        catch (ReferralException ex)
        {
            return NotFound(new Error 
            { 
                Code = ex.ErrorCode.ToString(),
                Message = ex.Message
            });
        }
    }
} 