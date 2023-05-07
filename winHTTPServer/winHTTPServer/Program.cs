using System;
using System.IO;
using System.Net;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string rD = Directory.GetCurrentDirectory();
        HttpListener litnsr = new HttpListener();
        litnsr.Prefixes.Add("http://+:8080/");
        litnsr.Start();

        Console.WriteLine($"Web server running at http://+:8080/ serving files from {rD}");

        while (true)
        {
            HttpListenerContext cxt = litnsr.GetContext();
            Console.WriteLine($"Client connected from {cxt.Request.RemoteEndPoint.Address}");
            string requestPath = cxt.Request.Url.LocalPath;
            if (requestPath == "/")
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<html><body><h1>Directory listing</h1><ul>");
                foreach (string file in Directory.GetFiles(rD))
                {
                    string fileName = Path.GetFileName(file);
                    sb.Append($"<li><a href=\"{fileName}\">{fileName}</a></li>");
                }
                sb.Append("</ul></body></html>");
                byte[] buffer = Encoding.UTF8.GetBytes(sb.ToString());

                HttpListenerResponse response = cxt.Response;
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.Close();
            }
            else
            {
                string filePath = Path.Combine(rD, requestPath.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    byte[] buffer = File.ReadAllBytes(filePath);

                    HttpListenerResponse response = cxt.Response;
                    response.ContentLength64 = buffer.Length;
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                    response.Close();

                }
                else
                {
                    cxt.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    cxt.Response.Close();
                }
            }
        }
    }
}

