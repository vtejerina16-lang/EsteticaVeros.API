using EsteticaVeros.API.Core.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EsteticaVeros.API.UseCases.Identity.ListUsers;

public class ListUsersHandler(
  UserManager<ApplicationUser> userManager,
  ILogger<ListUsersHandler> logger) : IQueryHandler<ListUsersQuery, Result<PagedResult<UserDTO>>>
{
  public async ValueTask<Result<PagedResult<UserDTO>>> Handle(
    ListUsersQuery query, 
    CancellationToken cancellationToken)
  {
    try
    {
      logger.LogInformation("Listing users - Page: {Page}, PerPage: {PerPage}, IsActive: {IsActive}, Role: {Role}, Search: {Search}",
        query.Page, query.PerPage, query.IsActive, query.RoleName, query.SearchTerm);

      // Base query
      var usersQuery = userManager.Users.AsQueryable();

      // Filter by active status
      if (query.IsActive.HasValue)
      {
        usersQuery = usersQuery.Where(u => u.IsActive == query.IsActive.Value);
      }

      // Filter by search term (FirstName, LastName, Email)
      if (!string.IsNullOrWhiteSpace(query.SearchTerm))
      {
        var searchLower = query.SearchTerm.ToLower();
        usersQuery = usersQuery.Where(u => 
          u.FirstName.ToLower().Contains(searchLower) ||
          u.LastName.ToLower().Contains(searchLower) ||
          u.Email!.ToLower().Contains(searchLower)
        );
      }

      // Apply ordering
      usersQuery = usersQuery
        .OrderBy(u => u.FirstName)
        .ThenBy(u => u.LastName);

      // Get total count BEFORE pagination
      var totalCount = await usersQuery.CountAsync(cancellationToken);
      
      logger.LogInformation("Total users found (before pagination): {Count}", totalCount);

      // Apply pagination
      var users = await usersQuery
        .Skip((query.Page - 1) * query.PerPage)
        .Take(query.PerPage)
        .ToListAsync(cancellationToken);

      logger.LogInformation("Users retrieved after pagination: {Count}", users.Count);

      // Map to DTOs and get roles
      var userDtos = new List<UserDTO>();
      foreach (var user in users)
      {
        var roles = await userManager.GetRolesAsync(user);
        var roleName = roles.FirstOrDefault() ?? "None";

        // Filter by role if specified (but after we know the role)
        if (!string.IsNullOrEmpty(query.RoleName) && 
            !roleName.Equals(query.RoleName, StringComparison.OrdinalIgnoreCase))
        {
          continue;
        }

        userDtos.Add(new UserDTO(
          user.Id,
          user.Email!,
          user.FirstName,
          user.LastName,
          user.FullName,
          user.PhoneNumber,
          user.Address,
          user.DateOfBirth,
          user.ProfilePictureUrl,
          user.IsActive,
          user.CreatedAt,
          user.LastLoginAt,
          roleName
        ));
      }

      logger.LogInformation("Final user DTOs count: {Count}", userDtos.Count);

      // Adjust total count if filtering by role
      var adjustedTotalCount = string.IsNullOrEmpty(query.RoleName) 
        ? totalCount 
        : userDtos.Count;

      var totalPages = (int)Math.Ceiling(adjustedTotalCount / (double)query.PerPage);

      var result = new PagedResult<UserDTO>(
        userDtos,
        query.Page,
        query.PerPage,
        adjustedTotalCount,
        totalPages
      );

      return Result.Success(result);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error listing users");
      return Result.Error("Error retrieving users");
    }
  }
}