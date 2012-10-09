using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace loadTestV2
{
    class LoadTestV2
    {
        public delegate void Del(int i);
        //public static int Intervals = 1000;
        //public static int ThreadNumber = 0;
        public static System.Timers.Timer LoadTime;
        public static int x = 0;
        
        public static void TimeElapsed(object sender, EventArgs e,int limit, Thread[] th)
        {
            x += 1000;
            if (x >= limit+2000)
            {
                LoadTime.Enabled = false;
                foreach (var thread in th)
                {
                    thread.Abort();
                }
                Console.WriteLine("END---------------------");
                Console.WriteLine("total calls" + totalCalls);
                Console.WriteLine("total errors" +errors);
                Console.ReadKey();
            }
        }

        static void Main(string[] args)
        {
            var proceed = false;
            var threads = 1;
            var timer = 1;
            LoadTime = new Timer {Interval = 1000};
            

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
                Console.WriteLine("INPUT: timer span in seconds");
                if (!int.TryParse(Console.ReadLine(), out timer))
                {
                    Console.WriteLine("Not a number/Int");
                }
                else
                {
                    proceed = true;
                    timer *= 1000;
                }
            }

            
            var runners = new Thread[threads];
            LoadTime.Elapsed += new ElapsedEventHandler(delegate { TimeElapsed(new object(), new EventArgs(), timer, runners); });
            //del = new; 
            //var call = new Caller(5);
            //var thing = new Del();

            LoadTime.Enabled = true;

            for (var i = 0; i < threads; i++)
            {
                runners[i] = new Thread(a => Call(timer, i));
                runners[i].Start();
            }



            Console.ReadKey();
        }

        #region startcalls
        public static int totalCalls = 0;
        public static int errors = 0;

        public static void Call(int timeout, int threadNo)
        {
            try
            {
                while (x < timeout)//x < 60000)
                {
                    var a = System.Diagnostics.Stopwatch.StartNew();
                    Caller.GET("https://sbfacade.pt4sb.betsson.local/MarketPromotionsWebApi/api/quicklinks?format=json ");
                    a.Stop();
                    totalCalls++;

                    Console.WriteLine("Call Success" + threadNo + " " + a.Elapsed.TotalSeconds.ToString());
                }
                
            }
            catch (Exception ex)
            {
                errors++;
                Console.WriteLine("Call failed" + ex.Message );
            }
            Thread.EndThreadAffinity();
        }
        #endregion
    }

    class Caller
    {
        //public static void Call()
        //{
        //    Call(Program.Intervals);
        //}

        public static string GET(string url)
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
