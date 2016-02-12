using System.Windows;
using TLSharp.Core;

namespace Tester
{
    public partial class MainWindow
    {
        private TelegramClient _client;
        private string _hash;
        private int? _me;

        public MainWindow()
        {
            InitializeComponent();
            Setup();
        }

        private async void Setup()
        {
            var store = new FileSessionStore();
            _client = new TelegramClient(store, "session");
            await _client.Connect();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            _hash = await _client.SendCodeRequest(textBox.Text);
        }

        private async void button2_Click(object sender, RoutedEventArgs e)
        {
            var user = await _client.MakeAuth(textBox.Text, _hash, textBox2.Text);
            MessageBox.Show($"Registered as {user}");
            _me = await _client.ImportByUserName(""); // FIX: <<USERNAME>>
        }

        private async void send_Click(object sender, RoutedEventArgs e)
        {
            if (_me.HasValue)
                await _client.SendMessage(_me.Value, textBox1.Text);
        }

    }
}
