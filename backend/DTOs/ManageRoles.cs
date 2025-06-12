namespace AdminApi.Dtos;

public record TeacherPreview(
    long Id,
    string Name,
    string Email,
    List<string> Roles
);