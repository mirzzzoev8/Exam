using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.MeetingDTOs;

public class CreateMeetingDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public int UserId { get; set; }

}
