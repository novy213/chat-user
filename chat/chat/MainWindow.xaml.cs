using chat.Models;
using chat.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User UserWithChat;
        Thread refreshTherad;
        public MainWindow()
        {
            InitializeComponent();
            if (Settings.Default.accessToken != "") SetAppStateLogin();
            else SetAppStateLogout();
        }
        public async void SetAppStateLogin()
        {
            refreshTherad = new Thread(RefreshApp);
            refreshTherad.Start();
            LoginGrid.Visibility = Visibility.Collapsed;
            UsersGrid.Visibility = Visibility.Visible;
            GetUsersResponse res = await Api.GetUsersAsync();
            UsersListView.ItemsSource = res.Users;
        }
        public void SetAppStateLogout()
        {
            LoginGrid.Visibility = Visibility.Visible;
            UsersGrid.Visibility = Visibility.Collapsed;
            TerminateThread(refreshTherad);
        }
        private async void Login_click(object sender, RoutedEventArgs e)
        {
            LoginResponse res = await Api.LoginAsync(LoginName.Text, PasswordName.Text);
            if (res.Error) MessageBox.Show(res.Message, "Error", MessageBoxButton.OK);
            else SetAppStateLogin();
        }
        private void GoToRegister_click(object sender, RoutedEventArgs e)
        {
            RegisterGrid.Visibility = Visibility.Visible;
            LoginGrid.Visibility = Visibility.Collapsed;
        }
        private async void Register_click(object sender, RoutedEventArgs e)
        {
            string login = LoginRegister.Text;
            string password = PasswordRegister.Text;
            string name = NameRegister.Text;
            string last_name = LNameRegister.Text;
            if (login == "" || password == "" || name == "" || last_name == "")
            {
                MessageBox.Show("All fields are required", "Error");
            }
            else
            {
                APIResponse res = await Api.RegisterAsync(login, password, name, last_name);
                if (res.Error) MessageBox.Show(res.Message, "Error", MessageBoxButton.OK);
                else
                {
                    RegisterGrid.Visibility = Visibility.Collapsed;
                    LoginGrid.Visibility = Visibility.Visible;
                }
            }
        }
        private void BackRegister_click(object sender, RoutedEventArgs e)
        {
            RegisterGrid.Visibility = Visibility.Collapsed;
            LoginGrid.Visibility = Visibility.Visible;
        }

        private async void Logout_click(object sender, RoutedEventArgs e)
        {
            APIResponse res = await Api.LogoutAsync();
            if (res.Error) MessageBox.Show(res.Message, "Error", MessageBoxButton.OK);
            else SetAppStateLogout();
        }

        private async void UsersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                /*User selectedItem = e.AddedItems[0] as User;
                ReciveMessageResponse res = await Api.ReciveMessageAsync(selectedItem.Id);
                UsersGrid.Visibility = Visibility.Collapsed;
                ChatGrid.Visibility = Visibility.Visible;
                ChatListView.ItemsSource = res.Messages;
                UserWithChat = selectedItem;  */              
            }
        }

        private async void BackUsersChat_click(object sender, RoutedEventArgs e)
        {
            ChatGrid.Visibility = Visibility.Collapsed;
            UsersGrid.Visibility = Visibility.Visible;
            UserWithChat = null;
            ChatListView.ItemsSource = null;
            GetUsersResponse res = await Api.GetUsersAsync();
            UsersListView.ItemsSource = res.Users;
            MessageSend.Text = "";
        }

        private async void SendButton_click(object sender, RoutedEventArgs e)
        {
            APIResponse res = await Api.SendMessageAsync(UserWithChat.Id, MessageSend.Text);
            ReciveMessageResponse res2 = await Api.ReciveMessageAsync(UserWithChat.Id);
            MessageSend.Text = "";
            ChatListView.ItemsSource = res2.Messages;
            ChatListView.SelectedIndex = ChatListView.Items.Count - 1;
            ChatListView.ScrollIntoView(ChatListView.SelectedItem);
        }

        private async void CreateChat_click(object sender, RoutedEventArgs e)
        {
            CreateNewChat createNewChat = new CreateNewChat { Owner = this };
            GetUsersResponse res = await Api.GetAllUsersAsync();
            createNewChat.UsersListView.ItemsSource = res.Users;
            if (createNewChat.ShowDialog() == true)
            {     
                UserWithChat = createNewChat.UsersListView.SelectedItem as User;
                if(UserWithChat != null)
                {
                    UsersGrid.Visibility = Visibility.Collapsed;
                    ChatGrid.Visibility = Visibility.Visible;
                    ReciveMessageResponse resp = await Api.ReciveMessageAsync(UserWithChat.Id);
                    ChatListView.ItemsSource = resp.Messages;
                }
            }
        }
        private void RefreshApp()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    _ = Dispatcher.BeginInvoke(new Action(async () =>
                    {
                        if (UserWithChat != null)
                        {
                            ReciveMessageResponse resp = await Api.ReciveMessageAsync(UserWithChat.Id);
                            ChatListView.ItemsSource = resp.Messages;
                        }
                        GetUsersResponse res = await Api.GetUsersAsync();
                        UsersListView.ItemsSource = res.Users;
                    }));
                }
            }
            catch (ThreadInterruptedException)
            {
                Debug.WriteLine("Exiting Refresh thread");
                return;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            TerminateThread(refreshTherad);
        }
        private void TerminateThread(Thread thread)
        {
            if (thread != null)
            {
                thread.Interrupt();
                thread.Join();
            }
        }

        private async void ListViewItem_Click(object sender, MouseButtonEventArgs e)
        {
            var clickedItem = (ListViewItem)sender;
            var data = clickedItem.Content;
            User selectedItem = data as User;
            ReciveMessageResponse res = await Api.ReciveMessageAsync(selectedItem.Id);
            UsersGrid.Visibility = Visibility.Collapsed;
            ChatGrid.Visibility = Visibility.Visible;
            ChatListView.ItemsSource = res.Messages;
            UserWithChat = selectedItem;
            ChatListView.SelectedIndex = ChatListView.Items.Count - 1;
            ChatListView.ScrollIntoView(ChatListView.SelectedItem);
        }
    }
}
