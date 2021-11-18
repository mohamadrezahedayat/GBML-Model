using System;
using System.IO;

namespace GBML_Model
{
    
    public class Executor
    {
        private readonly string _path;
        private DateTime _startTime, _endTime;

        public Executor(string path)
        {
            //  var path = $"{HostingEnvironment.MapPath("~/Inputs")}\\{DateTime.Now:yyyyMMddHH}";
            _path = path;
            Directory.CreateDirectory(path);
        }

        private void ExecuteMethod(Action method)
        {
             _startTime = DateTime.Now;
            method();
             _endTime = DateTime.Now;
           
        }
        private object ExecuteMethod(Func<object> method)
        {
            _startTime = DateTime.Now;
            var result = method();
            _endTime = DateTime.Now;
            return result;
        }
        public void ExecuteAndWriteDateTime(Action method,string parentMethodName)
        {
            ExecuteMethod(method);
            var totalSeconds = (_endTime - _startTime).TotalSeconds;

            var fileName = $@"{parentMethodName}\{DateTime.Now:yyyyMMddHH_mmss}.txt";
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_path, fileName)))
            {
                outputFile.WriteLineAsync($"{nameof(method)}:{totalSeconds}");
            }
        }
        public object ExecuteAndWriteDateTime(Func<object> method, string parentMethodName)
        {
            var obj =method();
            var totalSeconds = (_endTime - _startTime).TotalSeconds;

            var fileName = $@"{parentMethodName}\{DateTime.Now:yyyyMMddHH_mmss}.txt";
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(_path, fileName)))
            {
                outputFile.WriteLineAsync($"{nameof(method)}:{totalSeconds}");
            }
            return obj;
        }
    }
}