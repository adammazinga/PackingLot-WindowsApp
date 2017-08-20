using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using PackingLot1._2.Model;

namespace PackingLot1._2
{
    public partial class SignIn : PhoneApplicationPage
    {
        IsolatedStorageFile ISOFile = IsolatedStorageFile.GetUserStoreForApplication();
        List<UserData> ObjUserDataList = new List<UserData>();
        public SignIn()
        {
            InitializeComponent();
            this.Loaded += SignIn_Loaded;
        }
        private void SignIn_Loaded(object sender, RoutedEventArgs e)
        {
            var Settings = IsolatedStorageSettings.ApplicationSettings;
            //Check if user already login,so we need to direclty navigate to details page instead of showing login page when user launch the app.
            if (Settings.Contains("CheckLogin"))
            {
                NavigationService.Navigate(new Uri("/Views/UserDetails.xaml", UriKind.Relative));
            }
            else
            {
                if (ISOFile.FileExists("RegistrationDetails"))//loaded previous items into list
                {
                    using (IsolatedStorageFileStream fileStream = ISOFile.OpenFile("RegistrationDetails", FileMode.Open))
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(List<UserData>));
                        ObjUserDataList = (List<UserData>)serializer.ReadObject(fileStream);

                    }
                }
            }

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (UserName.Text != "" && PassWord.Password != "")
            {
                int Temp = 0;
                foreach (var UserLogin in ObjUserDataList)
                {
                    if (UserName.Text == UserLogin.UserName && PassWord.Password == UserLogin.Password)
                    {
                        Temp = 1;
                        var Settings = IsolatedStorageSettings.ApplicationSettings;
                        Settings["CheckLogin"] = "Login sucess";// store some temporery value when user successfully login with his details, so on next app launch we need to check 'CheckLogin' variable value. If it is having value means user already logined.

                        if (ISOFile.FileExists("CurrentLoginUserDetails"))
                        {
                            ISOFile.DeleteFile("CurrentLoginUserDetails");
                        }
                        using (IsolatedStorageFileStream fileStream = ISOFile.OpenFile("CurrentLoginUserDetails", FileMode.Create))
                        {
                            DataContractSerializer serializer = new DataContractSerializer(typeof(UserData));

                            serializer.WriteObject(fileStream, UserLogin);

                        }
                        // NavigationService.Navigate(new Uri("/Dashboard.xaml", UriKind.Relative));
                        NavigationService.Navigate(new Uri("/Dashboard.xaml", UriKind.Relative));
                    }
                }
                if (Temp == 0)
                {
                    MessageBox.Show("Invalid UserID/Password");
                    //NavigationService.Navigate(new Uri("/SignUp.xaml", UriKind.Relative));
                }
            }
        }

 
    }
}