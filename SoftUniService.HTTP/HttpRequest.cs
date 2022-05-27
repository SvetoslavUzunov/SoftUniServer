using System.Text;

namespace SoftUniService.HTTP;

public class HttpRequest
{
    public HttpRequest(string request)
    {
        this.Headers = new List<Header>();
        this.Cookies = new List<Cookie>();

        var lines = request.Split(new string[] { HttpConstants.NewLine }, StringSplitOptions.None);

        var header = lines[0];
        var headerParts = header.Split();
        this.Method = (HttpMethod)Enum.Parse(typeof(HttpMethod), headerParts[0], true);
        this.Path = headerParts[1];

        bool isInHeader = true;
        int lineIndex = 1;
        StringBuilder bodyBuilder = new StringBuilder();
        while (lineIndex < lines.Length)
        {
            var currentLine = lines[lineIndex++];

            if (string.IsNullOrWhiteSpace(currentLine))
            {
                isInHeader = false;
                continue;
            }

            if (isInHeader)
            {
                Headers.Add(new Header(currentLine));
            }
            else
            {
                bodyBuilder.AppendLine(currentLine);
            }
        }

        if (Headers.Any(x => x.Name == HttpConstants.RequestCookie))
        {
            var cookiesAsString = Headers.FirstOrDefault(x => x.Name == HttpConstants.RequestCookie).Value;
            var cookies = cookiesAsString.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var cookie in cookies)
            {
                Cookies.Add(new Cookie(cookie));
            }
        }


        this.Body = bodyBuilder.ToString();
    }

    public string Path { get; set; }
    public HttpMethod Method { get; set; }
    public ICollection<Header> Headers { get; set; }
    public ICollection<Cookie> Cookies { get; set; }
    public string Body { get; set; }
}
