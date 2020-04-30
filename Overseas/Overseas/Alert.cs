using System.Windows.Forms;

namespace Overseas
{
    class Alert
    {
        public static void showAlert(string message, string title)
        {
            MessageBox.Show(title, message);

        }
    }
}
