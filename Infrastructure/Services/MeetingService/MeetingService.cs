using System.Net;
using Domain.DTOs.MeetingDTOs;

using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.MeetingService;

public class MeetingService
    (ILogger<MeetingService> logger, DataContext context) : IMeetingService
{
    #region GetMeetingsAsync

    public async Task<PagedResponse<List<GetMeetingDto>>> GetMeetingsAsync(MeetingFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetMeetingsAsync} in time:{DateTime} ", "GetMeetingsAsync",
                DateTimeOffset.UtcNow);

            var Meetings = context.Meetings.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Description))
                Meetings = Meetings.Where(x => x.Description.ToLower().Contains(filter.Description.ToLower()));

            var response = await Meetings.Select(x => new GetMeetingDto()
            {
                Title = x.Title,
                Description = x.Description,
                Id = x.Id,
                StartDateTime = x.StartDateTime,
                EndDateTime = x.EndDateTime,
                UserId = x.UserId
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await Meetings.CountAsync();

            logger.LogInformation("Finished method {GetMeetingsAsync} in time:{DateTime} ", "GetMeetingsAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetMeetingDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetMeetingDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetMeetingByIdAsync

    public async Task<Response<GetMeetingDto>> GetMeetingByIdAsync(int MeetingId)
    {
        try
        {
            logger.LogInformation("Starting method {GetMeetingByIdAsync} in time:{DateTime} ", "GetMeetingByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Meetings.Where(x => x.Id == MeetingId).Select(x => new GetMeetingDto()
            {
                Title = x.Title,
                Description = x.Description,
                Id = x.Id,
                StartDateTime = x.StartDateTime,
                EndDateTime = x.EndDateTime,
                UserId = x.UserId
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found Meeting with id={Id},time={DateTimeNow}", MeetingId, DateTime.UtcNow);
                return new Response<GetMeetingDto>(HttpStatusCode.BadRequest, "Meeting not found");
            }

            logger.LogInformation("Finished method {GetMeetingByIdAsync} in time:{DateTime} ", "GetMeetingByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetMeetingDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetMeetingDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateMeetingAsync

    public async Task<Response<string>> CreateMeetingAsync(CreateMeetingDto createMeeting)
    {
        try
        {
            logger.LogInformation("Starting method {CreateMeetingAsync} in time:{DateTime} ", "CreateMeetingAsync",
                DateTimeOffset.UtcNow);
            var newMeeting = new Meeting()
            {
                Description = createMeeting.Description,
                Title = createMeeting.Title,
                UserId = createMeeting.UserId,
                StartDateTime = createMeeting.StartDateTime,
                EndDateTime = createMeeting.EndDateTime
            };
            await context.Meetings.AddAsync(newMeeting);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {CreateMeetingAsync} in time:{DateTime} ", "CreateMeetingAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created a new Meeting by id:{newMeeting.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateMeetingAsync

    public async Task<Response<string>> UpdateMeetingAsync(UpdateMeetingDto updateMeeting)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateMeetingAsync} in time:{DateTime} ", "UpdateMeetingAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Meetings.FirstOrDefaultAsync(x => x.Id == updateMeeting.Id);
            if (existing is null)
            {
                logger.LogWarning("Meeting not found by id:{Id},time:{DateTimeNow} ", updateMeeting.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "Meeting not found");
            }

            existing.Description = updateMeeting.Description;
            existing.Title = updateMeeting.Title!;
            existing.StartDateTime = updateMeeting.StartDateTime;
            existing.EndDateTime = updateMeeting.EndDateTime;
            existing.UserId = updateMeeting.UserId;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateMeetingAsync} in time:{DateTime} ", "UpdateMeetingAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated Meeting by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteMeetingAsync

    public async Task<Response<bool>> DeleteMeetingAsync(int MeetingId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteMeetingAsync} in time:{DateTime} ", "DeleteMeetingAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Meetings.FirstOrDefaultAsync(x => x.Id == MeetingId);
            if (existing == null)
                return new Response<bool>(HttpStatusCode.BadRequest, $"Meeting not found by id:{MeetingId}");

            logger.LogInformation("Finished method {DeleteMeetingAsync} in time:{DateTime} ", "DeleteMeetingAsync",
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