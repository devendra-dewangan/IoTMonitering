using IoTMonitoring.App.Services;
using IoTMonitoring.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoTMonitoring.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceAuthController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceAuthController(IDeviceService deviceService, IAuthService authService)
        {
            _deviceService = deviceService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> IsDeviceRegistered()
        {
            return Ok();
        }

        [HttpPost(Name = "RequestRegister")]
        public IActionResult RequestRegister(DeviceCreateDto dto)
        {
            _deviceService.AddDeviceToTempList(dto);
            return Ok();
        }
    }
}
