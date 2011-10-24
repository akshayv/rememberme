using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace F9S1.RememberMe
{
    class Storage
    {
        string contentFileName = "RememberMe.content.txt";
        string labelFileName = "RememberMe.labels.txt";
        public Storage()
        {
            
            if (!File.Exists(contentFileName))
            {
                StreamWriter contentStream = new StreamWriter(contentFileName);
                contentStream.Close();
            }
            if (!File.Exists(labelFileName))
            {
                StreamWriter labelStream = new StreamWriter(labelFileName);
                labelStream.Write(Utility.DEFAULT_LABEL);
                labelStream.Close();
            }
        }

        public void WriteLabels(List<string> labels)
        {
            TextWriter writer = new StreamWriter(labelFileName);
            for (int i = 0; i < labels.Count; i++)
            {
                writer.WriteLine(labels[i]);
            }
            writer.Close();
        }

        public List<string> ReadLabels() //Exception if file is missing
        {
            List<string> labels = new List<string>();
            using (StreamReader reader = new StreamReader(labelFileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    labels.Add(line);
                }
            }
            return labels;
        }

        public void WriteTasks(List<string> contents) //Exception if file is missing
        {
            TextWriter writer = new StreamWriter(contentFileName);
            for (int i = 0; i < contents.Count; i++)
            {
                writer.WriteLine(contents[i]);
            }
            writer.Close();
        }

        public List<string> ReadTasks() //Exception if file is missing
        {
            List<string> contents = new List<string>();
            using (StreamReader reader = new StreamReader(contentFileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    contents.Add(line);
                }
            }
            return contents;
        }
    }
}
