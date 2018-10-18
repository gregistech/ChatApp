using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace AndroidChatApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        void Send(string ip, string port)
        {
            var client = new TcpClient();
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
                client.Connect(ipEndPoint);
            if (client.Connected)
            {
                var reader = new StreamReader(client.GetStream());
                var writer = new StreamWriter(client.GetStream());
                writer.AutoFlush = true;

                if (client.Connected)
                {
                    writer.WriteLine("&&@@@///thegergo02phone&&@@@///");
                    Toast.MakeText(this, "Connected to " + ip + ":" + port, ToastLength.Short).Show();
                }
            }
        }

        Button button;
        EditText ip;
        EditText port;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            button = FindViewById<Button>(Resource.Id.connect);
            ip = FindViewById<EditText>(Resource.Id.ip);
            port = FindViewById<EditText>(Resource.Id.port);
            button.Click += delegate {
                string ipstr = ip.Text;
                string portstr = port.Text;
                Toast.MakeText(this, "Connecting to " + ipstr + ":" + portstr + "...", ToastLength.Short).Show();
                Send(ipstr,portstr);
            };
        }
    }
}