using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace loadtest
{
    class Program
    {
        public delegate void Del(int i);
        //public static int Intervals = 1000;
        //public static int ThreadNumber = 0;

        static void Main(string[] args)
        {
            var proceed = false;
            var threads = 1;
            var intervals = 1;

            while (!proceed)
            {
                Console.WriteLine("INPUT: # of threads/Interval");
                if (!int.TryParse(Console.ReadLine(), out threads))
                {
                    Console.WriteLine("Not a number/Int");
                }
                else proceed = true;
            }
            proceed = false;
            while (!proceed)
            {
                Console.WriteLine("INPUT: span of Intervals");
                if (!int.TryParse(Console.ReadLine(), out intervals))
                {
                    Console.WriteLine("Not a number/Int");
                }
                else proceed = true;
            }

            var runners = new Thread[threads];
            
            //del = new; 
            //var call = new Caller(5);
            //var thing = new Del();

            for (var i = 0; i < threads; i++)
            {
                runners[i] = new Thread(a => Caller.Call(intervals,i));
                runners[i].Start();
            }
            Console.ReadKey();
        }
    }
    class Caller
    {
        public static void Call(int timeout, int threadNo)
        {
            try
            {
                while (true)
                {
                    var a = System.Diagnostics.Stopwatch.StartNew();
                    GET("https://sbfacade.pt4sb.betsson.local/MarketPromotionsWebApi/api/quicklinks?format=json ");
                    a.Stop();
                    
                    Console.WriteLine("Call Success" + threadNo + " " + a.Elapsed);
                    Thread.Sleep(timeout);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Call failed");
            }
        }

        //public static void Call()
        //{
        //    Call(Program.Intervals);
        //}

        static string GET(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream, Encoding.UTF8);
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
        
    }
}
