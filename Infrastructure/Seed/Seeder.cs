using Domain.Constants;
using Domain.DTOs.RolePermissionDTOs;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Services.HashService;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seed;

public class Seeder(DataContext context, IHashService hashService)
{
    public async Task Initial()
    {
        await SeedRole();
        await DefaultUsers();
    }



    private async Task SeedRole()
    {
        try
        {
            var newRoles = new List<Role>()
            {
                new()
                {
                    Name = Roles.Admin,
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new()
                {
                    Name = Roles.User,
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
            };

            var existing = await context.Roles.ToListAsync();
            foreach (var role in newRoles)
            {
                if (existing.Exists(e => e.Name == role.Name) == false)
                {
                    await context.Roles.AddAsync(role);
                }
            }

            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
        }
    }


    private async Task DefaultUsers()
    {
        try
        {
            var existingAdmin = await context.Users.FirstOrDefaultAsync(x => x.Username == "Admin");
            if (existingAdmin is null)
            {
                var superAdmin = new User()
                {
                    Email = "admin@gmail.com",
                    Username = "Admin",
                    Phone = "985172315",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow,
                    Password = hashService.ConvertToHash("1234")
                };
                await context.Users.AddAsync(superAdmin);
                await context.SaveChangesAsync();var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Username == "Admin");
                var existingRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.Admin);
                if (existingUser is not null && existingRole is not null)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = existingRole.Id,
                        UserId = existingUser.Id,
                        Role = existingRole,
                        User = existingUser,
                        UpdateAt = DateTimeOffset.UtcNow,
                        CreateAt = DateTimeOffset.UtcNow
                    };
                    await context.UserRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();
                }

            }
                await AddAdminPermissions();



            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == "User");
            if (user is null)
            {
                var superAdmin = new User()
                {
                    Email = "user@gmail.com",
                    Phone = "985172315",
                    Username = "User",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow,
                    Password = hashService.ConvertToHash("1234")
                };
                await context.Users.AddAsync(superAdmin);
                await context.SaveChangesAsync();

                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Username == "User");
                var existingRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.User);
                if (existingUser is not null && existingRole is not null)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = existingRole.Id,
                        UserId = existingUser.Id,
                        Role = existingRole,
                        User = existingUser,
                        UpdateAt = DateTimeOffset.UtcNow,
                        CreateAt = DateTimeOffset.UtcNow
                    };
                    await context.UserRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();
                }

            }
                await AddUserPermissions();
        }
        catch (Exception e)
        {
        }
    }

    private async Task AddUserPermissions()
    {
        var userRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.User);
        if (userRole == null) return;
        var userClaims = new List<RoleClaimsDto>()
        {
            new("Permissions", Permissions.Permissions.Users.View),
            new("Permissions", Permissions.Permissions.Roles.View),
            new("Permissions", Permissions.Permissions.Meetings.View),
            new("Permissions", Permissions.Permissions.Notifications.View),
            new("Permissions", Permissions.Permissions.UserRoles.View),
        };var existingClaim = await context.RoleClaims.Where(x => x.RoleId == userRole.Id).ToListAsync();
        foreach (var claim in userClaims)
        {
            if (!existingClaim.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
            {
                await context.AddPermissionClaim(userRole, claim.Value);
            }
        }
    }



    private async Task AddAdminPermissions()
    {
        var adminRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.Admin);
        if (adminRole == null) return;
        var userClaims = new List<RoleClaimsDto>()
        {
            new("Permissions", Permissions.Permissions.Users.View),
            new("Permissions", Permissions.Permissions.Users.Create),
            new("Permissions",Permissions.Permissions.Users.Edit),
            new("Permissions",Permissions.Permissions.Users.Delete),

            new("Permissions", Permissions.Permissions.Roles.View),
            new("Permissions", Permissions.Permissions.Roles.Create),
            new("Permissions",Permissions.Permissions.Roles.Edit),
            new("Permissions",Permissions.Permissions.Roles.Delete),

            new("Permissions", Permissions.Permissions.UserRoles.View),
            new("Permissions", Permissions.Permissions.UserRoles.Create),
            new("Permissions",Permissions.Permissions.UserRoles.Edit),
            new("Permissions",Permissions.Permissions.UserRoles.Delete),

            new("Permissions", Permissions.Permissions.Meetings.View),
            new("Permissions", Permissions.Permissions.Meetings.Create),
            new("Permissions",Permissions.Permissions.Meetings.Edit),
            new("Permissions",Permissions.Permissions.Meetings.Delete),

            new("Permissions", Permissions.Permissions.Notifications.View),
            new("Permissions", Permissions.Permissions.Notifications.Create),
            new("Permissions",Permissions.Permissions.Notifications.Edit),
            new("Permissions",Permissions.Permissions.Notifications.Delete),

        };

        var existingClaim = await context.RoleClaims.Where(x => x.RoleId == adminRole.Id).ToListAsync();
        foreach (var claim in userClaims)
        {
            if (!existingClaim.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
            {
                await context.AddPermissionClaim(adminRole, claim.Value);
            }
        }
    }

}