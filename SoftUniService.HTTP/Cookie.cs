﻿namespace SoftUniService.HTTP;

public class Cookie
{
    public Cookie(string cookie)
    {
        var cookieParts = cookie.Split(new char[] { '=' }, 2);

        this.Name = cookieParts[0];
        this.Value = cookieParts[1];
    }

    public Cookie(string name, string value)
    {
        this.Name = name;
        this.Value = value;
    }

    public string Name { get; set; }
    public string Value { get; set; }

    public override string ToString()
    {
        return $"{this.Name}={this.Value}";
    }
}
