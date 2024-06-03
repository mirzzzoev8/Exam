namespace Domain.DTOs.NotificationDTOs;

public class UpdateNotificationDto
{
    public int Id { get; set; }
    public string? Message { get; set; }
    public DateTime SentDateTime { get; set; }
    public int MeetingId { get; set; }
    public int UserId { get; set; }
}
