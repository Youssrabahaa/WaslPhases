namespace phase_1.Services.Identity;

public interface IRoleManager
{
    bool IsSupportedRole(int role);

    bool IsPatient(int role);

    bool IsStudent(int role);
}
