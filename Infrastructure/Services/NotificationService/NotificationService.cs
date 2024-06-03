using System.Net;
using Domain.DTOs.NotificationDTOs;

using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.NotificationService;

public class NotificationService
    (ILogger<NotificationService> logger, DataContext context) : INotificationService
{
    #region GetNotificationsAsync

    public async Task<PagedResponse<List<GetNotificationDto>>> GetNotificationsAsync(NotificationFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetNotificationsAsync} in time:{DateTime} ", "GetNotificationsAsync",
                DateTimeOffset.UtcNow);

            var Notifications = context.Notifications.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Message))
                Notifications = Notifications.Where(x => x.Message.ToLower().Contains(filter.Message.ToLower()));

            var response = await Notifications.Select(x => new GetNotificationDto()
            {
                MeetingId = x.MeetingId,
                UserId = x.UserId,
                Id = x.Id,
                Message = x.Message,
                SentDateTime = x.SentDateTime,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await Notifications.CountAsync();

            logger.LogInformation("Finished method {GetNotificationsAsync} in time:{DateTime} ", "GetNotificationsAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetNotificationDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetNotificationDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetNotificationByIdAsync

    public async Task<Response<GetNotificationDto>> GetNotificationByIdAsync(int NotificationId)
    {
        try
        {
            logger.LogInformation("Starting method {GetNotificationByIdAsync} in time:{DateTime} ", "GetNotificationByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Notifications.Where(x => x.Id == NotificationId).Select(x => new GetNotificationDto()
            {
                MeetingId = x.MeetingId,
                UserId = x.UserId,
                Id = x.Id,
                Message = x.Message,
                SentDateTime = x.SentDateTime,
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found Notification with id={Id},time={DateTimeNow}", NotificationId, DateTime.UtcNow);
                return new Response<GetNotificationDto>(HttpStatusCode.BadRequest, "Notification not found");
            }

            logger.LogInformation("Finished method {GetNotificationByIdAsync} in time:{DateTime} ", "GetNotificationByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetNotificationDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetNotificationDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateNotificationAsync

    public async Task<Response<string>> CreateNotificationAsync(CreateNotificationDto createNotification)
    {
        try
        {
            logger.LogInformation("Starting method {CreateNotificationAsync} in time:{DateTime} ", "CreateNotificationAsync",
                DateTimeOffset.UtcNow);
            var newNotification = new Notification()
            {
                MeetingId = createNotification.MeetingId,
                Message = createNotification.Message,
                UserId = createNotification.UserId,
                SentDateTime = createNotification.SentDateTime,
            };
            await context.Notifications.AddAsync(newNotification);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreateNotificationAsync} in time:{DateTime} ", "CreateNotificationAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new Notification by id:{newNotification.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateNotificationAsync

    public async Task<Response<string>> UpdateNotificationAsync(UpdateNotificationDto updateNotification)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateNotificationAsync} in time:{DateTime} ", "UpdateNotificationAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Notifications.FirstOrDefaultAsync(x => x.Id == updateNotification.Id);
            if (existing is null)
            {
                logger.LogWarning("Notification not found by id:{Id},time:{DateTimeNow} ", updateNotification.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "Notification not found");
            }

            existing.MeetingId = updateNotification.MeetingId;
            existing.Message = updateNotification.Message!;
            existing.SentDateTime = updateNotification.SentDateTime;
            existing.UserId = updateNotification.UserId;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateNotificationAsync} in time:{DateTime} ", "UpdateNotificationAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated Notification by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteNotificationAsync

    public async Task<Response<bool>> DeleteNotificationAsync(int NotificationId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteNotificationAsync} in time:{DateTime} ", "DeleteNotificationAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Notifications.FirstOrDefaultAsync(x => x.Id == NotificationId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"Notification not found by id:{NotificationId}");

            logger.LogInformation("Finished method {DeleteNotificationAsync} in time:{DateTime} ", "DeleteNotificationAsync",
                DateTimeOffset.UtcNow);
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
}