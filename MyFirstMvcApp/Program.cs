using SoftUniService.HTTP;
using System.Text;

namespace MyFirstMvcApp;

public class Program
{
    static async Task Main(string[] args)
    {
        IHttpServer server = new HttpServer();
        server.AddRoute("/", Home);
        //server.AddRoute("/favicon.ico", Favicon);
        server.AddRoute("/about", About);
        server.AddRoute("/users/login", Login);
        await server.StartAsync(80);
    }

    //TODO: Add favicon
    public static HttpResponse Favicon(HttpRequest arg)
    {
        throw new NotImplementedException();
    }

    public static HttpResponse Home(HttpRequest request)
    {
        var responseHtml = "<h1>Welcome!</h1>"
               + request.Headers.FirstOrDefault(x => x.Name == "User-Agent")?.Value;
        var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
        var response = new HttpResponse("text/html", responseBodyBytes);

        return response;
    }

    public static HttpResponse About(HttpRequest request)
    {
        var responseHtml = "<h1>About...!</h1>";
        var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
        var response = new HttpResponse("text/html", responseBodyBytes);

        return response;
    }

    public static HttpResponse Login(HttpRequest request)
    {
        var responseHtml = "<h1>Login...!</h1>";
        var responseBodyBytes = Encoding.UTF8.GetBytes(responseHtml);
        var response = new HttpResponse("text/html", responseBodyBytes);

        return response;
    }
}