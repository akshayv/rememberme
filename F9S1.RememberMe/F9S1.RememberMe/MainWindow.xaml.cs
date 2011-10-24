using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace F9S1.RememberMe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller dispatch;

        public MainWindow()
        {
            InitializeComponent();
            dispatch = new Controller();
            SetOutputBox(dispatch.userDispatch("display"));
            inputBox.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            inputBox.Focus();
        }

        private void SetOutputBox(List<string> output)
        {
            outputBox.Items.Clear();
            for (int i = 0; i < output.Count; i++)
                outputBox.Items.Add(output[i]);
        }

        private void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (inputBox.Text.Trim() == "")
                {
                    SetOutputBox(dispatch.userDispatch("display"));
                    dispatch.Status = Controller.State.normal;
                    displayBox.Content = "";
                }
                else
                {
                    string input = inputBox.Text.ToString();
                    List<string> output = new List<string>(dispatch.userDispatch(input));
                    inputBox.Clear();
                    displayBox.Content = "";
                    if (output[0] == Utility.ERROR)
                    {
                        inputBox.Text = input;
                        displayBox.Content = input + "\n" + output[1];
                    }
                    SetOutputBox(dispatch.userDispatch("display"));
                }
            }
            else if (e.Key == Key.Escape)
            {
                inputBox.Clear();
                dispatch.Status = Controller.State.normal;
                displayBox.Content = "";
            }
        }
    }
}
