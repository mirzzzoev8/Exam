namespace Domain.DTOs.NotificationDTOs;

public class CreateNotificationDto
{
    public string? Message { get; set; }
    public DateTime SentDateTime { get; set; }
    public int MeetingId { get; set; }
    public int UserId { get; set; }
}
