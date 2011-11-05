using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace F9S1.RememberMe
{
    class Controller
    {
        Parser parse;
        Export GSync;
        Storage store;
        Operations taskData;
        MainWindow startWindow;
        public Controller(MainWindow appWindow)
        {
            startWindow = appWindow;
            GSync = new Export();
            parse = new Parser();
            store = new Storage();
            taskData = new Operations(store.ReadTasks(), store.ReadLabels());
            AlarmCheck checkAlarm = new AlarmCheck(this);
        }
        public Controller()
        {
            parse = new Parser();
            store = new Storage();
            taskData = new Operations(store.ReadTasks(), store.ReadLabels());
        }
        public void updateDisplay()
        {
            startWindow.SetDisplay();
        }

        public List<Task> GetTasks()
        {
            return taskData.TaskList;
        }
        public List<string> UserDispatch(string input)
        {
            if (input.Trim().Length > 3)
            {
                if (input.Trim().ToLower().Equals("exit") ||
                    input.Trim().ToLower().Equals("quit"))
                {
                    store.WriteTasks(taskData.GetList(), taskData.GetLabels());
                    Environment.Exit(0);
                }
            }
            List<string> parsedInput = parse.InputParse(input, taskData.GetLabels()), output = new List<string>();
            bool isModified = false;
            Debug.Assert(parsedInput != null);
            string commandName = parsedInput[0];
            if (commandName != Utility.ERROR)
                parsedInput.RemoveAt(0);

            switch (commandName)
            {

                case "label":
                    {
                        if (parsedInput[0] == "add")
                            isModified = taskData.AddLabel(parsedInput[1]);
                        else if (parsedInput[0] == "delete")
                            isModified = taskData.DeleteLabel(parsedInput[1]);
                        break;
                    }
                case "sync":
                    {
                        GSync.Synchronize(parsedInput[0], parsedInput[1], taskData.TaskList);
                        break;
                    }
                case "add":
                    {
                        isModified = taskData.AddTask(parsedInput);
                        if (!isModified)
                        {
                            output.Add(Utility.ERROR);
                            output.Add(Utility.ADD_ERROR);
                        }
                        break;
                    }

                case "delete":
                    {
                        if (parsedInput.Count < 1)
                            break;
                        isModified = taskData.DeleteTask(parsedInput[0]);
                        if (!isModified)
                        {
                            output.Add(Utility.ERROR);
                            output.Add(Utility.DELETE_ERROR);
                        }
                        break;
                    }

                case "edit":
                    {

                        isModified = taskData.EditTask(parsedInput, input);
                        
                        if (!isModified)
                        {
                            output.Add(Utility.ERROR);
                            output.Add(Utility.EDIT_ERROR);
                        }
                        break;
                    }
                case "archive":
                    {
                        if (parsedInput.Count < 1)
                            break;
                        isModified = taskData.ArchiveTask(parsedInput[0]);
                        if (!isModified)
                        {
                            output.Add(Utility.ERROR);
                            output.Add(Utility.ARCHIVE_ERROR);
                        }
                        break;
                    }
                case "undo":
                    {
                        isModified = taskData.UndoAction();
                        break;
                    }
                case "redo":
                    {
                        isModified = taskData.RedoAction();
                        break;
                    }
                case "clear":
                    {
                        isModified = taskData.ClearTasks();
                        break;
                    }
                case Utility.ERROR:
                    {
                        output = parsedInput;
                        break;
                    }
            }
            if (isModified)
            {
                store.WriteTasks(taskData.GetList(), taskData.GetLabels());
                taskData.UpdateTasks();
            }

            if (output.Count == 0)
                output = taskData.Display();
            return output;
        }
    }
}
