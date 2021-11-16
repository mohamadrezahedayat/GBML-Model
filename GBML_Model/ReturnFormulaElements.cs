using System;
using System.Collections;
using System.Data;
using System.Linq;
using DataLayer;

namespace GBML_Model
{
    public class ReturnFormulaElements
    {
        public void ReturnElements(PossibleProducts prod)
        {
            CostCenter cost = new CostCenter();
            cost = PublicData.CostCenter.Where(d => d.Id == prod.CostCenterId).FirstOrDefault();
            PublicData.FormulaParameters = new FormulaParameters();

            foreach (TablesTechData table in PublicData.TablesTechData)
            {
                table.FlgFound = false;
                foreach (TechnicalData tech in PublicData.TechnicalData)
                    if (cost.Code == tech.CcCod)
                        if (tech.CodTchdt == table.CodTchdt)
                            if (prod.CodShape <= tech.CodShape &&
                                prod.CodType <= tech.CodType &&
                                prod.CodEdge <= tech.CodEdge &&
                                prod.CodSurface <= tech.CodSurface &&
                                prod.CodRoughness <= tech.CodRoughness &&
                                prod.CodProtection <= tech.CodProtection &&
                                prod.CodTolerance <= tech.CodTolerance &&
                                prod.CodInternalQuality <= tech.CodInternalQuality &&
                                prod.CodAttributeQuality <= tech.CodAttributeQuality &&
                                prod.CodCmmercialQuality <= tech.CodCmmercialQuality &&
                                Convert.ToInt32(prod.CodEnterPoint) <= Convert.ToInt32(tech.CodCmmercialQuality) &&
                                prod.CodPrticularRule <= tech.CodPrticularRule &&
                                prod.CodNextUse <= tech.CodNextUse &&
                                prod.CodNextUser <= tech.CodNextUser &&
                                prod.ThiknessProd <= tech.TksProd &&
                                prod.WidthProd <= tech.WidthProd &&
                                prod.LengthProd <= tech.LengthProd)
                            {
                                table.NumCoef = tech.NumCoef;
                                table.FlgFound = true;
                                break;
                            }
            }
            PublicData.FormulaParameters.Tk = prod.ThiknessProd;
            PublicData.FormulaParameters.Wd = prod.WidthProd;
            PublicData.FormulaParameters.Lg = prod.LengthProd;
        }
        public double ReturnElements_eaf(ref ArrayList expenseItems, CostCenter costCenter, PossibleProducts prod, int codData, double k1, double k2, double k3, int flg201)
        {
            bool foundIntQly = false;
            double numCoef = 0;
            double totNumCoef = 0;

            foreach (Ferroalloys ferr in PublicData.Ferroalloys)
                if (ferr.LkpTyp == codData)
                    if (prod.CodInternalQuality == ferr.CodIntQly)
                    {
                        foundIntQly = true;
                        if (codData == 201 && flg201 == 1)
                        {
                            numCoef = ferr.NumCoef - (k1 * k3);
                            totNumCoef = totNumCoef + (ferr.NumCoef - (k1 * k3));
                        }
                        else if (codData == 201 && flg201 == 0)
                        {
                            numCoef = ferr.NumCoef;
                            totNumCoef = totNumCoef + (ferr.NumCoef);
                        }
                        if (codData == 202)
                        {
                            numCoef = ferr.NumCoef - (k2 * k3);
                            totNumCoef = totNumCoef + (ferr.NumCoef - (k2 * k3));
                        }
                        if (codData == 203)
                        {
                            numCoef = ferr.NumCoef;
                            totNumCoef = totNumCoef + (ferr.NumCoef);
                        }
                        if (codData == 204)
                        {
                            numCoef = ferr.NumCoef * k3;
                            totNumCoef = totNumCoef + (ferr.NumCoef * k3);
                        }
                        if (codData == 205)
                        {
                            numCoef = ferr.NumCoef;
                            totNumCoef = totNumCoef + (ferr.NumCoef);
                        }
                        expenseItems.Add(ExpenseItem(1, 0, costCenter.Id, prod, ferr.ExpitExpenseId, numCoef, prod.CodInternalQuality, codData));
                    }
            return totNumCoef;
        }
        public void ReturnElements_ass(PossibleProducts prod)
        {
            CostCenter cost = new CostCenter();
            cost = PublicData.CostCenter.Where(d => d.Id == prod.CostCenterId).FirstOrDefault();
            PublicData.FormulaParameters = new FormulaParameters();

            foreach (TablesTechData table in PublicData.TablesTechData)
            foreach (TechnicalData tech in PublicData.TechnicalData)
                if (cost.Code == tech.CcCod)
                    if (tech.CodTchdt == table.CodTchdt)
                        if (prod.CodShape <= tech.CodShape &&
                            prod.CodType <= tech.CodType &&
                            prod.CodEdge <= tech.CodEdge &&
                            prod.CodSurface <= tech.CodSurface &&
                            prod.CodRoughness <= tech.CodRoughness &&
                            prod.CodProtection <= tech.CodProtection &&
                            prod.CodTolerance <= tech.CodTolerance &&
                            prod.CodInternalQuality <= tech.CodInternalQuality &&
                            prod.CodAttributeQuality <= tech.CodAttributeQuality &&
                            prod.CodCmmercialQuality <= tech.CodCmmercialQuality &&
                            Convert.ToInt32(prod.CodEnterPoint) <= Convert.ToInt32(tech.CodEnterPoint) &&
                            prod.CodPrticularRule <= tech.CodPrticularRule &&
                            prod.CodNextUse <= tech.CodNextUse &&
                            prod.CodNextUser <= tech.CodNextUser &&
                            prod.ThiknessProd <= tech.TksProd &&
                            prod.WidthProd <= tech.WidthProd &&
                            prod.LengthProd <= tech.LengthProd)
                        {
                            table.NumCoef = tech.NumCoef;
                            break;
                        }
            PublicData.FormulaParameters.Tk = prod.ThiknessProd;
            PublicData.FormulaParameters.Wd = prod.WidthProd;
            PublicData.FormulaParameters.Lg = prod.LengthProd;
        }
        public ArrayList ExpenseItem(int flgFerro, int codPos, int costCenterId, PossibleProducts prod, int expitExpenseId, double numCoef, int codIntQly, int codData)
        {
            ArrayList expenseItem = new ArrayList();
            if (flgFerro != 1)
                expitExpenseId = PublicData.ExtractAssign.Where(b => b.CostCenterId == costCenterId && b.CodPos == codPos).FirstOrDefault().ExpitExpenseId;
            expenseItem.Add(costCenterId);
            expenseItem.Add(prod.Id);
            expenseItem.Add(expitExpenseId);
            expenseItem.Add(prod.MachinDataId);
            expenseItem.Add(Math.Round(numCoef, 8));
            expenseItem.Add(codIntQly);
            expenseItem.Add(codData);
            return expenseItem;
        }
        public void InsertError(int codError, int codCcntr, string desCcntr, int coopsStatusId, int tableNum, string product)
        {
            Database workDatabase = new Database(PublicData.ConnectionServer);
            workDatabase.ExecuteCommand(@"Insert Into Coa_Operation_Errors
                                                (Operation_Error_Id,
                                                 Num_Err_Operr,
                                                 Coops_Operation_Status_Id,
                                                 Des_Att1_Operr,
                                                 Cod_Att1_Operr,
                                                 Des_Att2_Operr,
                                                 Cod_Att2_Operr,
                                                 Des_Att4_Operr,
                                                 Cod_Att4_Operr,
                                                 Des_Att5_Operr,
                                                 Cod_Att5_Operr,
                                                 Des_Att6_Operr,
                                                 Cod_Att6_Operr,
                                                 Des_Att10_Operr,
                                                 Typ_Err_Operr)
                                              Values
                                                (Coa_Operation_Errors_Seq.Nextval,
                                                 {0},
                                                 {1},
                                                 {2},
                                                 'مرکز هزينه',
                                                 {3},
                                                 'شرح',
                                                 {4},
                                                 'جدول',
                                                 {5},
                                                 'محصول',
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
                tableNum,
                product,
                CommandType.Text);
        }
    }
}