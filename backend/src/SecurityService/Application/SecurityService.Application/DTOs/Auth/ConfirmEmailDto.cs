namespace SecurityService.Application.DTOs.Auth
{
    public class ConfirmEmailDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
