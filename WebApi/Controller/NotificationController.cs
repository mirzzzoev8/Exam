namespace WebApi.Controller;

using Domain.DTOs.NotificationDTOs;
using Domain.Filters;
using Infrastructure.Permissions;
using Infrastructure.Services.NotificationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController(INotificationService NotificationService) : ControllerBase
{
    [HttpGet("Notifications")]
    [PermissionAuthorize(Permissions.Notifications.View)]
    public async Task<IActionResult> GetNotifications([FromQuery] NotificationFilter filter)
    {
        var res1 = await NotificationService.GetNotificationsAsync(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("{NotificationId:int}")]
    [PermissionAuthorize(Permissions.Notifications.View)]
    public async Task<IActionResult> GetNotificationById(int NotificationId)
    {
        var res1 = await NotificationService.GetNotificationByIdAsync(NotificationId);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.Notifications.Create)]
    public async Task<IActionResult> CreateNotification([FromForm] CreateNotificationDto createNotification)
    {
        var result = await NotificationService.CreateNotificationAsync(createNotification);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.Notifications.Edit)]
    public async Task<IActionResult> UpdateNotification([FromForm] UpdateNotificationDto updateNotification)
    {
        var result = await NotificationService.UpdateNotificationAsync(updateNotification);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{NotificationId:int}")]
    [PermissionAuthorize(Permissions.Notifications.Delete)]
    public async Task<IActionResult> DeleteNotificationAsync(int NotificationId)
    {
        var result = await NotificationService.DeleteNotificationAsync(NotificationId);
        return StatusCode(result.StatusCode, result);
    }
}
