namespace AdminApi.Dtos;

public record FullUserDto
(
    long Id,
    string Name,
    string Email,
    string Klass,
    List<string> Roles,
    List<ComputerLoanDto> ComputerLoans


);
