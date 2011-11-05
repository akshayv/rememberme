using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F9S1.RememberMe
{
    class Operations
    {
        
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        List<Task> taskList;
        List<string> labels;

        public List<Task> TaskList
        {
            get
            {
                return new List<Task>(taskList);
            }
        }

        public List<string> Labels
        {
            get
            {
                return labels;
            }
        }
        
        Stack<List<Task>> undoStack, redoStack;
        public Operations(List<string> stringListTasks,List<string> labelList)
        {
            taskList = new List<Task>();
            labels = new List<string>();
            for (int i = 0; i < stringListTasks.Count; i++)
            {
                taskList.Add(new Task(stringListTasks[i]));
            }
            for (int i = 0; i < labelList.Count;i++ )
            {
                labels.Add(labelList[i]);
            }
            undoStack = new Stack<List<Task>>();
            redoStack = new Stack<List<Task>>();
            undoStack.Push(new List<Task>(taskList));
        }
        public void UpdateTasks()
        {
            if ((redoStack.Count > 0) && (taskList.Equals(redoStack.Peek())))
            {
                undoStack.Push(redoStack.Pop());
            }
            else
            {
                undoStack.Push(new List<Task>(taskList));
                redoStack.Clear();
            }
        }

        public bool UndoAction()
        {
            if (undoStack.Count > 1)
            {
                redoStack.Push(undoStack.Pop());
                taskList = new List<Task>(undoStack.Peek());
            }
            else
                logger.Info("No more undos");
            return false;
        }
        public bool RedoAction()
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(redoStack.Pop());
                taskList = new List<Task>(undoStack.Peek());
            }
            else
                logger.Info("No more redos");
            return false;
        }

        public List<string> GetLabels()
        {
            List<string> LabelList = new List<string>();
            for (int i = 0; i < labels.Count; i++)
            {
                LabelList.Add(labels[i]);
            }
            return LabelList;
        }
        

        public List<string> GetList()
        {
            List<string> stringListTasks = new List<string>();
            for (int i = 0; i < taskList.Count; i++)
            {
                stringListTasks.Add(taskList[i].ToString());
            }
            return stringListTasks;
        }
        public bool DeleteLabel(string newLabel)
        {
            for (int i = 0; i < labels.Count; i++)
                if (labels[i].ToLower() == newLabel.ToLower())
                {
                    labels.Remove(newLabel);
                    return true;
                }
            return false;
        }      
        public bool AddLabel(string newLabel)
        {
            for (int i = 0; i < labels.Count; i++)
                if (labels[i].ToLower() == newLabel.ToLower())
                    return false;
            labels.Add(newLabel);
            
            return true;
        }
        public bool AddTask(List<string> newTask)
        {
            newTask[0] = CheckIfDuplicate(newTask[0]);
            taskList.Add(new Task(newTask));
            return true;
        }
        string CheckIfDuplicate(string taskDetails)
        {
            int count = 0;
            string newDetails = taskDetails;
            bool isModified = true;
            while (isModified)
            {
                isModified = false;
                for (int i = 0; i < taskList.Count; i++)
                    if (taskList[i].Details == newDetails)
                    {
                        count++;
                        isModified = true;
                    }
                if (count != 0)
                {
                    newDetails = taskDetails + "(" + count + ")";
                }
            }
            if (isModified)
                logger.Info("Task with same name exists");
            return newDetails;
        }
        public bool DeleteTask(string taskDetails)
        {
            Task foundTask = SearchTask(taskDetails);
            Task Temp = foundTask;
            if (foundTask != null)
            {
                  taskList.Remove(foundTask);

                return true;
            }
            return false;
        }
        public bool ArchiveTask(string taskDetails)
        {
            Task foundTask = SearchTask(taskDetails);
            Task temp = foundTask;
            if (foundTask != null && !foundTask.IsArchived) //archive how?
            {
                foundTask.IsArchived = true;
                if (foundTask.Interval != TimeSpan.Parse("00:00:00"))
                {
                    DeleteTask(taskDetails);
                    foundTask.Deadline = foundTask.Deadline.Add(foundTask.Interval);
                    foundTask.IsArchived = false;
                    taskList.Add(foundTask);
                }
                return true;
            }
            return false;
        }
        public bool ClearTasks()
        {
            taskList.Clear();
            return true;
        }
        public bool EditTask(List<string> editInput, string input)
        {
            int n = -1;
            if (editInput.Count < 5)
                return false;
            Task foundTask = SearchTask(editInput[0], ref n);
            if (!(input.Contains('@')))
                editInput[1] = foundTask.Deadline.ToString(Utility.SHORT_DATE_FORMAT);
            if (!(input.Contains('#')))
                editInput[2] = foundTask.Labels.ToString(); 
            if (foundTask != null)
            {
                taskList[n] = new Task(editInput);
                return true;
            }
            return false;
        }
        public Task SearchTask(string taskDetails)
        {
            Task toBeFound = null;
            for (int i = 0; i < taskList.Count; i++)
            {
                if (taskList[i].Details.Equals(taskDetails))
                {
                    toBeFound = taskList[i];
                }
            }

            logger.Info("Task not found");
            return toBeFound;
        }
        public Task SearchTask(string taskDetails, ref int k)
        {
            Task toBeFound = null;
            for (int i = 0; i < taskList.Count; i++)
            {
                if (taskList[i].Details.Equals(taskDetails))
                {
                    toBeFound = taskList[i];
                    k = i;
                }
            }

            logger.Info("Task not found");
            return toBeFound;
        }
        public List<string> Display()
        {
            List<string> taskDetails = new List<string>();
            for (int i = 0; i < taskList.Count; i++)
                if(taskList[i].IsArchived==false)
                    taskDetails.Add(taskList[i].ToString());
            return taskDetails;
        }
    }
}
