using ProductClientHub.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordBuilder
{
    public static RequestChangePassword Build(string currentPassword, string newPassword)
    {
        return new RequestChangePassword
        {
            CurrentPassword = currentPassword,
            NewPassword = newPassword
        };
    }
}
