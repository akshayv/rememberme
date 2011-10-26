using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F9S1.RememberMe
{
    class Task
    {
        private string details;     //No semicolons
        public string Details       //Property
        {
            get
            {
                return details;
            }
            set
            {
                if (value == null)
                    details = "?";
                else
                    details = value;
            }
        }

        private DateTime deadline;  //After now
        public string Deadline    //Property
        {
            get
            {
                if (deadline == Utility.DEFAULT_UNDEFINED_DATE)
                    return "";
                return deadline.ToString();
            }
            set
            {
                if (value == null || value == Utility.DEFAULT_NO_TIME || value == "")
                    deadline = Utility.DEFAULT_UNDEFINED_DATE;
                else
                    deadline = DateTime.Parse(value);          
            }
        }

        private TimeSpan interval;
        public TimeSpan Interval
        {
            get;
            set;
        }

        public bool IsRepeat
        {
            get
            {
                return !(interval.Equals(Utility.NO_INTERVAL));
            }
        }

        private bool isStarred;
        public bool IsStarred       //Property
        {
            get
            {
                return isStarred;
            }
            set
            {
                isStarred = value;
            }
        }
        
        private bool isArchived;
        public bool IsArchived       //Property
        {
            get
            {
                return isArchived;
            }
            set
            {
                isArchived = value;
            }
        }

        private string[] labels;    //Single word, alphabets, underscore, digits
        public string Labels      //Property
        {
            get
            {
                return ConvertLabelsToString(labels);
            }
            set
            {
                if (value == null)
                    labels = new string[]{"#others"};
                else
                    labels = ConvertStringToLabels(value);
            }
        }

        public Task(List<string> values)
        {
            Details = values[0];
            Deadline = values[1];
            Labels = values[2];
            IsStarred = Boolean.Parse(values[3]);
            Interval = TimeSpan.Parse(values[4]);
            IsArchived = false;

        }

        public Task(string line)
        {
            List<string> values = FromString(line);
            Details = values[0];
            Deadline = values[1];
            Labels = values[2];
            IsStarred = Boolean.Parse(values[3]);
            Interval = TimeSpan.Parse(values[4]);
            IsArchived = false;
        }
        
        public override int GetHashCode()
        {
            return details.GetHashCode() + deadline.GetHashCode() + isStarred.GetHashCode() + labels.GetHashCode();
        }

        private List<string> FromString(string line)
        {
            return new List<string>(line.Split(new string[]{Utility.FILE_SEPARATER},10, StringSplitOptions.None));
        }

        private string ShortenDeadline(string longer)
        {
            if (longer == "")
                return "";
            else
            {
                DateTime longDeadline = DateTime.Parse(longer);
                return String.Concat(SetIntLength(longDeadline.Day.ToString(), 2), "/", SetIntLength(longDeadline.Month.ToString(), 2), " ",
                                     SetIntLength(longDeadline.Hour.ToString(),2), ":", SetIntLength(longDeadline.Minute.ToString(), 2));
            }
        }

        private string SetIntLength(string shortInt, int length)
        {
            int difference = length - shortInt.Length;
            if (difference > 0)
            {
                for (int i = 0; i < difference; i++)
                    shortInt = "0" + shortInt;
            }
            return shortInt;
        }

        public override string ToString()
        {
            return Details + Utility.FILE_SEPARATER + Deadline.ToString() + Utility.FILE_SEPARATER + Labels + Utility.FILE_SEPARATER + IsStarred.ToString() + Utility.FILE_SEPARATER + IsArchived.ToString();
        }

        public override bool Equals(object compareObject)
        {
            Task compareTask = (Task) compareObject;
            return (Details == compareTask.Details) &&
                   (Deadline.Equals(compareTask.Deadline)) &&
                   (IsStarred == compareTask.IsStarred) &&
                   (Labels == compareTask.Labels);
        }

        private string ConvertLabelsToString(string[] toConvert)
        {
            string result = "";
            for (int i = 0; i < toConvert.Length; i++)
            {
                result = String.Concat(result, " ", toConvert[i]);
            }
            return result;
        }

        private string[] ConvertStringToLabels(string toConvert)
        {
            return toConvert.Split(' ');
        }

        public string SetLength(string input, int length)
        {
            int size = input.Length;
            if (size <= length)
            {
                for (int i = 0; i <= (length - size); i++)
                {
                    input = input + " ";
                }
                return input;
            }
            else
            {
                return input.Substring(0, length - 4) + ("...  ");
            }
        }

        public string GetDisplay()
        {
            string stars, archives;
            if (IsStarred)
            {
                stars = "*";
            }
            else
            {
                stars = " ";
            }
            if (IsArchived)
            {
                archives = "ARCH.";
            }
                else
            {
                archives = "     ";
            } 
            return stars + " " + SetLength(Details, 30) + " " + SetLength(ShortenDeadline(Deadline), 15) +  " " + SetLength(Labels.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim(), 8) + " " + archives;
        }
    }
}
 