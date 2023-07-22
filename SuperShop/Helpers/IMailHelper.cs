namespace SuperShop.Helpers
{
    public interface IMailHelper
    {
        // Metodo que nao retorna nada
        Response SendEmail(string to, string subject, string body);


    }
}
