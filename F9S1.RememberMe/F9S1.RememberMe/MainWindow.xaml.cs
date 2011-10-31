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
using System.IO;
using System.Diagnostics;

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
        const string ADD_COMMAND = "add";
        const string EDIT_COMMAND = "edit";
        const string SORT_COMMAND = "sort";
        const string DELETE_COMMAND = "delete";
        const string SORT_PRIORITY = "priority";
        const string DEADLINE = "deadline";
        const string ARCHIVE_COMMAND = "archive";
        const string CLEAR_COMMAND = "clear";
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        int numberBackSpace = 0;
        string[] userPrompts = { "", "details", "deadline", "label", "priority" };
        public MainWindow()
        {
            initialiseNotificationIcon();
            dispatch = new Controller();
            taskInfo = dispatch.GetTasks();
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                logger.Error(e.StackTrace);
                //dispatch.Log(e.StackTrace);
            }
            inputBox.Focus();
            // SetDisplay();
            // dataGrid1.DataContext = dispatch.GetTasks();
            SetDisplay();
           // this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new timeCheck(setAlarm));
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
     /*   public void setAlarm()
        {
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
                        //Debug.Assert(taskInfo != null);
                        updateDeadline(ref taskInfo, time, i);
                    }
                    dispatch.WriteToFile(taskInfo);
                    SetDisplay();
                }
            }
            this.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new timeCheck(setAlarm));
        }*/
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
            // Debug.Assert(text == null); typing add; causes this stupid assert to give an error so commented it out fr now
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
            //Debug.Assert(input==null);
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
        private int findNumHits(Task check, string keyword)
        {
            int hitcount = 0;
            if (check.Details.Contains(keyword))
            {
                hitcount++;
            }
            if (check.Labels.Contains(keyword))
            {
                hitcount++;
            }
            try
            {
                DateTime.Parse(keyword).ToString(Utility.DATE_FORMAT);
                if (check.Deadline.ToString(Utility.DATE_FORMAT).Contains(DateTime.Parse(keyword).ToString(Utility.DATE_FORMAT)))
                {
                    hitcount++;
                }

            }
            catch (Exception e)
            {

            }

            return hitcount;
        }
        public List<string> InstantSearch(string input, string command)
        {
            List<Task> taskList = taskInfo;
            List<string> keywords = new List<string>(input.Split(' '));
            for (int i = 0; i < keywords.Count; i++)
                if (keywords[i].Equals(""))
                    keywords.RemoveAt(i);
            List<int> hitcount = new List<int>();
            for (int i = 0; i < taskList.Count; i++)
                hitcount.Add(0);
            int maxhits = 0;
            for (int i = 0; i < taskList.Count; i++)
            {
                for (int j = 0; j < keywords.Count; j++)
                {
                    if (taskList[i].ToString().Contains(keywords[j]) && taskList[i].IsArchived == false)
                    {
                        hitcount[i]++;

                        if (hitcount[i] > maxhits)
                            maxhits = hitcount[i];
                    }
                    if ("archive".Contains(keywords[j]) && taskList[i].IsArchived == true && command == "find")
                    {
                        hitcount[i]++;

                        if (hitcount[i] > maxhits)
                            maxhits = hitcount[i];
                    }
                    if ("highpriority".Contains(keywords[j]) && taskList[i].IsStarred == true)
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
                for (int j = 0; j < taskList.Count; j++)
                    if (hitcount[j] == i)
                        temp.Add(taskList[j].GetDisplay());
            }

            return temp;
        }
        private void inputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string passToRelegator = inputBox.Text;

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

                        if (countSemiColon < 5) //does this reduce the readablity of the code? - inian
                        {
                            inputBox.Text += userPrompts[countSemiColon];
                            int semicolonIndex = inputBox.Text.LastIndexOf(';');
                            inputBox.Select(semicolonIndex + 1, inputBox.Text.Length - semicolonIndex);
                        }
                    }
                }

            }
            numberBackSpace = 0;
            if (getCommand == "delete" || getCommand == "edit" || getCommand == "archive" || getCommand == "find")
            {
                int posOfSemi = inputBox.Text.LastIndexOf(";");
                string wordToSearch = inputBox.Text.Substring(posOfSemi + 1);
                if (posOfSemi != -1)
                {
                    List<string> toBeDisplay = InstantSearch(wordToSearch, getCommand);
                    SetOutputBox(toBeDisplay);
                }
            }

            if (inputBox.Text.Length <= getCommand.Length + 1)
                SetDisplay();
        }
        private void SetDisplay()
        {
            taskDetails = dispatch.UserDispatch("display");
            SetOutputBox(taskDetails);
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
                    if (output.Count > 0 && output[0] == Utility.ERROR)
                    {
                        inputBox.Text = input;
                        displayBox.Content = input + "\n" + output[1];
                    }
                    else
                    {
                        inputBox.Text = "";
                        displayBox.Content = "";
                    }
                    SetOutputBox(dispatch.UserDispatch("display"));
                }
                taskInfo = dispatch.GetTasks();
            }
            else if (e.Key == Key.Escape)
            {
                inputBox.Clear();
                dispatch.Status = Controller.State.normal;
                displayBox.Content = "";
            }
            if (inputBox.Text != "" && e.Key == Key.Tab)
            {
                string words = inputBox.Text.Substring(inputBox.Text.LastIndexOf(';') + 1).ToLower();
                string temp = inputBox.Text.Substring(0, inputBox.Text.LastIndexOf(';') + 1);
                if (temp == "delete;" || temp == "edit;" || temp == "archive;" || temp == "find;")
                {
                    string getCommand = temp.Remove(';');
                    List<string> toBeDisplay = InstantSearch(words, getCommand);
                    if (toBeDisplay.Count != 0)
                    {
                        inputBox.Text = temp + autoCompleteSearch(words);
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
            dispatch.UserDispatch("undo");
            inputBox.Text = "";
            displayBox.Content = "Action undone";
            SetDisplay();
        }

        private void redoExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            dispatch.UserDispatch("redo");
            inputBox.Text = "";
            displayBox.Content = "Action redone";
            SetDisplay();
        }
        public string autoCompleteSearch(string input)
        {
            List<Task> contents = taskInfo;
            List<string> keywords = new List<string>(input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < keywords.Count; i++)
                if (keywords[i].Equals(""))
                    keywords.RemoveAt(i);
            List<int> hitcount = new List<int>();
            for (int i = 0; i < contents.Count; i++)
                hitcount.Add(0);
            int maxhits = 0;
            string temp = null;
            for (int i = 0; i < contents.Count; i++)
            {
                for (int j = 0; j < keywords.Count; j++)
                {
                    if (contents[i].Details.Contains(keywords[j]))
                    {
                        hitcount[i]++;

                        if (hitcount[i] > maxhits)
                        {
                            maxhits = hitcount[i];
                            temp = contents[i].Details;
                        }
                    }
                }
            }
            return temp;
        }


        private void inputBox_PreviewKeyDown(object sender, KeyEventArgs e)
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
            /*outputBox.Items.Clear();
            for (int i = 0; i < output.Count; i++)
                outputBox.Items.Add(output[i]);
        */
            List<Task> temp = new List<Task>();
            foreach (string item in output)
            {
                temp.Add(new Task(item));
            }

            dataGrid1.DataContext = temp;
            dataGrid1.Items.Refresh();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            inputBox.Focus();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {

            //Task temp = ((FrameworkElement)sender).DataContext as Task;
            DataGridRow selectedRow = (DataGridRow)(dataGrid1.ItemContainerGenerator.ContainerFromIndex(dataGrid1.SelectedIndex));
            int positionOfSeperator = selectedRow.Item.ToString().IndexOf(Utility.FILE_SEPARATER);
            String taskName = selectedRow.Item.ToString().Substring(0, positionOfSeperator);
            String command = "delete;" + taskName;
            List<String> output = new List<String>(dispatch.UserDispatch(command));
            SetOutputBox(output);
            taskInfo = dispatch.GetTasks();
        }

        private void Archive_Button_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow selectedRow = (DataGridRow)(dataGrid1.ItemContainerGenerator.ContainerFromIndex(dataGrid1.SelectedIndex));
            int positionOfSeperator = selectedRow.Item.ToString().IndexOf(Utility.FILE_SEPARATER);
            String taskName = selectedRow.Item.ToString().Substring(0, positionOfSeperator);
            String command = "archive;" + taskName;
            List<String> output = new List<String>(dispatch.UserDispatch(command));
            if (output.Count > 0 && output[0] == Utility.ERROR)
            {
                displayBox.Content = output[1];
            }
            else
            {
                SetOutputBox(output);
            }
            taskInfo = dispatch.GetTasks();
        }

        private void dataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            FrameworkElement element;
            String newData;
            Task updatedTask = dispatch.GetTasks().ElementAt(0);//create a temp task
            //  var newData;

            int taskNameLength = e.Row.Item.ToString().IndexOf(Utility.FILE_SEPARATER);
            String taskName = e.Row.Item.ToString().Substring(0, taskNameLength);
            Task deletedTask = dispatch.GetTasks().ElementAt(0); //to initialse
            List<Task> updatedList = dispatch.GetTasks();
            for (int i = 0; i < updatedList.Count; i++)
            {
                if (updatedList[i].Details == taskName)
                {
                    deletedTask = updatedList.ElementAt(i);
                    updatedList.RemoveAt(i);
                    break;
                }
            }
            
            if (dataGrid1.CurrentCell.Column.Header.Equals("Label"))
            {
                element = dataGrid1.Columns[3].GetCellContent(e.Row);
                newData = ((TextBox)element).Text;
                updatedTask = new Task(((Task)(e.Row.Item)).ToString());
                updatedTask.Labels = newData;
            }
            if (dataGrid1.CurrentCell.Column.Header.Equals("Deadline"))
            {
                element = dataGrid1.Columns[2].GetCellContent(e.Row);
                newData = ((TextBox)element).Text;
                updatedTask = new Task(((Task)(e.Row.Item)).ToString());
                updatedTask.Deadline = DateTime.Parse(newData);
            }
            String command = "add " + updatedTask.Details + " @" + updatedTask.Deadline + " #" + updatedTask.Labels.Trim() + " ";
            if (updatedTask.IsStarred)
                command += Utility.STARRED;
            List<String> output = dispatch.UserDispatch(command);
            if (output.Count > 0 && output[0] == Utility.ERROR)
            {
                updatedList.Add(deletedTask);
                displayBox.Content = output[1];
            }
            SetOutputBox(dispatch.UserDispatch("display"));
            return;
        }

    }
}

