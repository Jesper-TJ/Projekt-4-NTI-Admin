namespace AdminApi.Dtos;

public record BasicUserDto
(
    long Id,
    string Name,
    string Email,
    List<string> Roles,
    string? Image = null
);
