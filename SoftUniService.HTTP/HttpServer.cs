using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SoftUniService.HTTP;

public class HttpServer : IHttpServer
{
    private static IDictionary<string, Func<HttpRequest, HttpResponse>> routeTable;

    public HttpServer()
    {
        routeTable = new Dictionary<string, Func<HttpRequest, HttpResponse>>();
    }

    public void AddRoute(string path, Func<HttpRequest, HttpResponse> action)
    {
        if (!routeTable.ContainsKey(path))
        {
            routeTable.Add(path, action);
        }
        else
        {
            routeTable[path] = action;
        }
    }

    public async Task StartAsync(int port = 80)
    {
        TcpListener listener = new TcpListener(IPAddress.Loopback, port);
        listener.Start();
        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            ProcessClientAsync(client);
        }
    }

    private static async Task ProcessClientAsync(TcpClient client)
    {
        try
        {
            using NetworkStream stream = client.GetStream();
            List<byte> data = new List<byte>();
            byte[] buffer = new byte[HttpConstants.bufferSize];
            int position = 0;
            while (true)
            {
                int countRead = await stream.ReadAsync(buffer, position, buffer.Length);
                position += countRead;

                if (countRead >= buffer.Length)
                {
                    data.AddRange(buffer);
                }
                else
                {
                    byte[] partialBuffer = new byte[countRead];
                    Array.Copy(buffer, partialBuffer, countRead);
                    data.AddRange(partialBuffer);
                    break;
                }
            }

            var requestAsString = Encoding.UTF8.GetString(data.ToArray());
            var request = new HttpRequest(requestAsString);
            Console.WriteLine($"{request.Method} {request.Path} => {request.Headers.Count}");

            HttpResponse response;
            if (routeTable.ContainsKey(request.Path))
            {
                var action = routeTable[request.Path];
                response = action(request);
            }
            else
            {
                response = new HttpResponse("text/html", new byte[0], HttpStatusCode.NotFound);
            }

            response.Headers.Add(new Header("Server", "SoftUni Server 1.0"));
            response.Cookies.Add(new ResponseCookie("sid", Guid.NewGuid().ToString())
            {
                HttpOnly = true,
                MaxAge = 60 * 24 * 60 * 60
            });
            var responseHeaderBytes = Encoding.UTF8.GetBytes(response.ToString());
            await stream.WriteAsync(responseHeaderBytes, 0, responseHeaderBytes.Length);
            await stream.WriteAsync(response.Body, 0, response.Body.Length);

            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}