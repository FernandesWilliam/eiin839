using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

namespace BasicServerHTTPlistener
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            //if HttpListener is not supported by the Framework
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("A more recent Windows version is required to use the HttpListener class.");
                return;
            }
 
 
            // Create a listener.
            HttpListener listener = new HttpListener();

            // Add the prefixes.
            if (args.Length != 0)
            {
                foreach (string s in args)
                {
                    listener.Prefixes.Add(s);
                    // don't forget to authorize access to the TCP/IP addresses localhost:xxxx and localhost:yyyy 
                    // with netsh http add urlacl url=http://localhost:xxxx/ user="Tout le monde"
                    // and netsh http add urlacl url=http://localhost:yyyy/ user="Tout le monde"
                    // user="Tout le monde" is language dependent, use user=Everyone in english 

                }
            }
            else
            {
                Console.WriteLine("Syntax error: the call must contain at least one web server url as argument");
            }
            listener.Start();

            // get args 
            foreach (string s in args)
            {
                Console.WriteLine("Listening for connections on " + s);
            }

            // Trap Ctrl-C on console to exit 
            Console.CancelKeyPress += delegate {
                // call methods to close socket and exit
                listener.Stop();
                listener.Close();
                Environment.Exit(0);
            };


            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                string documentContents;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }
                
                // get url 
                Console.WriteLine($"Received request for {request.Url}");

                //get url protocol
                Console.WriteLine(request.Url.Scheme);
                //get user in url
                Console.WriteLine(request.Url.UserInfo);
                //get host in url
                Console.WriteLine(request.Url.Host);
                //get port in url
                Console.WriteLine(request.Url.Port);
                //get path in url 
                Console.WriteLine(request.Url.LocalPath);
                Type type = typeof(MyReflectionClass);
                var methodsName = type.GetMethods();
                // parse path in url 
                string responseString;
                foreach (string str in request.Url.Segments)
                {
                    Console.WriteLine(str);
              
                    if(methodsName.Any(methodn => methodn.Name == str))
                    {
                        MethodInfo method = type.GetMethod(str);
                        MyReflectionClass c = new MyReflectionClass();
                        var param1 = HttpUtility.ParseQueryString(request.Url.Query).Get("param1");
                        var param2 = HttpUtility.ParseQueryString(request.Url.Query).Get("param2");
                        responseString = (string) method.Invoke(c,new object[]{param1,param2});
                        
                        HttpListenerResponse response = context.Response;
                        // Construct a response.
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        // Get a response stream and write the response to it.
                        response.ContentLength64 = buffer.Length;
                        System.IO.Stream output = response.OutputStream;
                        output.Write(buffer, 0, buffer.Length);
                        // You must close the output stream.
                        output.Close();
                    }
                }

                //get params un url. After ? and between &

                Console.WriteLine(request.Url.Query);
                //
                Console.WriteLine(documentContents);
       
                
             
            }
            // Httplistener neither stop ... But Ctrl-C do that ...
            // listener.Stop();
        }
    }
}
public class MyReflectionClass
{
    // public string callSh(string param1, string param2)
    // {
    //     ProcessStartInfo start = new ProcessStartInfo();
    //     start.FileName = "/Users/williamfernandes/Polytech/SI4/S2/WS/eiin839/TD2/BasicWebServer/launch.sh"; // Specify exe name.
    //    // start.Arguments = param1+ " "+ param2 ; // Specify arguments.
    //     start.UseShellExecute = false; 
    //     start.RedirectStandardOutput = true; 
    //     using (Process process = Process.Start(start))
    //     {
    //         // Read in all the text from the process with the StreamReader.
    //         //
    //         using (StreamReader reader = process.StandardOutput)
    //         {
    //             string result = reader.ReadToEnd();
    //             Console.WriteLine(result + "result ");
    //             return result;
    //             
    //         }
    //     }
    // }
    public string callExec(string param1, string param2)
    {
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = "/Users/williamfernandes/Polytech/SI4/S2/WS/eiin839/TD2/ExecTest/bin/Debug/ExecTest.exe"; // Specify exe name.
        start.Arguments = param1+ " "+ param2 ; // Specify arguments.
        start.UseShellExecute = false; 
        start.RedirectStandardOutput = true; 
        using (Process process = Process.Start(start))
        {
            // Read in all the text from the process with the StreamReader.
            //
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                return result;
                
            }
        }
        
    }

    public string MyMethod(string param1 , string param2)
    {
        return "<html><body> Hello " + param1 + " et " + param2 + "</body></html>";
    }
}