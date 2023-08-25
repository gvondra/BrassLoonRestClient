namespace BrassLoon.RestClient
{
    public interface IRequestBody
    {
        IRequestContentBuilder CreateContentBuilder();
    }
}
