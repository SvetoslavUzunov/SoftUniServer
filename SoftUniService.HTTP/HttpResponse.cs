using System.Text;

namespace SoftUniService.HTTP;

public class HttpResponse
{
    public HttpResponse(string contentType, byte[] body, HttpStatusCode statusCode = HttpStatusCode.Ok)
    {
        if (body == null)
        {
            body = new byte[0];
        }

        this.StatusCode = statusCode;
        this.Body = body;
        this.Headers = new List<Header>
        {
            { new Header("Content-Type",contentType) },
            { new Header("CContent-Length", body.Length.ToString()) },
        };
        this.Cookies = new List<Cookie>();
    }

    public HttpStatusCode StatusCode { get; set; }
    public ICollection<Header> Headers { get; set; }
    public ICollection<Cookie> Cookies { get; set; }
    public byte[] Body { get; set; }

    public override string ToString()
    {
        StringBuilder responseBuilder = new StringBuilder();
        responseBuilder.Append($"HTTP/1.1 {(int)this.StatusCode} {StatusCode}" + HttpConstants.NewLine);
        foreach (var header in Headers)
        {
            responseBuilder.Append(header.ToString() + HttpConstants.NewLine);
        }

        foreach (var cookie in this.Cookies)
        {
            responseBuilder.Append($"Set-Cookie: {cookie.ToString()}" + HttpConstants.NewLine);
        }

        responseBuilder.Append(HttpConstants.NewLine);

        return responseBuilder.ToString();
    }
}
