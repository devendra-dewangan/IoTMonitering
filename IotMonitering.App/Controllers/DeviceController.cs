using IoTMonitoring.App.Services;
using IoTMonitoring.Models;
using IoTMonitoring.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController(IDeviceService service) : ControllerBase
    {
        private IDeviceService _deviceService = service;

        [HttpPost]
        public async Task<IActionResult> CreateDevice(DeviceCreateDto dto)
        {
            
            var device = await _deviceService.AddDeviceAsync(dto);

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

        [HttpGet]
        public async Task<IActionResult> GetDevices(string userID)
        {
            
            var devices = await _deviceService.GetAllDevicesAsync(userID);

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
