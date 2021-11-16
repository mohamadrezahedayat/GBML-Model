using System;
using System.Collections;
using System.IO;
using System.Web.Hosting;
using GBML_Model.Properties;

namespace GBML_Model
{
    public class ExecuteModel
    {
        int _numModuleTotal;
        ArrayList _expenseItems;
        CreateData _createData;
        InsertData _insertData;
        CreateFirstData _createFirstData;

        public void RunModel(string connectionServer, int identifierName, int numModule)
        {
            _numModuleTotal = numModule;
            PublicData.ConnectionString = 
                connectionServer.Equals("PROD") ?
                    Settings.Default.ConnectionProd : Settings.Default.ConnectionPreprod;

            PublicData.ConnectionServer = connectionServer;
            RunMainModel();
        }
        public void RunMainModel()
        {
            PublicData.InputPath =
                $"{HostingEnvironment.MapPath("~/Inputs")}\\{DateTime.Now:yyyyMMddHH}";

            Directory.CreateDirectory(PublicData.InputPath);
            StreamWriter writeDateTime 
                = new StreamWriter($"{PublicData.InputPath}\\RunMainModel_{DateTime.Now:yyyyMMddHH_mmss}.txt");


            var startTotal = DateTime.Now;
            _createFirstData = new CreateFirstData(_numModuleTotal);
            var endTotal = DateTime.Now;
            
            writeDateTime.WriteLine("createFirstData:" + (endTotal - startTotal).TotalSeconds);

            _insertData = new InsertData();

            foreach (ActiveMachine item in PublicData.ActiveMachine)
            {
                _createData = new CreateData();
                startTotal = DateTime.Now;
                _createData.GetCostCenter(item);
                endTotal = DateTime.Now;
                writeDateTime.WriteLine(
                    $"GetCostCenter:{(endTotal - startTotal).TotalSeconds}");
                
                startTotal = DateTime.Now;
                _createData.GetmachineData(item.Id);
                endTotal = DateTime.Now;
                writeDateTime.WriteLine(
                    $"GetMachineData:{(endTotal - startTotal).TotalSeconds}" );

                writeDateTime.Flush();
                foreach (CostCenter costCenter in PublicData.CostCenter)
                {
                    _expenseItems = new ArrayList();
                    
                    startTotal = DateTime.Now;
                    _createData.MainCreateData(_numModuleTotal, costCenter, item);
                    endTotal = DateTime.Now;
                    writeDateTime.WriteLine(
                        $"MainCreateData_{costCenter.Code}:{(endTotal - startTotal).TotalSeconds}");
                    
                    RunModel run = new RunModel();
                    startTotal = DateTime.Now;
                    run.RunModelMain(costCenter, ref _expenseItems);
                    endTotal = DateTime.Now;
                    writeDateTime.WriteLine(
                        $"RunModelMain_{costCenter.Code}:{(endTotal - startTotal).TotalSeconds}");
                    
                    WriteFile write = new WriteFile();
                    
                    startTotal = DateTime.Now;
                    write.WriteDataToFiles(costCenter.Code, _expenseItems);
                    endTotal = DateTime.Now;
                    writeDateTime.WriteLine(
                        $"WriteDataToFiles_{costCenter.Code}:{(endTotal - startTotal).TotalSeconds}");
                    
                    startTotal = DateTime.Now;
                    _insertData.InsertResults(_expenseItems);
                    endTotal = DateTime.Now;
                    writeDateTime.WriteLine($"insertResults_{costCenter.Code}:{(endTotal - startTotal).TotalSeconds}"); 
                    
                    writeDateTime.Flush();
                }
            }
            writeDateTime.Close();
        }
    }
}
