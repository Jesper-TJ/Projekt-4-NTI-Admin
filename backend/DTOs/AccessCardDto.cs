namespace AdminApi.Dtos;

public record AccessCardDataDto
(
    long Id,
    string Name,
    string UniqueId, //     (<ntijoh><startår><tresiffrigt löpnummer>) - for students : (-<två-bokstäver från förnamnet><två-bokstäver från efternamnet>) - for staff
    string BarCodeFilePath,
    List<string> Roles,
    string? ImageFilePath
);

public record FinishedAccessCardDto
(
    long Id,
    string Image64Encoded
);
