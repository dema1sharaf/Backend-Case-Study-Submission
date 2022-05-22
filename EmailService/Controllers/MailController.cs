using EmailService.WebApi.Models;
using EmailService.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.WebApi.Controllers
{
[Route("api/[controller]")]
[ApiController]
public class MailController : ControllerBase
{
    private readonly IMailService mailService;

    public MailController(IMailService mailService)
    {
        this.mailService = mailService;
    }
    [HttpPost("send")]
    public async Task<IActionResult> SendMail([FromForm]MailRequest request)
    {
        try
        {
            await mailService.SendEmailAsync(request);
            return Ok();
        }
        catch (Exception ex)
        {

            throw;
        }
            
    }
}
}