using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.ComponentModel;

namespace F9S1.RememberMe
{
    /// <summary>
    /// 
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        public delegate void timeCheck();
        Controller dispatch;
        List<Task> taskInfo;
        List<string> taskDetails;
        string lastInput;
        const string C_ERROR = "error";
        const string C_PRINT = "print";
        const string C_TASK = "task";
        const string C_TASKLIST = "tasklist";
        const int SIZE_OF_DESCRIPTION = 11;
        const int SIZE_OF_TASKNAME = 8;
        const int SIZE_OF_DEADLINE = 8;
        const int SIZE_OF_PRIORITY = 8;
        const int SIZE_OF_LABEL = 5;
        const string ADD_COMMAND = "add";
        const string EDIT_COMMAND = "edit";
        const string SORT_COMMAND = "sort";
        const string DELETE_COMMAND = "delete";
        const string SORT_PRIORITY = "priority";
        const string DEADLINE = "deadline";
        const string ARCHIVE_COMMAND = "archive";
        const string CLEAR_COMMAND = "clear";

        int numberBackSpace = 0;
        string[] userPrompts = { "", "taskname", "description", "deadline", "priority", "label" };
        int[] lengthofPrompts = { 0, SIZE_OF_TASKNAME, SIZE_OF_DESCRIPTION, SIZE_OF_DEADLINE, SIZE_OF_PRIORITY, SIZE_OF_PRIORITY };
        public MainWindow()
        {
            initialiseNotificationIcon();
            dispatch = new Controller();
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                dispatch.Log(e.StackTrace);
            }
            inputBox.Focus(); 
            SetDisplay();
            this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new timeCheck(setAlarm));
        }
        void initialiseNotificationIcon()
        {
            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.BalloonTipText = "Remember Me has been minimised. Click the tray icon to show.";
            m_notifyIcon.BalloonTipTitle = "Remember Me";
            m_notifyIcon.Text = "Remember Me";
            m_notifyIcon.Icon = new System.Drawing.Icon("AddedIcon.ico");
            m_notifyIcon.Click += new EventHandler(m_notifyIcon_Click);
        }
        private void OnClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_notifyIcon.Dispose();
            m_notifyIcon = null;
        }

        private WindowState m_storedWindowState = WindowState.Normal;
        private void OnStateChanged(object sender, System.EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                if (m_notifyIcon != null)
                    m_notifyIcon.ShowBalloonTip(2000);
            }
            else
                m_storedWindowState = WindowState;
        }
        void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon();
        }

        void m_notifyIcon_Click(object sender, System.EventArgs e)
        {
            Show();
            WindowState = m_storedWindowState;
        }
        void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }

        void ShowTrayIcon(bool show)
        {
            if (m_notifyIcon != null)
                m_notifyIcon.Visible = show;
        }
        void maximiseWindow()
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Show();
            }
        }
        void updateDeadline(ref List<Task> taskList, int[] time, int i)
        {
            TimeSpan difference = new TimeSpan(time[0], time[1], time[2], 0);
            DateTime updatedDate = DateTime.Now.Add(difference);
            taskList[i].Deadline = updatedDate;
        }
        public void setAlarm()
        {
            taskInfo = dispatch.GetTasks();
            bool isLabelNotArchive;
            bool isDeadlineReached;

            for (int i = 0; i < taskInfo.Count; i++)
            {
                isLabelNotArchive = !taskInfo[i].IsArchived;
                isDeadlineReached = taskInfo[i].Deadline.CompareTo(DateTime.Now) < 0;
                if (isLabelNotArchive && isDeadlineReached)
                {
                    int[] time = new int[3];
                    Alarm showAlarm = new Alarm(taskInfo[i].Details, taskInfo[i].Deadline);
                    maximiseWindow();
                    showAlarm.ShowDialog();
                    time = showAlarm.getTimeArray();
                    if (time == null)
                    {
                        taskInfo[i].IsArchived = true;
                    }
                    else
                    {
                        updateDeadline(ref taskInfo, time, i);
                    }
                    dispatch.WriteToFile(taskInfo);
                    SetDisplay();
                }
            }
            this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new timeCheck(setAlarm));
        }
        private bool checkIfZero(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != 0)
                    return false;
            }
            return true;
        }
        private int numberOfSemiColon(String text)
        {
            int count = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ';')
                {
                    count++;
                }
            }
            return count;
        }
        private void AutoComplete(String input)
        {
            if (input.Equals(""))
                return;
            else if (input.Length < ARCHIVE_COMMAND.Length && input.StartsWith("ar") && input.Equals(ARCHIVE_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, ARCHIVE_COMMAND);
            else if (input.Length < ADD_COMMAND.Length && input.StartsWith("a") && input.Equals(ADD_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, ADD_COMMAND);
            else if (input.Length < EDIT_COMMAND.Length && input.StartsWith("e") && input.Equals(EDIT_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, EDIT_COMMAND);
            else if (input.Length < SORT_COMMAND.Length && input.StartsWith("s") && input.Equals(SORT_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, SORT_COMMAND);
            else if (input.Length < DELETE_COMMAND.Length && input.StartsWith("d") && input.Equals(DELETE_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, DELETE_COMMAND);
            else if (input.Length < CLEAR_COMMAND.Length && input.StartsWith("c") && input.Equals(CLEAR_COMMAND.Substring(0, input.Length)))
                AutoComplete(input, CLEAR_COMMAND);
        }
        private void AutoComplete(String input, String keyWord)
        {
            int start = inputBox.Text.Length;
            inputBox.Text += keyWord.Substring(input.Length, keyWord.Length - input.Length);
            inputBox.Select(start, keyWord.Length - input.Length);
        }

        private void sortAutoComplete(String input)
        {
            if (input.Length < DEADLINE.Length && input.StartsWith("d") && input.Equals(DEADLINE.Substring(0, input.Length)))
                AutoComplete(input, DEADLINE);
            else if (input.Length < SORT_PRIORITY.Length && input.StartsWith("p") && input.Equals(SORT_PRIORITY.Substring(0, input.Length)))
                AutoComplete(input, SORT_PRIORITY);
        }
        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string passToRelegator = inputBox.Text;
            if (inputBox.Text != "" && inputBox.Text.Length > 5 && passToRelegator.Substring(0, 5) == "find;")
            {
                string searchString = passToRelegator.Substring(5, passToRelegator.Length - 5);
                SetOutputBox(dispatch.CallSearch(searchString));
                return;
            }
            if (Keyboard.IsKeyDown(Key.Back))
                numberBackSpace = 1;
            if (numberBackSpace == 0 && inputBox.Text.StartsWith(SORT_COMMAND + ';') && !inputBox.Text.EndsWith(";"))
                sortAutoComplete(inputBox.Text.Substring(inputBox.Text.IndexOf(';') + 1));
            numberBackSpace = 0;
            if (Keyboard.IsKeyDown(Key.Back))
                numberBackSpace = 1;
            if (numberBackSpace == 0 && !inputBox.Text.Contains(' ') && !inputBox.Text.Contains(";"))
                AutoComplete(inputBox.Text);
            numberBackSpace = 0;

            string getCommand = "";
            if (inputBox.Text.Contains(";"))
            {

                getCommand = inputBox.Text.Substring(0, inputBox.Text.IndexOf(';')).ToLower().Trim();
                int count = numberOfSemiColon(inputBox.Text);
                if (inputBox.Text[inputBox.Text.Length - 1] == ';' && (getCommand.Equals("add") || getCommand.Equals("edit:")) && numberBackSpace == 0)
                {
                    if (Keyboard.IsKeyDown(Key.Back))
                        numberBackSpace = 1;
                    if (numberBackSpace == 0)
                    {
                        int countSemiColon = numberOfSemiColon(inputBox.Text);

                        if (countSemiColon < 6) //does this reduce the readablity of the code? - inian
                        {
                            inputBox.Text += userPrompts[countSemiColon];
                            inputBox.Select(inputBox.Text.LastIndexOf(';') + 1, lengthofPrompts[countSemiColon]);
                        }
                    }

                }
            }
            numberBackSpace = 0;
            if (getCommand == "delete" || getCommand == "edit" || getCommand == "archive")
            {
                int posOfSemi = inputBox.Text.IndexOf(";");
                string wordToSearch = "";
                if (posOfSemi != -1)
                {
                    for (int i = posOfSemi + 1; i < inputBox.Text.Length && inputBox.Text[i] != ';'; i++)
                    {
                        wordToSearch += inputBox.Text[i];
                        List<string> toBeDisplay = dispatch.CallSearch(wordToSearch);
                        outputBox.Items.Clear();
                        for (int j = 0; j < toBeDisplay.Count; j++)
                        {
                            outputBox.Items.Add(toBeDisplay[j]);
                        }
                    }
                }
            }


            if (inputBox.Text.Length <= getCommand.Length + 1)
                SetDisplay();
        }
        private void SetDisplay()
        {
            outputBox.Items.Clear();
            taskDetails = dispatch.UserDispatch("display");
            for (int i = 0; i < taskDetails.Count; i++)
            {
                outputBox.Items.Add(taskDetails[i]);
            }
        }

        private void setErrorLabel(string errorMessage)
        {
            displayBox.Content = errorMessage;
        }

        private void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (inputBox.Text.Trim() == "")
                {
                    SetOutputBox(dispatch.UserDispatch("display"));
                    dispatch.Status = Controller.State.normal;
                    displayBox.Content = "";
                }
                else
                {
                    string input = inputBox.Text.ToString();
                    lastInput = input;
                    List<string> output = new List<string>(dispatch.UserDispatch(input));
                    inputBox.Clear();
                    displayBox.Content = "";
                    if (output[0] == Utility.ERROR)
                    {
                        inputBox.Text = input;
                        displayBox.Content = input + "\n" + output[1];
                    }
                    SetOutputBox(dispatch.UserDispatch("display"));
                }
            }
            else if (e.Key == Key.Escape)
            {
                inputBox.Clear();
                dispatch.Status = Controller.State.normal;
                displayBox.Content = "";
            }
            if (inputBox.Text != "" && e.Key == Key.Tab)
            { // List<string> taskDetails = directFunction.displayTasks();
                string words = inputBox.Text.Substring(inputBox.Text.LastIndexOf(';') + 1);
                string temp = inputBox.Text.Substring(0, inputBox.Text.LastIndexOf(';') + 1);
                if (temp == "delete;" || temp == "edit;" || temp == "archive;")
                {
                    List<string> toBeDisplay = dispatch.CallSearch(words);
                    // while (toBeDisplay.Count != 0)

                    if (toBeDisplay.Count != 0)
                    {
                        inputBox.Text = temp + autoCompleteSearch(words)[0].Split(';')[0];
                        inputBox.Select(inputBox.Text.Length, 0);
                    }
                    else
                        inputBox.Text = "";
                }
            }
            if (e.Key == Key.Escape)
            {
                if (displayBox.Content.ToString() == "edit: ")
                    dispatch.UserDispatch("");
                inputBox.Text = "";
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        private void undoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string displayText = "";
            dispatch.UserDispatch("undo;");
            inputBox.Text = displayText;
            displayBox.Content = "Action undone";
            SetDisplay();
        }

        private void redoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            string displayText = "";
            dispatch.UserDispatch("redo;");
            inputBox.Text = displayText;
            displayBox.Content = "Action redone";
            SetDisplay();
        }
        public List<string> autoCompleteSearch(string input)
        {
            List<Task> contents = dispatch.GetTasks();
            List<string> keywords = new List<string>(input.Split(' '));
            for (int i = 0; i < keywords.Count; i++)
                if (keywords[i].Equals(""))
                    keywords.RemoveAt(i);
            List<int> hitcount = new List<int>();
            for (int i = 0; i < contents.Count; i++)
                hitcount.Add(0);
            int maxhits = 0;
            for (int i = 0; i < contents.Count; i++)
            {
                for (int j = 0; j < keywords.Count; j++)
                {
                    if (contents[i].Details.Contains(keywords[j]))
                    {
                        hitcount[i]++;

                        if (hitcount[i] > maxhits)
                            maxhits = hitcount[i];
                    }
                }
            }
            List<string> temp = new List<string>();
            for (int i = maxhits; i > 0; i--)
            {
                for (int j = 0; j < contents.Count; j++)
                    if (hitcount[j] == i)
                        temp.Add(contents[j].ToString());
            }
            return temp;
        }


        private void textBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                inputBox.Text = lastInput;
                inputBox.SelectionStart = inputBox.Text.Length;
            }
            else if (e.Key == Key.Down)
                displayBox.Focus();
        }

        private void SetOutputBox(List<string> output)
        {
            outputBox.Items.Clear();
            for (int i = 0; i < output.Count; i++)
                outputBox.Items.Add(output[i]);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            inputBox.Focus();
        }
    }
    //public partial class MainWindow : Window
    //{
    //    Controller dispatch;

    //    public MainWindow()
    //    {
    //        InitializeComponent();
    //        dispatch = new Controller();
    //        SetOutputBox(dispatch.userDispatch("display"));
    //        inputBox.Focus();
    //    }

    //    private void Window_Loaded(object sender, RoutedEventArgs e)
    //    {
    //        inputBox.Focus();
    //    }

    //    private void SetOutputBox(List<string> output)
    //    {
    //        outputBox.Items.Clear();
    //        for (int i = 0; i < output.Count; i++)
    //            outputBox.Items.Add(output[i]);
    //    }

    //    private void inputBox_KeyDown(object sender, KeyEventArgs e)
    //    {
    //        if (e.Key == Key.Enter)
    //        {
    //            if (inputBox.Text.Trim() == "")
    //            {
    //                SetOutputBox(dispatch.userDispatch("display"));
    //                dispatch.Status = Controller.State.normal;
    //                displayBox.Content = "";
    //            }
    //            else
    //            {
    //                string input = inputBox.Text.ToString();
    //                List<string> output = new List<string>(dispatch.userDispatch(input));
    //                inputBox.Clear();
    //                displayBox.Content = "";
    //                if (output[0] == Utility.ERROR)
    //                {
    //                    inputBox.Text = input;
    //                    displayBox.Content = input + "\n" + output[1];
    //                }
    //                SetOutputBox(dispatch.userDispatch("display"));
    //            }
    //        }
    //        else if (e.Key == Key.Escape)
    //        {
    //            inputBox.Clear();
    //            dispatch.Status = Controller.State.normal;
    //            displayBox.Content = "";
    //        }
    //    }
    }

