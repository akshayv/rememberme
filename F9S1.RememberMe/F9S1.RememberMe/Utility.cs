using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F9S1.RememberMe
{
    class Utility 
    {
        public const string EDIT_PRINT = "edit: ";
        public const string ADD_ERROR = "Add failed. Please check your input";
        public const string DELETE_ERROR = "Delete failed. Please check your input";
        public const string EDIT_ERROR = "Edit Failed. Please check your input";
        public const string SORT_ERROR = "Sort Failed. Please check your input";
        public const string ARCHIVE_ERROR = "Archive Failed. Please check your input";
        public const string DATE_ERROR = "Date Error. Please enter in the format \"<day> <hh:mm>\" or \"<dd-mm-yy> <hh:mm>\"";

        public const string ERROR = "Error";
        public const string LABEL_UNDEFINED_ERROR = "Undefined label error. Add new labels using \"label add <newlabel>\"";
        public const string LABEL_INPUT_ERROR = "Input error. The correct way of adding/deleting a label is \"label add/delete <label name>\"";
        public const string ADD_INPUT_ERROR = "Add error. Input missing";

        public const string DEFAULT_LABEL = "#others";
        public const string DEFAULT_NO_TIME = "undefined";
        public const string FILE_SEPARATER = " ~~ ";
        public const string STARRED = "**";
        public const string UNSTARRED = "--";

        public static TimeSpan NO_INTERVAL = new TimeSpan(0, 0, 0, 0);
        public static DateTime DEFAULT_ERROR_DATE = DateTime.MinValue;
        public static DateTime DEFAULT_UNDEFINED_DATE = DateTime.MaxValue;
    }
}
