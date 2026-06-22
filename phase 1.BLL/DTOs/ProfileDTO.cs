namespace phase_1.DTOs;

public class ProfileDTO
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? Email { get; set; }
    public int Role { get; set; }
    public int Status { get; set; }
    public bool IsPhoneVerified { get; set; }
    public bool IsEmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public PatientProfileDTO? PatientProfile { get; set; }
    public StudentProfileDTO? StudentProfile { get; set; }
}

public class PatientProfileDTO
{
    public DateTime? BirthDate { get; set; }
    public string? Gender { get; set; }
    public string? Notes { get; set; }
}

public class StudentProfileDTO
{
    public int FacultyId { get; set; }
    public string? FacultyName { get; set; }
    public int AcademicYear { get; set; }
    public string? ClinicName { get; set; }
    public string? StudentCode { get; set; }
    public string? SupervisorName { get; set; }
    public int RequiredCasesCount { get; set; }
    public int CompletedCasesCount { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerifiedAt { get; set; }
}
