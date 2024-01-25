using PushnotificationsBack.DataTransferObjects;

namespace PushnotificationsBack.Services.FirebaseManager
{
    public interface IFirebaseManager
    {
        string Token { get; set; }
        Task SendMessage(MessageDTO messageDTO);
    }
}
