namespace Eshopper_website.Services.Recaptcha
{
    public interface IRecaptchaService
    {
        Task<bool> VerifyToken(string token);
        string LastError { get; }
    }
}
