using System;
using System.Collections;
using System.Web.Hosting;
using GBML_Model.Properties;

namespace GBML_Model
{
    public class ExecuteModel
    {
        private readonly int _numModuleTotal;
        private ArrayList _expenseItems;
        private CreateData _createData;
        private InsertData _insertData;
        private CreateFirstData _createFirstData;

        public ExecuteModel(int numModuleTotal, string connectionServer)
        {
            _numModuleTotal = numModuleTotal;
            PublicData.ConnectionString =
                connectionServer.Equals("PROD") ? Settings.Default.ConnectionProd : Settings.Default.ConnectionPreprod;

            PublicData.ConnectionServer = connectionServer;
        }
     
        public void RunModel()
        {
            PublicData.InputPath =
                $"{HostingEnvironment.MapPath("~/Inputs")}\\{DateTime.Now:yyyyMMddHH}";
            var executor = new Executor(PublicData.InputPath);

            _createFirstData = (CreateFirstData)executor.ExecuteAndWriteDateTime(
                () => CreateFirstData.Create(_numModuleTotal), "RunModel");

            _insertData = new InsertData();

            //Directory.CreateDirectory(PublicData.InputPath);
            //var writeDateTime
            //    = new StreamWriter($"{PublicData.InputPath}\\RunModel_{DateTime.Now:yyyyMMddHH_mmss}.txt");
            //var startTotal = DateTime.Now;
            //_createFirstData = new CreateFirstData(_numModuleTotal);
            //var endTotal = DateTime.Now;
            //writeDateTime.WriteLine("createFirstData:" + (endTotal - startTotal).TotalSeconds);
            //_insertData = new InsertData();

            foreach (ActiveMachine item in PublicData.ActiveMachine)
            {
                _createData = new CreateData();
                executor.ExecuteAndWriteDateTime(() => _createData.GetCostCenter(item), "GetCostCenter");
                executor.ExecuteAndWriteDateTime(() => _createData.GetmachineData(item.Id), "GetCostCenter");

                //startTotal = DateTime.Now;
                //_createData.GetCostCenter(item);
                //endTotal = DateTime.Now;
                //writeDateTime.WriteLine(
                //    $"GetCostCenter:{(endTotal - startTotal).TotalSeconds}");
                //startTotal = DateTime.Now;
                //_createData.GetmachineData(item.Id);
                //endTotal = DateTime.Now;
                //writeDateTime.WriteLine(
                //    $"GetMachineData:{(endTotal - startTotal).TotalSeconds}");
                //writeDateTime.Flush();

                foreach (CostCenter costCenter in PublicData.CostCenter)
                {
                    _expenseItems = new ArrayList();

                    executor.ExecuteAndWriteDateTime(() => _createData.MainCreateData(_numModuleTotal, costCenter, item), "MainCreateData");

                    RunModel run = new RunModel();
                    executor.ExecuteAndWriteDateTime(() => run.RunModelMain(costCenter, ref _expenseItems), "RunModelMain");

                    WriteFile write = new WriteFile();
                    executor.ExecuteAndWriteDateTime(() => write.WriteDataToFiles(costCenter.Code, _expenseItems), "WriteDataToFiles");

                    executor.ExecuteAndWriteDateTime(() => _insertData.InsertResults(_expenseItems), "insertResults");

                    //startTotal = DateTime.Now;
                    //_createData.MainCreateData(_numModuleTotal, costCenter, item);
                    //endTotal = DateTime.Now;
                    //writeDateTime.WriteLine(
                    //    $"MainCreateData_{costCenter.Code}:{(endTotal - startTotal).TotalSeconds}");
                    //RunModel run = new RunModel();
                    //startTotal = DateTime.Now;
                    //run.RunModelMain(costCenter, ref _expenseItems);
                    //endTotal = DateTime.Now;
                    //writeDateTime.WriteLine(
                    //    $"RunModelMain_{costCenter.Code}:{(endTotal - startTotal).TotalSeconds}");

                    //WriteFile write = new WriteFile();

                    //startTotal = DateTime.Now;
                    //write.WriteDataToFiles(costCenter.Code, _expenseItems);
                    //endTotal = DateTime.Now;
                    //writeDateTime.WriteLine(
                    //    $"WriteDataToFiles_{costCenter.Code}:{(endTotal - startTotal).TotalSeconds}");

                    //startTotal = DateTime.Now;
                    //_insertData.InsertResults(_expenseItems);
                    //endTotal = DateTime.Now;
                    //writeDateTime.WriteLine($"insertResults_{costCenter.Code}:{(endTotal - startTotal).TotalSeconds}");

                    //writeDateTime.Flush();
                }
            }
            //writeDateTime.Close();
        }
    }
}
