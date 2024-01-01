using Microsoft.AspNetCore.Mvc;

namespace SensorReceiverServer.Controllers;

[ApiController]
public class IMUController : ControllerBase
{
    private readonly ILogger<IMUController> _logger;

    public IMUController(ILogger<IMUController> logger)
    {
        _logger = logger;
    }


    [HttpPost]
    [Route("imu/receiveData")]
    public async Task<ActionResult> ReceiveData([FromForm] string justSomething)
    {
        _logger.LogInformation("Received: " + justSomething);

        return StatusCode(200);
    }
}
