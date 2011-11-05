using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Threading;

namespace F9S1.RememberMe
{
    public delegate void timeCheck();
    class AlarmCheck
    {
        public delegate void timeCheck();
        List<Task> taskInfo;
        Controller dispatch;
        Alarm newAlarm = new Alarm("",DateTime.Now);
        public AlarmCheck(Controller dispatch)
        {
            this.dispatch = dispatch;
            newAlarm.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new timeCheck(SetAlarm));
        }
        public void SetAlarm()
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
                    showAlarm.ShowDialog();
                    time = showAlarm.getTimeArray();
                    if (time == null)
                    {
                        taskInfo[i].IsArchived = true;
                        string command = "archive;" + taskInfo[i].Details;
                        dispatch.UserDispatch(command);
                        dispatch.updateDisplay();
                    }
                    else
                    {
                        //Debug.Assert(taskInfo != null);
                        updateDeadline(ref taskInfo, time, i);
                        string command = "edit " + taskInfo[i].Details + " @" + taskInfo[i].Deadline;
                        dispatch.UserDispatch(command);
                        dispatch.updateDisplay();
                    }
                 }
            }
            newAlarm.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new timeCheck(SetAlarm));
        }
        void updateDeadline(ref List<Task> taskList, int[] time, int i)
        {
            TimeSpan difference = new TimeSpan(time[0], time[1], time[2], 0);
            DateTime updatedDate = DateTime.Now.Add(difference);
            taskList[i].Deadline = updatedDate;
            string command = "edit " + taskList[i].Details + " @" + updatedDate;
            dispatch.UserDispatch(command);
            dispatch.UserDispatch("display");
        }
          
    }
}
