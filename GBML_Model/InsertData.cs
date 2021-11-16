using System;
using System.Collections;
using System.IO;
using DataLayer;
using System.Linq;

namespace GBML_Model
{
    class InsertData
    {
        public void InsertResults(ArrayList expenseItems)
        {
            StreamWriter writeDateTime = new StreamWriter(PublicData.InputPath + "\\inserResults_" + (DateTime.Now.ToString($"yyyyMMddHH_mmss")) + ".txt");
            DateTime startTot, endTot;
            startTot = DateTime.Now;
            InsertRecupExp(expenseItems);
            endTot = DateTime.Now;
            writeDateTime.WriteLine("InsertRecupExp:" + (endTot - startTot).TotalSeconds);
            startTot = DateTime.Now;
            UpdateRecupExp();
            endTot = DateTime.Now;
            writeDateTime.WriteLine("UpdateRecupExp:" + (endTot - startTot).TotalSeconds);
            startTot = DateTime.Now;
            UpdateMachinData();
            endTot = DateTime.Now;
            writeDateTime.WriteLine("UpdateMachinData:" + (endTot - startTot).TotalSeconds);
            startTot = DateTime.Now;
            UpdateQtyStdMadat();
            endTot = DateTime.Now;
            writeDateTime.WriteLine("UpdateQtyStdMadat:" + (endTot - startTot).TotalSeconds);
            writeDateTime.Close();
        }
        public void InsertPcnResult(ActiveMachine activeMachin, string type)
        {
            if (type == "PCN_ALT_MAC")
                UpdatePcnAlternative(activeMachin.Id, type, activeMachin.LkpGrpAlt.ToString());
            else
            {
                UpdatePcnAlternative(activeMachin.Id, type, activeMachin.LkpGrpAlt.ToString());
                UpdateRecupExp_ALt(activeMachin.Id, activeMachin.LkpGrpAlt.ToString());
                DeleteRecupExp_ALt(activeMachin.Id, activeMachin.LkpGrpAlt.ToString());
                InsertGbmlRuns(activeMachin.Id, activeMachin.LkpGrpAlt.ToString());
                UpdateMachinData_Qty(activeMachin.LkpGrpAlt.ToString());
                if (activeMachin.LkpGrpAlt != 40)
                    UpdateChargeProd(activeMachin.CodeSubArea);
                UpdateQtyPossibleProd();
            }
        }
        private void InsertRecupExp(ArrayList expenseItems)
        {
            if (expenseItems.Count == 0)
                return;
            Database clsData = new Database(PublicData.ConnectionServer);

            ArrayList expItemId = new ArrayList();
            ArrayList qtyBsstd = new ArrayList();
            ArrayList machinDataId = new ArrayList();
            foreach (ArrayList item in expenseItems)
            {
                expItemId.Add(item[2]);
                machinDataId.Add(item[3]);
                qtyBsstd.Add(item[4]);
            }
            int maxRecord = expItemId.Count;
            clsData.ExecuteCommand(@"begin
                                        apps.App_Stg_Tps_Gbml_Model_Pkg.delete_recup_Exp_prc(
                                           p_Exp_Item_Id => :p_Exp_Item_Id,
                                           p_Machin_Data_Id => :p_Machin_Data_Id);
                                     end;",
                maxRecord,
                expItemId.ToArray(typeof(int)) as int[],
                machinDataId.ToArray(typeof(int)) as int[]);
            clsData.ExecuteCommand(@"begin
                                        apps.App_Stg_Tps_Gbml_Model_Pkg.insert_recup_Exp_prc(
                                           p_Exp_Item_Id => :p_Exp_Item_Id,
                                           p_Qty_Bsstd => :p_Qty_Bsstd,
                                           p_Machin_Data_Id => :p_Machin_Data_Id);
                                     end;",
                maxRecord,
                expItemId.ToArray(typeof(int)) as int[],
                qtyBsstd.ToArray(typeof(double)) as double[],
                machinDataId.ToArray(typeof(int)) as int[]);
        }
        private void UpdateMachinData()
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            ArrayList qtyStdBasic = new ArrayList();
            ArrayList qtyStdHour = new ArrayList();
            ArrayList machinDataId = new ArrayList();
            ArrayList tksProd = new ArrayList();
            ArrayList widProd = new ArrayList();
            ArrayList lthProd = new ArrayList();
            ArrayList possibleProductId = new ArrayList();
            ArrayList codProdFthr = new ArrayList();
            foreach (PossibleProducts item in PublicData.PossibleProducts)
            {
                qtyStdBasic.Add(PublicData.ChargingRatio[item.Index]);
                qtyStdHour.Add(PublicData.Std[item.Index]);
                machinDataId.Add(item.MachinDataId);
                tksProd.Add(item.ThiknessProdNew);
                widProd.Add(item.WidthProdNew);
                lthProd.Add(item.LengthProdNew);
                possibleProductId.Add(item.Id);
                codProdFthr.Add(item.CodProdFather);
            }
            int maxRecord = machinDataId.Count;
            if (maxRecord > 0)
            {
                clsData.ExecuteCommand(@"begin
                                     apps.App_Stg_Tps_Gbml_Model_Pkg.update_machin_data_prc(
                                                    p_machin_data_id => :p_machin_data_id,
                                                    p_Qty_Std_Basic  => :p_Qty_Std_Basic,
                                                    p_qty_std_hour   => :p_qty_std_hour,
                                                    p_Tks_Prod       => :p_Tks_Prod,
                                                    p_Wid_Prod       => :p_Wid_Prod,
                                                    p_Lth_Prod       => :p_Lth_Prod);
                                     end;",
                    maxRecord,
                    machinDataId.ToArray(typeof(int)) as int[],
                    qtyStdBasic.ToArray(typeof(double)) as double[],
                    qtyStdHour.ToArray(typeof(double)) as double[],
                    tksProd.ToArray(typeof(double)) as double[],
                    widProd.ToArray(typeof(int)) as int[],
                    lthProd.ToArray(typeof(int)) as int[]
                );

                clsData.ExecuteCommand(@"begin
                                     apps.App_Stg_Tps_Gbml_Model_Pkg.update_possible_Products_prc(
                                                    p_Possible_Product_Id => :p_Possible_Product_Id,
                                                    p_Cod_Prod_Fthr => :p_Cod_Prod_Fthr);
                                     end;",
                    maxRecord,
                    possibleProductId.ToArray(typeof(int)) as int[],
                    codProdFthr.ToArray(typeof(string)) as string[]);
            }
        }
        public void UpdateRecupExp()
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            ArrayList machinDataId = new ArrayList();
            ArrayList qtyYear = new ArrayList();
            foreach (MachineData item in PublicData.MachineData)
            {
                machinDataId.Add(item.MachinDataId);
                qtyYear.Add(item.QtyYear);
            }
            int maxRecord = machinDataId.Count;
            if (maxRecord == 0)
                return;
            clsData.ExecuteCommand(@"
                   begin
                       apps.App_Stg_Tps_Gbml_Model_Pkg.update_recup_Exp_prc(
                       p_machin_data_id => :p_machin_data_id,
                       p_qty_year => :p_qty_year);
                   end;",
                maxRecord,
                machinDataId.ToArray(typeof(int)) as int[],
                qtyYear.ToArray(typeof(double)) as double[]
            );
        }
        public void UpdateVirtMachineData()
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            int costCenterId = 0;
            foreach (var item in PublicData.CostCenter)
                costCenterId = (item.FlagVirtual == 1) ? item.Id : 0;
            if (costCenterId != 0)
                clsData.ExecuteCommand(@"begin
                                        apps.App_Stg_Tps_Gbml_Model_Pkg.update_virt_Machin_Datas_prc();
                                     end;");
        }
        public void UpdateQtyStdMadat()
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            ArrayList qtyYear = new ArrayList();
            ArrayList possibleProductId = new ArrayList();
            foreach (PossibleProducts item in PublicData.PossibleProducts)
            foreach (MachineData data in PublicData.MachineData)
                if (data.MachinDataId == item.MachinDataId)
                {
                    possibleProductId.Add(item.Id);
                    qtyYear.Add(data.QtyYear);
                }
            int maxRecord = possibleProductId.Count;
            if (maxRecord == 0)
                return;
            clsData.ExecuteCommand(@"begin
                                        apps.App_Stg_Tps_Gbml_Model_Pkg.update_Qty_Std_Madat_prc(
                                            p_Qty_Year => :p_Qty_Year,
                                            p_Possible_Product_Id => :p_Possible_Product_Id);
                                     end;",
                maxRecord,
                qtyYear.ToArray(typeof(double)) as double[],
                possibleProductId.ToArray(typeof(int)) as int[]
            );
        }
        public void DeleteGbmlRuns(string lkpGrp)
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            clsData.ExecuteCommand("Delete From Coa.Coa_Gbml_Runs r Where r.Lkp_Grp_Alt_Gbmlr = '" + lkpGrp + "'");
        }
        private void UpdatePcnAlternative(int activeMachinId, string type, string lkpGroup)
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            ArrayList flgAlt = new ArrayList();
            ArrayList pcn = new ArrayList();
            ArrayList pcnPalma = new ArrayList();
            ArrayList ccntr = new ArrayList();
            ArrayList ccntrPalma = new ArrayList();
            ArrayList activeMachin = new ArrayList();
            ArrayList typeArray = new ArrayList();
            ArrayList lkpGroups = new ArrayList();

            foreach (AlternativeMachine item in PublicData.AlternativeMachine)
            {
                PcnCostCenter cost = PublicData.PcnCostCenter.Where(a => a.CostCenterId == item.CostCenterId).FirstOrDefault();
                flgAlt.Add(PublicData.FlgPcnCostCenter);
                pcn.Add(item.Pcn);
                pcnPalma.Add(cost.Pcn);
                ccntr.Add(item.CostCenterId);
                ccntrPalma.Add(cost.CostCenterId);
                activeMachin.Add(activeMachinId);
                typeArray.Add(type);
                lkpGroups.Add(lkpGroup);
            }
            int maxRecord = flgAlt.Count;
            clsData.ExecuteCommand(@"begin
                                        apps.App_Stg_Tps_Gbml_Model_Pkg.update_Pcn_Alternative_prc(
                                            p_Flg            => :p_Flg           ,
                                            p_Pcn            => :p_Pcn           ,
                                            p_pcn_palma      => :p_pcn_palma     ,
                                            p_Ccntr_id       => :p_Ccntr_id      ,
                                            p_Ccntr_Id_palma => :p_Ccntr_Id_palma,
                                            p_Machine_Load_Area_Id => :p_Machine_Load_Area_Id,
                                            p_typ => :p_typ,
                                            p_Lkp_group => :p_Lkp_group);
                                     end;",
                maxRecord,
                flgAlt.ToArray(typeof(int)) as int[],
                pcn.ToArray(typeof(double)) as double[],
                pcnPalma.ToArray(typeof(double)) as double[],
                ccntr.ToArray(typeof(int)) as int[],
                ccntrPalma.ToArray(typeof(int)) as int[],
                activeMachin.ToArray(typeof(int)) as int[],
                typeArray.ToArray(typeof(string)) as string[],
                lkpGroups.ToArray(typeof(string)) as string[]
            );
        }
        private void UpdateRecupExp_ALt(int activeMachinId, string lkpGroup)
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            ArrayList machinDataId = new ArrayList();
            ArrayList qtyProd = new ArrayList();
            ArrayList activeMachin = new ArrayList();
            ArrayList lkpGroups = new ArrayList();
            foreach (MachineData item in PublicData.MachineData)
                if (item.FlgPrtp == 2 && item.QtyProd != 0)
                {
                    machinDataId.Add(item.MachinDataId);
                    qtyProd.Add(item.QtyProd);
                    activeMachin.Add(activeMachinId);
                    lkpGroups.Add(lkpGroup);
                }
            int maxRecord = activeMachin.Count;
            clsData.ExecuteCommand(@"begin
                                        apps.App_Stg_Tps_Gbml_Model_Pkg.update_Recup_Exp_Alt_prc(
                                                p_Qty_Prod_Madat => :p_Qty_Prod_Madat,
                                                p_Machin_Data_Id => :p_Machin_Data_Id,
                                                p_Machine_Load_Area_Id => :p_Machine_Load_Area_Id,
                                                p_lkp_Group => :p_lkp_Group);
                                     end;",
                maxRecord,
                machinDataId.ToArray(typeof(int)) as int[],
                qtyProd.ToArray(typeof(double)) as double[],
                activeMachin.ToArray(typeof(int)) as int[],
                lkpGroups.ToArray(typeof(string)) as string[]
            );
        }
        private void DeleteRecupExp_ALt(int activeMachinId, string lkpGroup)
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            clsData.ExecuteCommand(
                "begin apps.App_Stg_Tps_Gbml_Model_Pkg.delete_Recup_Exp_Alt_prc(p_Machine_Load_Area_Id => "
                + activeMachinId + ", p_Lkp_Group => " + lkpGroup + "); end;");
        }
        private void InsertGbmlRuns(int activeMachinId, string lkpGroup)
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            ArrayList flg = new ArrayList();
            ArrayList pcn = new ArrayList();
            ArrayList pcnPalma = new ArrayList();
            ArrayList ccntrId = new ArrayList();
            ArrayList ccntrIdPalma = new ArrayList();
            ArrayList machineLoadAreaId = new ArrayList();
            ArrayList maxHour = new ArrayList();
            ArrayList obligateHour = new ArrayList();
            ArrayList obligateCharge = new ArrayList();
            ArrayList remainHour2 = new ArrayList();
            ArrayList numSeq = new ArrayList();
            ArrayList freeQty2 = new ArrayList();
            ArrayList freeHour = new ArrayList();
            ArrayList freeCharge = new ArrayList();
            ArrayList lkp = new ArrayList();
            foreach (AlternativeMachine item in PublicData.AlternativeMachine)
            {
                PcnCostCenter cost = PublicData.PcnCostCenter.Where(a => a.CostCenterId == item.CostCenterId).FirstOrDefault();
                flg.Add(PublicData.FlgPcnCostCenter);
                pcn.Add(item.Pcn);
                pcnPalma.Add(cost.Pcn);
                ccntrId.Add(item.CostCenterId);
                ccntrIdPalma.Add(cost.CostCenterId);
                machineLoadAreaId.Add(activeMachinId);
                maxHour.Add(item.MaxHour);
                obligateHour.Add(item.ObligateHour);
                obligateCharge.Add(item.ObligateCharge);
                remainHour2.Add(item.RemainHour2);
                numSeq.Add(item.NumSeq);
                freeQty2.Add(PublicData.FreeQty2);
                freeHour.Add(item.FreeHour);
                freeCharge.Add(item.FreeCharge);
                lkp.Add(lkpGroup.ToString());
            }
            int maxRecord = machineLoadAreaId.Count;
            clsData.ExecuteCommand(@"begin
                                        apps.App_Stg_Tps_Gbml_Model_Pkg.insert_Gbml_Runs_prc(
                                     p_Flg                  => :p_Flg                 ,
                                     p_Pcn                  => :p_Pcn                 ,
                                     p_pcn_palma            => :p_pcn_palma           ,
                                     p_Ccntr_id             => :p_Ccntr_id            ,
                                     p_Ccntr_Id_palma       => :p_Ccntr_Id_palma      ,
                                     p_Machine_Load_Area_Id => :p_Machine_Load_Area_Id,
                                     p_Max_Hour             => :p_Max_Hour            ,
                                     p_Obligate_Hour        => :p_Obligate_Hour       ,
                                     p_Obligate_Charge      => :p_Obligate_Charge     ,
                                     p_Remain_Hour_2        => :p_Remain_Hour_2       ,
                                     p_num_seq              => :p_num_seq             ,
                                     p_Free_Qty_2           => :p_Free_Qty_2          ,
                                     p_Free_Hour            => :p_Free_Hour           ,
                                     p_Free_Charge          => :p_Free_Charge         ,
                                     p_Lkp_Group            => :p_Lkp_Group);
                                     end;",
                maxRecord,
                flg.ToArray(typeof(int)) as int[],
                pcn.ToArray(typeof(double)) as double[],
                pcnPalma.ToArray(typeof(double)) as double[],
                ccntrId.ToArray(typeof(int)) as int[],
                ccntrIdPalma.ToArray(typeof(int)) as int[],
                machineLoadAreaId.ToArray(typeof(int)) as int[],
                maxHour.ToArray(typeof(double)) as double[],
                obligateHour.ToArray(typeof(double)) as double[],
                obligateCharge.ToArray(typeof(double)) as double[],
                remainHour2.ToArray(typeof(double)) as double[],
                numSeq.ToArray(typeof(int)) as int[],
                freeQty2.ToArray(typeof(double)) as double[],
                freeHour.ToArray(typeof(double)) as double[],
                freeCharge.ToArray(typeof(double)) as double[],
                lkp.ToArray(typeof(string)) as string[]
            );
        }
        private void UpdateMachinData_Qty(string lkpGroup)
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            ArrayList machinDataId = new ArrayList();
            ArrayList lkp = new ArrayList();
            foreach (MachineData item in PublicData.MachineData)
            {
                machinDataId.Add(item.MachinDataId);
                lkp.Add(lkpGroup);
            }
            int maxRecord = machinDataId.Count;
            clsData.ExecuteCommand(@"begin
                                        apps.App_Stg_Tps_Gbml_Model_Pkg.update_Machine_Data_Qty_prc(
                                                p_Machin_Data_Id => :p_Machin_Data_Id,
                                                p_Lkp_Group => :p_Lkp_Group);
                                     end;",
                maxRecord,
                machinDataId.ToArray(typeof(int)) as int[],
                lkp.ToArray(typeof(string)) as string[]);
        }
        private void UpdateQtyPossibleProd()
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            ArrayList possibleProductId = new ArrayList();
            ArrayList qtyProd = new ArrayList();
            ArrayList qtyChrg = new ArrayList();
            foreach (ResultQtyCharge item in PublicData.ResultQtyCharge)
            {
                possibleProductId.Add(item.PossibleProductId);
                qtyProd.Add(Math.Round(item.QtyProd, 3));
                qtyChrg.Add(Math.Round(item.QtyChrg, 3));
            }
            int maxRecord = possibleProductId.Count;
            clsData.ExecuteCommand(@"begin
                                        apps.App_Stg_Tps_Gbml_Model_Pkg.update_Qty_Possible_Prod_prc(
                                            p_possible_product_id => :p_possible_product_id,
                                            p_qty_prod => :p_qty_prod,
                                            p_Qty_Chrg => :p_Qty_Chrg);
                                     end;",
                maxRecord,
                possibleProductId.ToArray(typeof(int)) as int[],
                qtyProd.ToArray(typeof(double)) as double[],
                qtyChrg.ToArray(typeof(double)) as double[]);
        }
        private void UpdateChargeProd(String subArea)
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            clsData.ExecuteCommand(@"begin
                                        apps.App_Stg_Tps_Gbml_Model_Pkg.update_Upd_Chrg_Prod_prc
                                        (p_cod_subArea => " + subArea + "); end;");
        }
        private void UpdateNum_Cont(int machineLoadAreaId)
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            clsData.ExecuteCommand(@"begin
                                        apps.App_Stg_Tps_Gbml_Model_Pkg.update_Num_Cont_prc(
                                                p_Machine_Load_Area_Id => " + machineLoadAreaId + "); end;");
        }
        public void InsertProductAttributes()
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            ArrayList codFather = new ArrayList();
            foreach (var item in PublicData.PossibleProducts)
                if (item.FlagFoundFather)
                    codFather.Add(item.CodProdFather);
            int maxRecord = codFather.Count;
            clsData.ExecuteCommand(@"
                begin
                    apps.App_Stg_Tps_Gbml_Model_Pkg.update_Product_Attributes_prc(
                        p_cod_father => :p_cod_father);
                end;",
                maxRecord,
                codFather.ToArray(typeof(string)) as string[],
                null);
        }
        public void InsertTransportProducts()
        {
            Database clsData = new Database(PublicData.ConnectionServer);
            ArrayList codFather = new ArrayList();
            ArrayList costCenterId = new ArrayList();
            ArrayList codExit = new ArrayList();
            foreach (var item in PublicData.PossibleProducts)
                if (!item.FlagFoundFather)
                {
                    codExit.Add(item.CodExit);
                    codFather.Add(item.CodProdFather);
                    costCenterId.Add(item.CostCenterId);
                }
            int maxRecord = codFather.Count;
            clsData.ExecuteCommand(@"
                begin
                    apps.App_Stg_Tps_Gbml_Model_Pkg.update_Trans_prod_prc
                        (p_Cost_Center_Id => :p_Cost_Center_Id,
                        p_Cod_Prod_Fthr   => :p_Cod_Prod_Fthr,
                        p_Cod_Ext         => :p_Cod_Ext);
                end;",
                maxRecord,
                costCenterId.ToArray(typeof(int)) as int[],
                codFather.ToArray(typeof(string)) as string[],
                codExit.ToArray(typeof(string)) as string[]);
        }
    }
}