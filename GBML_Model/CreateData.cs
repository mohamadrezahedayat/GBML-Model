using System;
using System.Collections.Generic;
using System.Data;
using DataLayer;
using System.Collections;
using System.IO;

namespace GBML_Model
{
    public class CreateData
    {
        Database _workDatabase;
        int _flgCostCenter = (int)FlagCostCenter.RunAllCostCenters;
        int _costCenterCodTest = (int)CostCenterCodeTest.Cgl;
        public CreateData()
        {
            _workDatabase = new Database(PublicData.ConnectionServer);
        }
        public void MainCreateData(int numModule, CostCenter costCenter, ActiveMachine activeMachine)
        {
            _workDatabase = new Database(PublicData.ConnectionServer);
            StreamWriter writeDateTime =
                new StreamWriter($"{PublicData.InputPath}\\MainCreateData{DateTime.Now:yyyyMMddHH_mmss}.txt");
          
            var startTotal = DateTime.Now;
            DeleteData();
            var endTotal = DateTime.Now;
            writeDateTime.WriteLine($"DeleteData:{ (endTotal - startTotal).TotalSeconds}");

            startTotal = DateTime.Now;
            GetPossibleProducts(costCenter.MachineLoadId, costCenter.Id);
            endTotal = DateTime.Now;
            writeDateTime.WriteLine("GetPossibleProducts:" + (endTotal - startTotal).TotalSeconds);
            startTotal = DateTime.Now;
            GetCodDataTable(costCenter.Code);
            endTotal = DateTime.Now;
            writeDateTime.WriteLine("GetCodDataTable:" + (endTotal - startTotal).TotalSeconds);
            startTotal = DateTime.Now;
            GettechnicalData(costCenter.Code);
            endTotal = DateTime.Now;
            writeDateTime.WriteLine("GetTechnicalData:" + (endTotal - startTotal).TotalSeconds);
            startTotal = DateTime.Now;
            GetNumFormulas(costCenter);
            endTotal = DateTime.Now;
            writeDateTime.WriteLine("GetNumFormulas:" + (endTotal - startTotal).TotalSeconds);
            startTotal = DateTime.Now;
            GetferroalloysData(costCenter.Id);
            endTotal = DateTime.Now;
            writeDateTime.WriteLine("GetferroalloysData:" + (endTotal - startTotal).TotalSeconds);
            startTotal = DateTime.Now;
            GetextractAssignExpit(costCenter.Id);
            endTotal = DateTime.Now;
            writeDateTime.WriteLine("GetExtractAssignExpit:" + (endTotal - startTotal).TotalSeconds);
            startTotal = DateTime.Now;
            GetLkpGroup_activeMachine(activeMachine);
            endTotal = DateTime.Now;
            writeDateTime.WriteLine("GetLkpGroup_activeMachine:" + (endTotal - startTotal).TotalSeconds);
        }
        public void DeleteData()
        {
            PublicData.ChargingRatio = Array.Empty<double>();
            PublicData.Std = Array.Empty<double>();
            PublicData.Paintlosses = 0;
            PublicData.Accidental = 0;
            PublicData.Samples = 0;
            PublicData.HeadAndTail = 0;
            PublicData.DryCoating = 0;
            PublicData.CoilProcess = 0;
            PublicData.ConsumableTin = 0;
            PublicData.TinLosses = 0;
            PublicData.ZincIngot = 0;
            PublicData.ZincLosses = 0;
            PublicData.Trimming = 0;
            PublicData.CoilLoss = 0;
            PublicData.LossSample = 0;
            PublicData.BarCrops = 0;
            PublicData.HeatingScale = 0;
            PublicData.Cobbles = 0;
            PublicData.OxideScale = 0;
            PublicData.Slabcrops = 0;
            PublicData.SlabTrimmer = 0;
            PublicData.SlabScrapped = 0;
            PublicData.SlabLosses = 0;
            PublicData.LadleSkull = 0;
            PublicData.TundishSkull = 0;
            PublicData.SpongeIron = 0;
            PublicData.Scrap = 0;
            PublicData.NormalHeat = 0;
            PublicData.RecycledSteel = 0;
            PublicData.RecuperableMaterials = 0;
            PublicData.Products = 1;
            PublicData.TotStd = 0;
            PublicData.TotCharg = 0;
            PublicData.TotQtyStd = 0;
            PublicData.TotQtyCharg = 0;
        }
        public void GetLkpGroup_activeMachine(ActiveMachine activeMachine)
        {
            DataTable dt = new DataTable();
            string commandString = $@"Select Distinct Assma.Lkp_Grp_Alt_Assma
                                      From Coa_Assignment_Machines Assma
                                      Where Assma.Mlcyc_Machine_Load_Cycle_Id In
                                        (Select Mlcyc.Machine_Load_Cycle_Id
                                         From 
                                            Coa_Machine_Load_Cycles Mlcyc
                                         Where 
                                            Mlcyc.Mlare_Machine_Load_Area_Id = {activeMachine.Id})
                                            And Assma.Lkp_Grp_Alt_Assma Is Not Null)";

