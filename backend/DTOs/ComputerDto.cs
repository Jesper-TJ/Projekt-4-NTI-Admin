namespace AdminApi.Dtos;

public record ComputerDto
(
    long Id,
    string Name,
    string Serial,
    string State,
    string Status,
    List<string>? ComputerLogs, // Assuming it's a list of strings
    DateTime EntryDate
);
