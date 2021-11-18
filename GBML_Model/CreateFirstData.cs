using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using DataLayer;

namespace GBML_Model
{
    class CreateFirstData
    {
        Database _workDatabase;
        int _numVersionTest = (int)NumVersionTest.LastSuccessRunData;
        int _activeMachineTest = (int)ActiveMachineTest.Crm0102;

        public static CreateFirstData Create(int numModule)
        {
            return new CreateFirstData(numModule);
        }
        public CreateFirstData(int numModule)
        {
            _workDatabase = new Database(PublicData.ConnectionServer);
            PublicData.NumPossibleProducts = 0;
            PublicData.NumCostCenter = 0;

            var executor = new Executor(PublicData.InputPath);
            executor.ExecuteAndWriteDateTime(()=> GetactiveMachine(numModule) , "CreateFirstData");
            executor.ExecuteAndWriteDateTime( GetNumVersion , "CreateFirstData");
            executor.ExecuteAndWriteDateTime(GetDatEnd, "CreateFirstData");
            PublicData.CodProcedure = 501;
            executor.ExecuteAndWriteDateTime(()=> GetProcedureId(PublicData.CodProcedure), "CreateFirstData");
            executor.ExecuteAndWriteDateTime(GetCoopsStatusId, "CreateFirstData");


            //StreamWriter writeDateTime = new StreamWriter(PublicData.InputPath + "\\CreateFirstData_" + (DateTime.Now.ToString("yyyyMMddHH_mmss")) + ".txt");
            //DateTime startTot, endTot;
            //startTot = DateTime.Now;
            //GetactiveMachine(numModule);
            //endTot = DateTime.Now;
            //writeDateTime.WriteLine("GetActiveMachine:" + (endTot - startTot).TotalSeconds);
            //startTot = DateTime.Now;
            //GetNumVersion();
            //endTot = DateTime.Now;
            //writeDateTime.WriteLine("GetNumVersion:" + (endTot - startTot).TotalSeconds);
            //startTot = DateTime.Now;
            //GetDatEnd();
            //endTot = DateTime.Now;
            //writeDateTime.WriteLine("GetDatEnd:" + (endTot - startTot).TotalSeconds);
            //startTot = DateTime.Now;
            //PublicData.CodProcedure = 501;
            //GetProcedureId(PublicData.CodProcedure);
            //endTot = DateTime.Now;
            //writeDateTime.WriteLine("GetProcedureId:" + (endTot - startTot).TotalSeconds);
            //startTot = DateTime.Now;
            //GetCoopsStatusId();
            //endTot = DateTime.Now;
            //writeDateTime.WriteLine("GetCoopsStatusId:" + (endTot - startTot).TotalSeconds);
            //writeDateTime.Close();
        }
        public void GetactiveMachine(int numModule)
        {
            _workDatabase = new Database(PublicData.ConnectionServer);
            DataTable dt = new DataTable();
            PublicData.ActiveMachine = new List<ActiveMachine>();
            if (numModule == 1 || numModule == 2)
            {
                string commandString = $@"
                    select 
                        distinct m.machine_load_area_id,
                        t.cod_mlare || m.cod_mlare cod_subArea
                    from COA.COA_MACHINE_LOAD_AREAS t, COA_MACHINE_LOAD_AREAS m
                    where 
                        t.machine_load_area_id = m.mlare_machine_load_area_id
                        and ({numModule} = 2 or m.Flg_Sta_Mlare = {numModule})
                    order by t.cod_mlare || m.cod_mlare";

                dt = _workDatabase.GetDataTable(commandString, CommandType.Text);
                foreach (DataRow dr in dt.Rows)
                {
                    PublicData.ActiveMachine.Add(new ActiveMachine());
                    PublicData.ActiveMachine[PublicData.ActiveMachine.Count - 1].Index = PublicData.ActiveMachine.Count - 1;
                    PublicData.ActiveMachine[PublicData.ActiveMachine.Count - 1].Id = Convert.ToInt32(dr["Machine_Load_Area_Id"]);
                    PublicData.ActiveMachine[PublicData.ActiveMachine.Count - 1].CodeSubArea = (dr["cod_subArea"]).ToString();
                }

            }
            else
            {
                string commandString = $@"
                    select 
                        distinct m.machine_load_area_id,
                        t.cod_mlare || m.cod_mlare cod_subArea
                    from COA.COA_MACHINE_LOAD_AREAS t, COA_MACHINE_LOAD_AREAS m
                    where 
                        t.machine_load_area_id = m.mlare_machine_load_area_id
                        and m.machine_load_area_id = {_activeMachineTest}
                    order by t.cod_mlare || m.cod_mlare";

                dt = _workDatabase.GetDataTable(commandString, CommandType.Text);
                foreach (DataRow dr in dt.Rows)
                {
                    PublicData.ActiveMachine.Add(new ActiveMachine());
                    PublicData.ActiveMachine[PublicData.ActiveMachine.Count - 1].Index = PublicData.ActiveMachine.Count - 1;
                    PublicData.ActiveMachine[PublicData.ActiveMachine.Count - 1].Id = Convert.ToInt32(dr["Machine_Load_Area_Id"]);
                    PublicData.ActiveMachine[PublicData.ActiveMachine.Count - 1].CodeSubArea = (dr["cod_subArea"]).ToString();
                }
            }
        }
        public void GetNumVersion()
        {
            string commandString = string.Format(@"Select App_Coa_Pkg.Extract_Num_Ver_Gbml_Fun num_version
                                                     From dual");

            DataTable dt = _workDatabase.GetDataTable(commandString, CommandType.Text);

            PublicData.NumVersion = Convert.ToInt32(dt.Rows[0]["num_version"]);
            PublicData.NumVersion = _numVersionTest;// --for test must be edited
        }
        public void GetDatEnd()
        {
            string commandString = @"Select App_Coa_Pkg.Extract_Dat_End_Gbml_Fun dat_end,
                                                          to_char(App_Coa_Pkg.Extract_Dat_End_Gbml_Fun,
                                                                  'yyyy',
                                                                  'nls_calendar=persian') val_year
                                                     From dual";

            DataTable dt = _workDatabase.GetDataTable(commandString, CommandType.Text);

            PublicData.DateEnd = DateTime.Parse(dt.Rows[0]["dat_end"].ToString());
            PublicData.ValYear = dt.Rows[0]["val_year"].ToString();
        }
        public void GetProcedureId(int codProcedure)
        {
            PublicData.TypCost = 5;
            string commandString = $@" Select Proc.Procedure_Id
                                                      From Coa.Coa_Procedures Proc
                                                     Where Proc.Cod_Proc_Copro = '{codProcedure}'
                                                       And Proc.Typ_Cost_Copro = '{PublicData.TypCost}'";

            DataTable dt = _workDatabase.GetDataTable(commandString, CommandType.Text);

            PublicData.ProcedureId = Convert.ToInt32(dt.Rows[0]["Procedure_Id"]);
        }
        public void GetCoopsStatusId()
        {
            string commandString = $@" Select Coops.Operation_Status_Id
                                                      From Coa_Operation_Statuses Coops
                                                     Where Coops.Lkp_Cod_Opn_Coops = '{PublicData.ProcedureId}'";

            DataTable dt = _workDatabase.GetDataTable(commandString, CommandType.Text);

            PublicData.CoopsStatusId = Convert.ToInt32(dt.Rows[0]["Operation_Status_Id"]);
        }
    }
}