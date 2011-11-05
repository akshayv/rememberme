using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;

namespace F9S1.RememberMe
{
    class Export
    {
        public void Synchronize(string username, string password, List<Task> taskList)
        {
            CalendarService Gcal = new CalendarService("remMe");
            Gcal.setUserCredentials(username, password);
            //get tasks from google
            EventQuery query = new EventQuery("https://www.google.com/calendar/feeds/default/private/full");
            EventFeed feed = Gcal.Query(query);

            //delete all RM tasks
            query.Query = "[RM!]";
            EventFeed allTasks = Gcal.Query(query);
            for (int i = 0; i < allTasks.Entries.Count; i++)
            {
                AtomEntry task = allTasks.Entries[i];
                task.Delete();
            }

            for (int i = 0; i < taskList.Count; i++)
            {
                {
                    EventEntry entry = new EventEntry();
                    entry.Title.Text = "[RM!]" + taskList[i].Details;
                    entry.Content.Content = "Label = " + taskList[i].Labels;
                    if (taskList[i].Deadline.Year != DateTime.MaxValue.Year)
                    {
                        When eventTime = new When(taskList[i].Deadline, taskList[i].Deadline.AddHours(1));
                        entry.Times.Add(eventTime);
                    }
                    Uri postUri = new Uri("https://www.google.com/calendar/feeds/default/private/full");
                    AtomEntry insertedEntry = Gcal.Insert(postUri, entry);
                }
            }
        }
    }
}