            dt = _workDatabase.GetDataTable(commandString, CommandType.Text);
            int count = dt.Rows.Count;
            if (count > 0 && dt.Rows[0]["Lkp_Grp_Alt_Assma"] != DBNull.Value)
            {
                activeMachine.LkpGrpAlt = Convert.ToInt32(dt.Rows[0]["Lkp_Grp_Alt_Assma"]);
            }
        }
        public void GetCostCenter(ActiveMachine activeMachine)
        {
            DataTable dt = new DataTable();
            PublicData.CostCenter = new List<CostCenter>();
            PublicData.NumCostCenter = 0;
            string commandString = $@"
                select distinct 
                    t.cost_center_id, 
                    t.cod_cc_ccntr, 
                    t.des_cc_ccntr,
                    t.Qty_Max_Ccntr, 
                    nvl(t.flg_virt_ccntr,0) flg_virt_ccntr
                from 
                    COA.COA_COST_CENTERS t,
                    COA.COA_ASSIGNMENT_MACHINES a,
                    COA.COA_MACHINE_LOAD_CYCLES c,
                    Coa_Machine_Load_Areas M1
                where 
                    c.mlare_machine_load_area_id = m1.machine_load_area_id
                    and a.mlcyc_machine_load_cycle_id = c.machine_load_cycle_id
                    and t.cost_center_id = a.ccntr_cost_center_id
                    and m1.machine_load_area_id= {activeMachine.Id}
                    and ({_flgCostCenter} = 0 or t.cod_cc_ccntr in ('{_costCenterCodTest}'))
                order by
                    t.cod_cc_ccntr ;";

            dt = _workDatabase.GetDataTable(commandString, CommandType.Text);
            PublicData.NumCostCenter = dt.Rows.Count;

            foreach (DataRow dr in dt.Rows)
            {
                PublicData.CostCenter.Add(new CostCenter());
                PublicData.CostCenter[PublicData.CostCenter.Count - 1].Index = PublicData.CostCenter.Count - 1;
                PublicData.CostCenter[PublicData.CostCenter.Count - 1].Id = Convert.ToInt32(dr["cost_center_id"]);
                PublicData.CostCenter[PublicData.CostCenter.Count - 1].Code = Convert.ToInt32(dr["cod_cc_ccntr"]);
                PublicData.CostCenter[PublicData.CostCenter.Count - 1].Description = dr["des_cc_ccntr"].ToString();
                PublicData.CostCenter[PublicData.CostCenter.Count - 1].FlagVirtual = Convert.ToInt32(dr["flg_virt_ccntr"]);
                if (dr["Qty_Max_Ccntr"] != DBNull.Value)
                    PublicData.CostCenter[PublicData.CostCenter.Count - 1].MaxHour = Convert.ToInt32(dr["Qty_Max_Ccntr"]);
                else
                    PublicData.CostCenter[PublicData.CostCenter.Count - 1].MaxHour = 0;
                PublicData.CostCenter[PublicData.CostCenter.Count - 1].MachineLoadId = activeMachine.Id;
            }
        }
        public void GetPossibleProducts(int machineLoadId, int costCenterId)
        {
            DataTable dt = new DataTable();
            PublicData.PossibleProducts = new List<PossibleProducts>();
            string commandString = $@"
                Select 
                    Pospr.Possible_Product_Id,
                    Assma.Ccntr_Cost_Center_Id,
                    madat.machin_data_id,
                    Pospr.Cod_Prod_Pospr,
                    Proat.Cod_Sha_Proat,
                    Proat.Cod_Typ_Proat,
                    Proat.Cod_Edge_Proat,
                    Proat.Cod_Sur_Proat,
                    Proat.Cod_Fla_Proat,
                    Proat.Cod_Ptc_Proat,
                    Proat.Cod_Tol_Proat,
                    Proat.Cod_Int_Qly_Proat,
                    Proat.Cod_Attr_Qly_Proat,
                    Proat.Cod_Cmm_Qly_Proat,
                    Proat.Cod_Pvc_Proat,
                    Proat.Cod_Prtc_Rule_Proat,
                    Proat.Cod_Next_Use_Proat,
                    Proat.Cod_Next_User_Proat,
                    Proat.Tks_Prod_Proat,
                    Proat.Wid_Prod_Proat,
                    Proat.Lth_Prod_Proat,
                    Madat.Qty_Prod_Madat
                From 
                    Coa_Machine_Load_Cycles     Mlcyc,
                    Coa_Machine_Load_Areas_Viw  Mlare,
                    Coa_Assignment_Cycles      Asscyc,
                    Coa_Assignment_Machines_Viw Assma,
                    Coa_Machin_Datas            Madat,
                    Coa_Product_Attributes      Proat,
                    Coa.Coa_Possible_Products   Pospr --
                Where 
                    Mlcyc.Mlare_Machine_Load_Area_Id = Mlare.Machine_Load_Area_Id
                    And Asscyc.Mlcyc_Machine_Load_Cycle_Id = Mlcyc.Machine_Load_Cycle_Id
                    And Madat.Asscy_Assignment_Cycle_Id = Asscyc.Assignment_Cycle_Id
                    And Proat.Product_Attribute_Id = Asscyc.Proat_Product_Attribute_Id
                    And Assma.Assignment_Machine_Id = Madat.Assma_Assignment_Machine_Id
                    And Pospr.Proat_Product_Attribute_Id = Proat.Product_Attribute_Id 
                    And Asscyc.Pospr_Possible_Product_Id = Pospr.Possible_Product_Id 
                    And Pospr.Mlare_Machine_Load_Area_Id = {0}
                    And Assma.Ccntr_Cost_Center_Id = {1}
                    --And Pospr.Possible_Product_Id = 15640 ---for test must be deleted
                    --and Madat.Qty_Prod_Madat > 1
                Order By 
                    Proat.Cod_Prod_Proat,
                    Asscyc.Assignment_Cycle_Id,
                    Assma.Num_Seq_Assma,
                    Proat.Cod_Sha_Proat,
                    Proat.Cod_Typ_Proat,
                    Proat.Cod_Edge_Proat,
                    Proat.Cod_Sur_Proat,
                    Proat.Cod_Fla_Proat,
                    Proat.Cod_Ptc_Proat,
                    Proat.Cod_Tol_Proat,
                    Proat.Cod_Int_Qly_Proat,
                    Proat.Cod_Attr_Qly_Proat,
                    Proat.Cod_Cmm_Qly_Proat,
                    Proat.Cod_Pvc_Proat,
                    Proat.Cod_Prtc_Rule_Proat,
                    Proat.Cod_Next_Use_Proat,
                    Proat.Cod_Next_User_Proat,
                    Proat.Tks_Prod_Proat,
                    Proat.Wid_Prod_Proat,
                    Proat.Lth_Prod_Proat,
                    {machineLoadId},
                    {costCenterId};";

            dt = _workDatabase.GetDataTable(commandString, CommandType.Text);
            PublicData.NumPossibleProducts = dt.Rows.Count;
            foreach (DataRow dr in dt.Rows)
            {
                PublicData.PossibleProducts.Add(new PossibleProducts());
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].Index = PublicData.PossibleProducts.Count - 1;
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].Id = Convert.ToInt32(dr["Possible_Product_Id"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CostCenterId = Convert.ToInt32(dr["Ccntr_Cost_Center_Id"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].MachinDataId = Convert.ToInt32(dr["machin_data_id"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodProd = (dr["Cod_Prod_Pospr"]).ToString();
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodShape = Convert.ToInt32(dr["Cod_Sha_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodType = Convert.ToInt32(dr["Cod_Typ_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodEdge = Convert.ToInt32(dr["Cod_Edge_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodSurface = Convert.ToInt32(dr["Cod_Sur_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodRoughness = Convert.ToInt32(dr["Cod_Fla_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodProtection = Convert.ToInt32(dr["Cod_Ptc_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodTolerance = Convert.ToInt32(dr["Cod_Tol_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodInternalQuality = Convert.ToInt32(dr["Cod_Int_Qly_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodAttributeQuality = Convert.ToInt32(dr["Cod_Attr_Qly_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodCmmercialQuality = Convert.ToInt32(dr["Cod_Cmm_Qly_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodEnterPoint = (dr["Cod_Pvc_Proat"]).ToString();
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodPrticularRule = Convert.ToInt32(dr["Cod_Prtc_Rule_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodNextUse = Convert.ToInt32(dr["Cod_Next_Use_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].CodNextUser = Convert.ToInt32(dr["Cod_Next_User_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].ThiknessProd = Convert.ToDouble(dr["Tks_Prod_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].WidthProd = Convert.ToInt32(dr["Wid_Prod_Proat"]);
                PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].LengthProd = Convert.ToInt32(dr["Lth_Prod_Proat"]);
                if (dr["Qty_Prod_Madat"] != DBNull.Value)
                    PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].QtyProd = Convert.ToDouble(dr["Qty_Prod_Madat"]);
                else
                    PublicData.PossibleProducts[PublicData.PossibleProducts.Count - 1].QtyProd = 0;
            }
            PublicData.ChargingRatio = new double[PublicData.NumPossibleProducts];
            PublicData.Std = new double[PublicData.NumPossibleProducts];
        }
        public void GetCodDataTable(int costCenterCod)
        {
            DataTable dt = new DataTable();
            PublicData.TablesTechData = new List<TablesTechData>();
            string commandString = string.Format(@"select v.des_cod, v.cod_tchdt, v.cod_cc_ccntr 
                                                         from stg_tps_gbml_nam_cod_viw v 
                                                        where v.cod_cc_ccntr = '{0}' ", costCenterCod);

            dt = _workDatabase.GetDataTable(commandString, CommandType.Text);
            foreach (DataRow dr in dt.Rows)
            {
                PublicData.TablesTechData.Add(new TablesTechData());
                PublicData.TablesTechData[PublicData.TablesTechData.Count - 1].Index = PublicData.TablesTechData.Count - 1;
                PublicData.TablesTechData[PublicData.TablesTechData.Count - 1].CodTchdt = Convert.ToInt32(dr["cod_tchdt"]);
                PublicData.TablesTechData[PublicData.TablesTechData.Count - 1].Des = (dr["des_cod"]).ToString();
                PublicData.TablesTechData[PublicData.TablesTechData.Count - 1].CcCod = costCenterCod;
            }
        }
        public void GettechnicalData(int costCenterCod)
        {
            DataTable dt = new DataTable();
            PublicData.TechnicalData = new List<TechnicalData>();
            string commandString = $@"
                select cod_cc_ccntr, --کد مرکز هزينه
                  des_cc_ccntr, -- شرح مرکز هزينه
                  cod_tchdt, -- نوع داده فني
                  num_user_tchdt, -- شماره کاربري
                  num_coef_tchdt, -- ضريب
                  des_cod_tchdt, --شرح داده فني
                  cod_prod_proat, -- کد محصول
                  -- مشخصه هاي محصول--
                  cod_sha_proat,
                  cod_typ_proat,
                  cod_edge_proat,
                  cod_sur_proat,
                  cod_fla_proat,
                  cod_ptc_proat,
                  cod_tol_proat,
                  cod_int_qly_proat,
                  cod_attr_qly_proat,
                  cod_cmm_qly_proat,
                  cod_pvc_proat,
                  cod_prtc_rule_proat,
                  cod_next_use_proat,
                  cod_next_user_proat,
                  tks_prod_proat,
                  wid_prod_proat,
                  lth_prod_proat
                from coa_technical_datas_viw v
                where 
                    to_char(v.Dat_End_Tchdt, 'rrrr') = '{PublicData.ValYear}'
                    and v.Num_Ver_Tchdt = {PublicData.NumVersion}
                    and v.cod_cc_ccntr ='{costCenterCod}'
                order by 
                    v.cod_tchdt,
                    v.cod_sha_proat,
                    v.cod_typ_proat,
                    v.cod_edge_proat,
                    v.cod_sur_proat,
                    v.cod_fla_proat,
                    v.cod_ptc_proat,
                    v.cod_tol_proat,
                    v.cod_int_qly_proat,
                    v.cod_attr_qly_proat,
                    v.cod_cmm_qly_proat,
                    v.cod_pvc_proat,
                    v.cod_prtc_rule_proat,
                    v.cod_next_use_proat,
                    v.cod_next_user_proat,
                    v.tks_prod_proat,
                    v.wid_prod_proat,
                    v.lth_prod_proat";

            dt = _workDatabase.GetDataTable(commandString, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
            {
                PublicData.TechnicalData.Add(new TechnicalData());
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].Index = PublicData.TechnicalData.Count - 1;
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodTchdt = Convert.ToInt32(dr["cod_tchdt"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CcCod = costCenterCod;
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].NumUser = Convert.ToInt32(dr["num_user_tchdt"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].NumCoef = Convert.ToDouble(dr["num_coef_tchdt"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodShape = Convert.ToInt32(dr["Cod_Sha_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodType = Convert.ToInt32(dr["Cod_Typ_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodEdge = Convert.ToInt32(dr["Cod_Edge_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodSurface = Convert.ToInt32(dr["Cod_Sur_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodRoughness = Convert.ToInt32(dr["Cod_Fla_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodProtection = Convert.ToInt32(dr["Cod_Ptc_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodTolerance = Convert.ToInt32(dr["Cod_Tol_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodInternalQuality = Convert.ToInt32(dr["Cod_Int_Qly_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodAttributeQuality = Convert.ToInt32(dr["Cod_Attr_Qly_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodCmmercialQuality = Convert.ToInt32(dr["Cod_Cmm_Qly_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodEnterPoint = dr["Cod_Pvc_Proat"].ToString();
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodPrticularRule = Convert.ToInt32(dr["Cod_Prtc_Rule_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodNextUse = Convert.ToInt32(dr["Cod_Next_Use_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].CodNextUser = Convert.ToInt32(dr["Cod_Next_User_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].TksProd = Convert.ToDouble(dr["Tks_Prod_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].WidthProd = Convert.ToInt32(dr["Wid_Prod_Proat"]);
                PublicData.TechnicalData[PublicData.TechnicalData.Count - 1].LengthProd = Convert.ToInt32(dr["Lth_Prod_Proat"]);
            }
        }
        public void GetNumFormulas(CostCenter costCenter)
        {
            string commandString = string.Format($@"
                select distinct t.cod_cc_ccntr,
                    t.num_dir_mat_ccntr,
                    t.num_std_prod_ccntr,
                    t.num_dim_change_ccntr
                from 
                    COA.COA_COST_CENTERS t,
                    COA.COA_ASSIGNMENT_MACHINES a,
                    COA.COA_MACHINE_LOAD_CYCLES c,
                    Coa_Machine_Load_Areas M1
                where 
                    c.mlare_machine_load_area_id = m1.machine_load_area_id
                    and a.mlcyc_machine_load_cycle_id = c.machine_load_cycle_id
                    and t.cost_center_id = a.ccntr_cost_center_id
                    and t.cost_center_id = {0}
                    and t.flg_virt_ccntr is null
                    order by t.cod_cc_ccntr,
                    {costCenter.Id}");

            DataTable dt = _workDatabase.GetDataTable(commandString, CommandType.Text);

            PublicData.NumberFormulas = new List<FormulaNumber>();
            foreach (DataRow dr in dt.Rows)
            {
                PublicData.NumberFormulas.Add(new FormulaNumber());
                PublicData.NumberFormulas[PublicData.NumberFormulas.Count - 1].Index = PublicData.NumberFormulas.Count - 1;
                PublicData.NumberFormulas[PublicData.NumberFormulas.Count - 1].CcCod = costCenter.Code;

                if (dr["Num_Dir_Mat_Ccntr"] != DBNull.Value)
                {
                    PublicData.NumberFormulas[PublicData.NumberFormulas.Count - 1].NumDirMat = Convert.ToInt32(dr["Num_Dir_Mat_Ccntr"]);
                }
                else
                {
                    PublicData.NumberFormulas[PublicData.NumberFormulas.Count - 1].NumDirMat = -1;
                }
                if (dr["Num_Std_Prod_Ccntr"] != DBNull.Value)
                {
                    PublicData.NumberFormulas[PublicData.NumberFormulas.Count - 1].NumStdProd = Convert.ToInt32(dr["Num_Std_Prod_Ccntr"]);
                }
                else
                {
                    PublicData.NumberFormulas[PublicData.NumberFormulas.Count - 1].NumStdProd = -1;
                }
                if (dr["Num_Dim_Change_Ccntr"] != DBNull.Value)
                {
                    PublicData.NumberFormulas[PublicData.NumberFormulas.Count - 1].NumDimChange = Convert.ToInt32(dr["Num_Dim_Change_Ccntr"]);
                }
                else
                {
                    PublicData.NumberFormulas[PublicData.NumberFormulas.Count - 1].NumDimChange = -1;
                }
            }
        }
        public void GetferroalloysData(int costCenterId)
        {
            PublicData.Ferroalloys = new List<Ferroalloys>();
            string commandString = $@"
                Select 
                    Fer.Expit_Expense_Item_Id,
                    Fer.Num_Coef_Fraly,
                    Fer.Cod_Int_Qly_Fraly,
                    Fer.Lkp_Typ_Fraly,
                    Fer.Ccntr_Cost_Center_Id
                From Coa_ferroalloys Fer
                Where 
                    Fer.Num_Ver_Fraly = {PublicData.NumVersion}
                    And To_Char(Fer.Dat_End_Fraly, 'rrrr') ='{PublicData.ValYear}'
                    And fer.ccntr_cost_center_id = {costCenterId}
                order by 
                    Fer.Cod_Int_Qly_Fraly";

            DataTable dt = _workDatabase.GetDataTable(commandString, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
            {
                PublicData.Ferroalloys.Add(new Ferroalloys());
                PublicData.Ferroalloys[PublicData.Ferroalloys.Count - 1].Index = PublicData.Ferroalloys.Count - 1;
                PublicData.Ferroalloys[PublicData.Ferroalloys.Count - 1].ExpitExpenseId = Convert.ToInt32(dr["Expit_Expense_Item_Id"]);
                PublicData.Ferroalloys[PublicData.Ferroalloys.Count - 1].NumCoef = Convert.ToDouble(dr["Num_Coef_Fraly"]);
                PublicData.Ferroalloys[PublicData.Ferroalloys.Count - 1].CodIntQly = Convert.ToInt32(dr["Cod_Int_Qly_Fraly"]);
                PublicData.Ferroalloys[PublicData.Ferroalloys.Count - 1].CostCenterId = Convert.ToInt32(dr["Ccntr_Cost_Center_Id"]);
                PublicData.Ferroalloys[PublicData.Ferroalloys.Count - 1].LkpTyp = Convert.ToInt32(dr["Lkp_Typ_Fraly"]);
            }
        }
        public void GetextractAssignExpit(int costCenterId)
        {
            PublicData.ExtractAssign = new List<ExtractAssign>();
            string commandString = $@"
                Select Assex.Expit_Expense_Item_Id, Cod_Pos_Assex
                From Coa.Coa_Assign_Expense_Items Assex
                Where To_Char(Assex.Dat_End_Assex, 'rrrr') = '{PublicData.ValYear}'
                And Assex.Num_Ver_Assex = {PublicData.NumVersion}
                and Ccntr_Cost_Center_Id = {costCenterId}
                And Assex.Cod_Int_Qly_Assex = 9999";

            DataTable dt = _workDatabase.GetDataTable(commandString, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
            {
                PublicData.ExtractAssign.Add(new ExtractAssign());
                PublicData.ExtractAssign[PublicData.ExtractAssign.Count - 1].Index = PublicData.ExtractAssign.Count - 1;
                PublicData.ExtractAssign[PublicData.ExtractAssign.Count - 1].ExpitExpenseId = Convert.ToInt32(dr["Expit_Expense_Item_Id"]);
                PublicData.ExtractAssign[PublicData.ExtractAssign.Count - 1].CostCenterId = costCenterId;
                PublicData.ExtractAssign[PublicData.ExtractAssign.Count - 1].CodPos = Convert.ToInt32(dr["Cod_Pos_Assex"]);

            }
        }
        public void GetmachineData(int activeMachinId)
        {
            PublicData.MachineData = new List<MachineData>();
            string commandString = $@"
                Select Madat.Machin_Data_Id,
                Madat.Qty_Year_Pospr,
                Madat.Qty_Chrg_Madat,
                Madat.flg_Prtp_Madat,
                Madat.qty_Prod_Madat
                From Coa_Machin_Datas_Viw Madat
                Where Madat.Mlare_Machine_Load_Area_Id = {activeMachinId}";

            DataTable dt = _workDatabase.GetDataTable(commandString, CommandType.Text);

            foreach (DataRow dr in dt.Rows)
            {
                PublicData.MachineData.Add(new MachineData());
                PublicData.MachineData[PublicData.MachineData.Count - 1].Index = PublicData.MachineData.Count - 1;
                PublicData.MachineData[PublicData.MachineData.Count - 1].MachinDataId = Convert.ToInt32(dr["Machin_Data_Id"]);
                if (dr["Qty_Year_Pospr"] != DBNull.Value)
                    PublicData.MachineData[PublicData.MachineData.Count - 1].QtyYear = Convert.ToDouble(dr["Qty_Year_Pospr"]);
                else
                    PublicData.MachineData[PublicData.MachineData.Count - 1].QtyYear = 0;
                if (dr["Qty_Chrg_Madat"] != DBNull.Value)
                    PublicData.MachineData[PublicData.MachineData.Count - 1].QtyChrg = Convert.ToDouble(dr["Qty_Chrg_Madat"]);
                else
                    PublicData.MachineData[PublicData.MachineData.Count - 1].QtyChrg = 0;
                PublicData.MachineData[PublicData.MachineData.Count - 1].FlgPrtp = Convert.ToInt32(dr["flg_Prtp_Madat"]);
                PublicData.MachineData[PublicData.MachineData.Count - 1].QtyProd = Convert.ToDouble(dr["flg_Prtp_Madat"]);

            }
        }
        public void InsertError(int codError, int codCcntr, string desCcntr, int coopsStatusId)
        {
            _workDatabase.ExecuteCommand(@"
                Insert Into Coa_Operation_Errors
                    (Operation_Error_Id,
                    Num_Err_Operr,
                    Coops_Operation_Status_Id,
                    Des_Att1_Operr,
                    Cod_Att1_Operr,
                    Des_Att2_Operr,
                    Cod_Att2_Operr,
                    Des_Att3_Operr,
                    Cod_Att3_Operr,
                    Des_Att10_Operr,
                    Typ_Err_Operr)
                Values
                    (Coa_Operation_Errors_Seq.Nextval,
                    {0},
                    {1},
                    {2},
                    'مرکز هزينه',
                    '{3}',
                    'شرح',
                    (Select Mlare.Cod_Mlare_Parent || Mlare.Cod_Mlare
                     From Coa_Machine_Load_Areas_Viw Mlare
                    Where Mlare.Flg_Sta_Mlare = 1),
                    'ناحيه-زير ناحيه',
                    (Select Mlare.Machine_Load_Area_Id
                     From Coa_Machine_Load_Areas Mlare
                    Where Mlare.Flg_Sta_Mlare = 1),
                    1)", codError,
                       coopsStatusId,
                       codCcntr,
                       desCcntr,
                       CommandType.Text);
        }

    }
}
