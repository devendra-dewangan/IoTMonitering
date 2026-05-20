using IoTMonitoring.App.Services;
using IoTMonitoring.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IoTMonitoring.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DeviceController(IDeviceService service) : ControllerBase
    {
        private IDeviceService _deviceService = service;

        [HttpPost("SaveDevice")]
        public async Task<IActionResult> CreateDevice(string key)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var device = await _deviceService.RegisterDevice(key,userId);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(new 
            {
                key = device.DeviceKey,
                Name = device.Name,
                Type = device.Type,
            });
        }

        [HttpGet("DeviceList")]
        public async Task<IActionResult> GetDevices()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var devices = await _deviceService.GetAllDevicesAsync(userId);

            if (!devices.Any())
            {
                return NoContent();
            }

            return Ok(devices.Select(d => new
            {
                Id = d.DeviceKey,
                Name = d.Name,
                Type = d.Type,
            }));
        }

        [HttpGet(Name = "RequestedDevice")]
        public async Task<IActionResult> GetReqDevices()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var devices = _deviceService.GetRequestedDevices(userId);

            if (!devices.Any())
            {
                return NoContent();
            }

            return Ok(devices.Select(d => new
            {
                Id = d.DeviceKey,
                Name = d.Name,
                Type = d.Type,
            }));
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<DeviceReadDto>> GetDevice(string key)
        {
            var device = await _deviceService.GetDeviceByIdAsync(key);
            return Ok(device);
        }

        

        [HttpPut("{key}")]
        public async Task<IActionResult> UpdateDevice(string key, DeviceUpdateDto dto)
        {
            var device = await _deviceService.UpdateDeviceAsync(key, dto);
            if (device == null)
            {
                return NotFound();
            }
            
            return Ok(device);
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteDevice(string key)
        {
            await _deviceService.DeleteDeviceAsync(key);
            return Ok();
        }
    }
}
