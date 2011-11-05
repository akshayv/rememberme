using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F9S1.RememberMe
{
    class Parser
    {
        enum Command
        {
            add,
            edit,
            delete,
            clear,
            undo,
            redo,
            error,
            search,
            find,
            display,
            archive,
            sync,
            label
        };


        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public Parser()
        {

        }
        private Command ToCommand(string input)
        {
            switch (input)
            {
                case "edit":
                    return Command.edit;
                case "delete":
                    return Command.delete;
                case "clear":
                    return Command.clear;
                case "undo":
                    return Command.undo;
                case "redo":
                    return Command.redo;
                case "display":
                    return Command.display;
                case "search":
                    return Command.search;
                case "find":
                    return Command.find;
                case "sync":
                    return Command.sync;
                case "label":
                    return Command.label;
                case "archive":
                    return Command.archive;
                default:
                    return Command.add;
            }
        }

        private void addSemiCol(ref string input)
        {
            int count = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == ';')
                    {
                        count++;
                    }
                }
                int numOfSemi = 4;
                for (int i = 0; i < numOfSemi - count; i++)
                    input += ";";
           }

        public List<string> SymbolParse(string input, List<string> labels)
        {
            List<string> parsedInput = new List<string>(), inputLabels = new List<string>(), betaParse = new List<string>(input.Split(new Char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries)); ;
            string commandName, taskInterval, taskDetails, taskTime, betaInput = new string(input.ToCharArray());
            DateTime deadline;
            bool hasStars = betaInput.Contains("**");
            betaInput = betaInput.Replace("**", "");
            
            commandName = "add";
            if (betaParse[0].Trim().ToLower().Equals("add"))
            {
                string[] temp = betaInput.Split(new Char[] { ' ' , ';'}, 2);
                if (temp.Length < 2)
                {
                    parsedInput.Add(Utility.ERROR);
                    parsedInput.Add(Utility.INPUT_ERROR);
                    return parsedInput;
                }
                else
                    betaInput = temp[1];
            }

            if (betaParse[0].Trim().ToLower().Equals("edit"))
            {
                commandName = "edit";
                string[] temp = betaInput.Split(new Char[] { ' ', ';' }, 2);
                if (temp.Length < 2)
                {
                    parsedInput.Add(Utility.ERROR);
                    parsedInput.Add(Utility.INPUT_ERROR);
                    return parsedInput;
                }
                else
                    betaInput = temp[1];
            }

            if (input.Contains(';'))
            {
                addSemiCol(ref input);
                addSemiCol(ref betaInput);
                List<string> toBeChecked =  ColonParse(betaInput, labels);
                toBeChecked[0] = commandName;
                if (toBeChecked[1].Length == 0)
                {
                    logger.Info("No name entered");
                    parsedInput.Add(Utility.ERROR);
                    parsedInput.Add(Utility.INPUT_ERROR);
                    return parsedInput;
                }
                if (toBeChecked[2] == Utility.DEFAULT_ERROR_DATE.ToString(Utility.DATE_FORMAT))
                {
                    logger.Info("Incorrect Date format");
            
                    parsedInput.Add(Utility.ERROR);
                    parsedInput.Add(Utility.DATE_ERROR);
                    return parsedInput;
                }
                if (DateTime.Parse(toBeChecked[2]) < System.DateTime.Now)
                {
                    parsedInput.Add(Utility.ERROR);
                    parsedInput.Add(Utility.EARLY_DATE_ERROR);
                    return parsedInput;

                }
                if (!CheckLabels(toBeChecked[3], labels))
                {

                    logger.Info("Label not found");
                    parsedInput.Add(Utility.ERROR);
                    parsedInput.Add(Utility.LABEL_UNDEFINED_ERROR);
                    return parsedInput;
                }
                return toBeChecked;
            }
            
            if (betaInput.Contains('#'))
            {
                if (!(betaParse.Contains("#")))
                {
                    for (int i = 0; i < betaParse.Count; )
                    {
                        if (betaParse[i].Contains('#'))         //last words
                        {
                            inputLabels.Add(betaParse[i].Substring(1));
                            betaParse.RemoveAt(i);
                        }
                        else
                            i++;
                    }
                }
            }

            if (!CheckLabels(inputLabels, labels))
            {
                logger.Info("Label not found");
                  
                parsedInput.Add(Utility.ERROR);
                parsedInput.Add(Utility.LABEL_UNDEFINED_ERROR);
                return parsedInput;
            }

            if (!(betaInput.Contains('@')))
            {
                char[] splitter = new char[] { ' ' };
                taskTime = Utility.DEFAULT_NO_TIME;
                taskInterval = Utility.NO_INTERVAL.ToString();
                taskDetails = betaInput.Split('@', '#')[0].Trim();
            }
            else
            {
                taskDetails = betaInput.Split('@', '#')[0].Trim();
                int _at = betaInput.IndexOf('@');
                int _hash = betaInput.IndexOf('#');
                int length = betaInput.Length;
                taskTime = betaInput.Substring(_at + 1, ((_hash - _at > 0) ? _hash - _at - 1: length - _at - 1));
                taskTime = taskTime.Trim();
                taskInterval = GetRepeat(taskTime).ToString();
                if (taskTime.Contains('%'))
                   taskTime = taskTime.Replace(taskTime.Substring(taskTime.IndexOf('%')).Split(' ', ';')[0], "");
                deadline = ToDate(taskTime);
                if (deadline.Equals(Utility.DEFAULT_ERROR_DATE))
                {
                    logger.Info("Incorrect Date Format");
                    parsedInput.Add(Utility.ERROR);
                    parsedInput.Add(Utility.DATE_ERROR);
                    return parsedInput;
                }
                else if (deadline < System.DateTime.Now)
                {
                    parsedInput.Add(Utility.ERROR);
                    parsedInput.Add(Utility.EARLY_DATE_ERROR);
                    return parsedInput;
                }
                else
                    taskTime = deadline.ToString(Utility.DATE_FORMAT);
            }

            if (taskDetails == null || taskDetails == "")
            {
                parsedInput.Add(Utility.ERROR);
                parsedInput.Add(Utility.INPUT_ERROR);
                return parsedInput;
            }

            parsedInput.Add(commandName);
            parsedInput.Add(taskDetails);
            parsedInput.Add(taskTime);
            parsedInput.Add(String.Concat(inputLabels));
            parsedInput.Add(hasStars.ToString());
            parsedInput.Add(taskInterval);
            return parsedInput;
        }

        private bool CheckLabels(string input, List<string> labels)
        {
            string[] inputSplit = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in inputSplit)
            {
                if (!labels.Contains(item))
                    return false;
            }
            return true;
        }
        private bool CheckLabels(List<string> input, List<string> labels)
        {
            foreach (string item in input)
            {
                if (!labels.Contains(item))
                    return false;
            }
            return true;
        }

        public List<string> LabelParse(string input, List<string> labels)
        {
            char[] splitter = new char[] { ' ' , ';'};
            List<string> parsedInput = new List<string>(), betaParse = new List<string>(input.Split(splitter, StringSplitOptions.RemoveEmptyEntries));
            if (betaParse.Count < 3 || (betaParse[1] != "add" && betaParse[1] != "delete"))
            {
                parsedInput.Add(Utility.ERROR);
                parsedInput.Add(Utility.LABEL_INPUT_ERROR);
                return parsedInput;
            }
            parsedInput.Add(betaParse[0].ToLower());
            parsedInput.Add(betaParse[1].ToLower());
            parsedInput.Add(betaParse[2].ToLower());
            return parsedInput;
        }

        public List<string> CommandParse(string input)
        {
            List<string> parsedInput = new List<string>();
            if (input.Contains(';'))
            {
                char[] splitter = new char[] { ';' };
                parsedInput = new List<string>(input.Split(splitter, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                char[] splitter = new char[] { ' ' };
                parsedInput = new List<string>(input.Split(splitter, 2, StringSplitOptions.RemoveEmptyEntries));                
            }
            parsedInput[0] = parsedInput[0].ToLower();
            return parsedInput;
        }

        private TimeSpan GetRepeat(string dateInput)
        {
            TimeSpan interval;
            if (dateInput.Contains('%'))
            {
                int posmod = dateInput.IndexOf('%');
                string next = dateInput.Substring(posmod + 1).Split(' ', ';')[0];
                if (next == "w" || next == "weekly")
                {
                    interval = Utility.WEEK_INTERVAL;
                }
                else if (next == "m" || next == "monthly")
                {
                    interval = Utility.MONTH_INTERVAL;
                }
                else if (Char.IsDigit(next[0]))
                {
                    interval = new TimeSpan(int.Parse(next), 0, 0, 0);
                }
                else
                    interval = Utility.NO_INTERVAL;
            }
            else
                interval = Utility.NO_INTERVAL;
            return interval;
        }

        public List<string> ColonParse(string input, List<string> labels)
        {
            List<string> betaInput = new List<string>(input.Split(';')), parsedInput = new List<string>();
            
                parsedInput.Add("add");
                try
                {
                    parsedInput.Add(betaInput[0].Trim());
                }                                       //Task Details 
                catch (Exception e)
                {
                    logger.Error("Details null");
                    parsedInput.Add("");
                }
               
            try
                {
                    parsedInput.Add(ToDate(betaInput[1].Trim().ToLower()).ToString(Utility.DATE_FORMAT));          //Deadline
                }
            catch (Exception e)
            {
                parsedInput.Add("");
                logger.Error("Deadline null");
            }
           
            try
                {
                    parsedInput.Add(betaInput[2].Trim().ToLower());                             //Labels
                }
           
               catch (Exception e)
                {
                   
                    logger.Error("Label null");
                    parsedInput.Add("");
                }
               try
                {
                    parsedInput.Add((betaInput[3].Trim().ToLower() == "high").ToString());      //Priority
                }
                 catch (Exception e)
                {
                     
                    logger.Error("Priority null");
                    parsedInput.Add("");
                }
               
                try
                {
                    parsedInput.Add(GetRepeat(betaInput[1].Trim()).ToString());                 //Repetition
                }
                catch (Exception e)
                {
                    parsedInput.Add("");

                    logger.Error("isRepeat null");
                }
               
            return parsedInput;

            }
           

        public List<string> InputParse(string input, List<string> labels)
        {
            
            Command inputCommand = ToCommand(input.Trim().Split(new char[] { ' ', ';' })[0].ToLower());
            List<string> parsedInput = new List<string>();
            if (inputCommand == Command.add || inputCommand == Command.edit)
            {
                parsedInput = SymbolParse(input, labels);
            }
            else if (inputCommand == Command.label)
            {
                parsedInput = LabelParse(input, labels);
            }
            else
            {
                parsedInput = CommandParse(input);
            }
            return parsedInput;
        }

        public string ToDayValid(string day)
        {
            if (day.Contains("monday") || day.Contains("mon"))
                return "monday";
            else if (day.Contains("tuesay") || day.Contains("tue"))
                return "tuesday";
            else if (day.Contains("wednesday") || day.Contains("wed"))
                return "wednesday";
            else if (day.Contains("thursday") || day.Contains("thu") || day.Contains("thurs"))
                return "thursday";
            else if (day.Contains("friday") || day.Contains("fri"))
                return "friday";
            else if (day.Contains("saturday") || day.Contains("sat"))
                return "saturday";
            else if (day.Contains("sunday") || day.Contains("sun"))
                return "sunday";
            else if (day.Contains("today"))
                return "today";
            else if (day.Contains("tomorrow") || day.Contains("tom"))
                return "tomorrow";
            else
            {
                return "nil";
            }
        }
        private DayOfWeek toDay(string day)
        {
            day = day.Trim();
            if (day == "monday" || day == "mon")
                return DayOfWeek.Monday;
            else if (day == "tuesday" || day == "tue")
                return DayOfWeek.Tuesday;
            else if (day == "wednesday" || day == "wed")
                return DayOfWeek.Wednesday;
            else if (day == "thursday" || day == "thu")
                return DayOfWeek.Thursday;
            else if (day == "friday" || day == "fri")
                return DayOfWeek.Friday;
            else if (day == "saturday" || day == "sat")
                return DayOfWeek.Saturday;
            else if (day == "sunday" || day == "sun")
                return DayOfWeek.Sunday;
            else if (day == "tomorrow")
                return System.DateTime.Today.AddDays(1).DayOfWeek;
            else
                return System.DateTime.Today.DayOfWeek;
        }
        private int NumberOfDays(string day)
        {
            DayOfWeek deadline = toDay(day);
            DayOfWeek curDay = System.DateTime.Today.DayOfWeek;
            if (deadline >= curDay)
                return (deadline - curDay);
            else
                return deadline - curDay + 7;
        }

        private DateTime updateTime(ref DateTime Template, DateTime containsTime)
        {
           Template=Template.AddHours(containsTime.Hour);
           Template=Template.AddMinutes(containsTime.Minute);
           Template= Template.AddSeconds(containsTime.Second);
            return Template;
        }

        private string RemoveDay(string date, string day)
        {
            day = day.Substring(0, 3);
            string[] dateParse = date.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < dateParse.Length; i++)
                if (dateParse[i].Contains(day))
                    dateParse[i] = "";
            return String.Concat(dateParse);
        }
        public DateTime ToDate(string toBeConverted)
        {
            if (toBeConverted.Length == 0)
                return Utility.DEFAULT_UNDEFINED_DATE;
            DateTime tempDate = new DateTime();
            string day = ToDayValid(toBeConverted.ToLower().Trim());
            if (day != "nil")
            {
                tempDate = System.DateTime.Today.Date;
                int x = NumberOfDays(day);
                tempDate = tempDate.AddDays(x * 1.0);
                toBeConverted = RemoveDay(toBeConverted, day);
                DateTime tempTime;
                try
                {
                    tempTime = DateTime.Parse(toBeConverted);
                    tempDate = updateTime(ref tempDate, tempTime);
                }
                catch (Exception e)
                { 
                }
            }
            else
            {   
                
                try
                {
                    tempDate = DateTime.Parse(toBeConverted);
                }
                catch (Exception e)
                {
                    if (e is FormatException)
                        return Utility.DEFAULT_ERROR_DATE;
                }
            }
            return tempDate;
        }
    }
}
