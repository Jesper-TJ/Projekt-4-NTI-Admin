namespace AdminApi.Dtos;

public record ComputerLoanDto
(
    long Id,
    long UserId,
    string? User,
    long ComputerId,
    ComputerDto Computer, // Embedded computer object
    DateTime ReturnDate,
    DateTime? ReturnedAt, // Nullable for loans not yet returned
    DateTime EntryDate
    
);
