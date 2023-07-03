using Domain.Entities;

namespace Domain.Interface
{
    public interface ISmsCodeSender
    {
        Task SendCodeAsync(PhoneNumber phoneNumber, string code);
    }
}
