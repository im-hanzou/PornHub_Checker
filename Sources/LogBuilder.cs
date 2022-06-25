using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHChecker
{
    public class LogBuilder
    {
        public LogBuilder()
        {

        }

        private List<string> _logs = new List<string>();
        private List<string> _logsW = new List<string>();

        public void Add(string data, Response response)
        {
            FileInfo fileInfo = new FileInfo(Environment.CurrentDirectory + @"\WithPremium.txt");
            FileInfo file_Info = new FileInfo(Environment.CurrentDirectory + @"\WithoutPremium.txt");

            //using (StreamWriter writer = fileInfo.CreateText())
            //{
            //if (response.Success && response.HavePremium)
            //{
            //    //writer.WriteLine(data);
            //}
            //else if (response.Success)
            //{
            //    //using (StreamWriter writer2 = file_Info.CreateText())
            //    //{
            //    //    //writer2.WriteLine(data);                        
            //    //}
            //}
            //}

            if (response.Success && response.HavePremium)
            {
                _logs.Add(data);
            }
            else if (response.Success)
            {
                _logsW.Add(data);
            }
        }

        public bool Save()
        {
            try
            {
                File.WriteAllLines(Environment.CurrentDirectory + @"\WithPremium.txt", _logs);
                File.WriteAllLines(Environment.CurrentDirectory + @"\WithoutPremium.txt", _logsW);
                return true;
            }
            catch
            {
                return false;
            }            
        }
    }
}
