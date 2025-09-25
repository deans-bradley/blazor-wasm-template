namespace BlazorApp.Utils.Services
{
    public interface IHttpService
    {
        Task<T> Get<T>(string uri);
        Task<T> Post<T>(string uri, object value);
        Task<T> Post<T>(string uri);
        Task<T> Put<T>(string uri, object value);
        Task<T> Delete<T>(string uri);
    }
}
