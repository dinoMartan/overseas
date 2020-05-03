using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Overseas
{
    public partial class Validation : Form
    {
        public string username;
        public string password;

        public Validation()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            username = usernameText.Text;
            password = passwordText.Text;

            if(username == "" || password == "")
            {
                Alert.showAlert("Greška", "Potrebno je upisati korisničko ime i lozinku!");
            }
            else
            {
                this.Close();
            }


        }
    }
}
