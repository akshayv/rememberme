using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F9S1.RememberMe
{
    class Controller
    {
        public enum State
        {
            normal,
            edit
        };

        State status;
        public State Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }
        Parser parse;
        Storage store;
        List<string> labels;
        Register taskData;
        public Controller()
        {
            status = State.normal;
            parse = new Parser();
            store = new Storage();
            taskData = new Register(store.ReadTasks());
            labels = store.ReadLabels();
        }

        public List<Task> GetTasks()
        {
            return taskData.TaskList;
        }
     

//        public List<string> CallSearch(string input)
//        {
//            List<string> tobeDisplayed = taskData.InstantSearch(input);
//            return tobeDisplayed;
//        }

        public void WriteToFile(List<Task> taskList)
        {
            taskData.TaskList = taskList;
            store.WriteTasks(taskData.GetList());
        }

        public List<string> UserDispatch(string input)
        {
            if (input.Trim().Length > 3)
            {
                if (input.Trim().ToLower().Equals("exit") ||
                    input.Trim().ToLower().Equals("quit"))
                {
                    Environment.Exit(0);
                }
            }
            List<string> parsedInput = parse.InputParse(input, labels), output = new List<string>();
            bool isModified = false;
            string commandName = parsedInput[0];
            if (commandName != Utility.ERROR)
                parsedInput.RemoveAt(0);
           
            if (status.Equals(State.normal))
            {
                switch (commandName)
                {

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
                            string editOut = taskData.MoveTaskToEnd(parsedInput[0]);
                            if (editOut != "")
                                isModified = true;
                            if (isModified)
                            {
                                status = State.edit;
                                output.Add(Utility.EDIT_PRINT + editOut);
                            }
                            if (!isModified)
                            {
                                output.Add(Utility.ERROR);
                                output.Add(Utility.EDIT_ERROR);
                            }
                            break;
                        }
                    case "archive":
                        {
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
            }
            else if (status == State.edit)
            {
                taskData.EditTask(parsedInput);
                isModified = true;
                status = State.normal;

                if (!isModified)
                {
                    output.Add(Utility.ERROR);
                    output.Add(Utility.EDIT_ERROR);
                }
            }

            if (isModified)
            {
                store.WriteTasks(taskData.GetList());
                taskData.UpdateTasks();
            }

            if (output.Count == 0)
                output = taskData.Display();
            return output;
        }
    }
}
