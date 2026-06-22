namespace phase_1.Services.Identity;

public class RoleManager : IRoleManager
{
    private const int PatientRole = 1;
    private const int StudentRole = 2;

    public bool IsSupportedRole(int role) => role == PatientRole || role == StudentRole;

    public bool IsPatient(int role) => role == PatientRole;

    public bool IsStudent(int role) => role == StudentRole;
}
