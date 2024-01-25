using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using PushnotificationsBack.DataTransferObjects;
using PushnotificationsBack.Services.FirebaseManager;

namespace PushnotificationsBack.Controllers
{
    [ApiController]
    [Route("api")]
    public class Messages : ControllerBase
    {
        private FirebaseAdmin.FirebaseApp _App;
        private readonly IFirebaseManager _FirebaseManager;

        public Messages(IFirebaseManager firebaseManager)
        {
            _FirebaseManager = firebaseManager;
        }

        [HttpPost]
        [Route("send-message")]        
        public async Task<IActionResult> SendMessage([FromBody] MessageDTO messageDTO)
        {
            if (string.IsNullOrEmpty(_FirebaseManager.Token))
            {
                return BadRequest("Token not delivered.");
            }
            try
            {
                _FirebaseManager.SendMessage(messageDTO);
            }
            catch (Exception ex)
            {

            }
            return Ok("Message sent!");
        }

        [HttpPost]
        [Route("update-token")]
        public async Task<IActionResult> UpdateTokenAsync([FromBody] string token)
        {
            try
            {
                _FirebaseManager.Token = token;
            }
            catch (Exception exc)
            {

            }
            return Ok("Token updated.");
        }

    }
}
