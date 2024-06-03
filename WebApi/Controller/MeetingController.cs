using Domain.DTOs.MeetingDTOs;
using Domain.Filters;
using Infrastructure.Permissions;
using Infrastructure.Services.MeetingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MeetingController(IMeetingService MeetingService) : ControllerBase
{
    [HttpGet("Meetinges")]
    [PermissionAuthorize(Permissions.Meetings.View)]
    public async Task<IActionResult> GetMeetings([FromQuery] MeetingFilter filter)
    {
        var res1 = await MeetingService.GetMeetingsAsync(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("{MeetingId:int}")]
    [PermissionAuthorize(Permissions.Meetings.View)]
    public async Task<IActionResult> GetMeetingById(int MeetingId)
    {
        var res1 = await MeetingService.GetMeetingByIdAsync(MeetingId);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.Meetings.Create)]
    public async Task<IActionResult> CreateMeeting([FromForm] CreateMeetingDto createMeeting)
    {
        var result = await MeetingService.CreateMeetingAsync(createMeeting);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.Meetings.Edit)]
    public async Task<IActionResult> UpdateMeeting([FromForm] UpdateMeetingDto updateMeeting)
    {
        var result = await MeetingService.UpdateMeetingAsync(updateMeeting);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{MeetingId:int}")]
    [PermissionAuthorize(Permissions.Meetings.Delete)]
    public async Task<IActionResult> ChangePassword(int MeetingId)
    {
        var result = await MeetingService.DeleteMeetingAsync(MeetingId);
        return StatusCode(result.StatusCode, result);
    }
}