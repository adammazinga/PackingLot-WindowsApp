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
using System.IO;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PackingLot1._2.Model;

namespace PackingLot1._2
{
    public partial class SignUp : PhoneApplicationPage
    {
        string gender = "";

        IsolatedStorageFile ISOFile = IsolatedStorageFile.GetUserStoreForApplication();
        List<UserData> ObjUserDataList = new List<UserData>();
        public SignUp()
        {
            InitializeComponent();
            this.Loaded += SignUp_Loaded;
            GenMale.Checked += Gender_Checked;
            GenFeMale.Checked += Gender_Checked;
        }
        public void Gender_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rtn = (RadioButton)sender;
            gender = rtn.Content.ToString();
        }

        private void SignUp_Loaded(object sender, RoutedEventArgs e)
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

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //UserName Validation
            if (!Regex.IsMatch(TxtUserName.Text.Trim(), @"^[A-Za-z_][a-zA-Z0-9_\s]*$"))
            {
                MessageBox.Show("Invalid UserName");
            }

            //Password length Validation
            else if (TxtPwd.Password.Length < 6)
            {
                MessageBox.Show("Password length should be minimum of 6 characters!");
            }

            //Gender Selection Empty Validation
            else if (gender == "")
            {
                MessageBox.Show("Please select gender!");
            }

            //Phone Number Length Validation
            else if (TxtPhNo.Text.Length != 10)
            {
                MessageBox.Show("Invalid Phone Number");
            }

            //Email Address validation
            else if (!Regex.IsMatch(TxtEmail.Text.Trim(), @"^([a-zA-Z_])([a-zA-Z0-9_\-\.]*)@(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}|((([a-zA-Z0-9\-]+)\.)+))([a-zA-Z]{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])$"))
            {
                MessageBox.Show("Invalid Email Address");
            }
            //After validation success ,store user detials in isolated storage
            else if (TxtUserName.Text != "" && TxtPwd.Password != "" && gender != "" && TxtPhNo.Text != "" && TxtEmail.Text != "")
            {
                UserData ObjUserData = new UserData();
                ObjUserData.UserName = TxtUserName.Text;
                ObjUserData.Password = TxtPwd.Password;
                ObjUserData.Gender = gender;
                ObjUserData.PhoneNo = TxtPhNo.Text;
                ObjUserData.Email = TxtEmail.Text;
                int Temp = 0;
                foreach (var UserLogin in ObjUserDataList)
                {
                    if (ObjUserData.UserName == UserLogin.UserName)
                    {
                        Temp = 1;
                    }
                }
                //Checking existing user names in local DB
                if (Temp == 0)
                {
                    ObjUserDataList.Add(ObjUserData);
                    if (ISOFile.FileExists("RegistrationDetails"))
                    {
                        ISOFile.DeleteFile("RegistrationDetails");
                    }
                    using (IsolatedStorageFileStream fileStream = ISOFile.OpenFile("RegistrationDetails", FileMode.Create))
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(List<UserData>));

                        serializer.WriteObject(fileStream, ObjUserDataList);

                    }
                    MessageBox.Show("Congrats! your have successfully Registered.");
                    NavigationService.Navigate(new Uri("/SignIn.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Sorry! username already exists.");
                }
            }
            else
            {
                MessageBox.Show("Please enter all details");
            }
        }
    }
}