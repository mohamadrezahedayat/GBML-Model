using System.Collections;
using System.Linq;

namespace GBML_Model
{
    public class Formula3Charge : ChargeFormula, IFormulaCharge
    {
        public Formula3Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double nsq = PublicData.TablesTechData.Where(c => c.Des.Equals("Nsq") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//111
            double sc = PublicData.TablesTechData.Where(c => c.Des.Equals("Sc") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//131
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//119
            double ls1 = PublicData.TablesTechData.Where(c => c.Des.Equals("Ls1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//139
            double ls2 = PublicData.TablesTechData.Where(c => c.Des.Equals("Ls2") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//140
            double hs = PublicData.TablesTechData.Where(c => c.Des.Equals("Hs") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//137
            double c1 = PublicData.TablesTechData.Where(c => c.Des.Equals("C1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//138
            double ts = PublicData.TablesTechData.Where(c => c.Des.Equals("Ts") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//141
            double la = PublicData.TablesTechData.Where(c => c.Des.Equals("La") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//132

            ///02290 Slab crops
            if (wg != 0 || nsq != 0)
            {
                PublicData.Slabcrops = (sc) / ((wg) * (nsq));
                expenseItems.Add(Ret.ExpenseItem(0, 1, costCenter.Id, prod, 0, PublicData.Slabcrops, 0, 0));
            }
            else if (wg == 0)
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc3", "Wg", "", prod.Index);
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc3", "Nsq", "", prod.Index);

            ///02300 LadleSkull
            if (wg != 0 && nsq != 0)
            {
                PublicData.LadleSkull = ((ls1) / ((wg) * (nsq))) + (ls2);
                expenseItems.Add(Ret.ExpenseItem(0, 2, costCenter.Id, prod, 0, PublicData.LadleSkull, 0, 0));
            }
            else if (wg == 0)
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc3", "Wg", "", prod.Index);
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc3", "Nsq", "", prod.Index);

            ///02260 Tundish skull 
            if (wg != 0 && nsq != 0)
            {
                PublicData.TundishSkull = (ts) / ((wg) * (nsq));
                expenseItems.Add(Ret.ExpenseItem(0, 3, costCenter.Id, prod, 0, PublicData.TundishSkull, 0, 0));
            }
            else if (wg == 0)
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc3", "Wg", "", prod.Index);
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc3", "Nsq", "", prod.Index);

            ///02100 Oxide scale
            if (wg != 0)
            {
                PublicData.OxideScale = (hs) / (wg);
                expenseItems.Add(Ret.ExpenseItem(0, 4, costCenter.Id, prod, 0, PublicData.OxideScale, 0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc3", "Wg", "", prod.Index);

            ///02110 Slab cutting Losses
            if (wg != 0)
            {
                PublicData.SlabLosses = (c1) / (wg);
                expenseItems.Add(Ret.ExpenseItem(0, 5, costCenter.Id, prod, 0, PublicData.SlabLosses, 0, 0));
            }
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "fc3", "Wg", "", prod.Index);

            ///02280 Slab scrapped
            PublicData.SlabScrapped = (la);
            expenseItems.Add(Ret.ExpenseItem(0, 6, costCenter.Id, prod, 0, PublicData.SlabScrapped, 0, 0));

            PublicData.ChargingRatio[prod.Index] =
                PublicData.Products
                + (PublicData.OxideScale
                   + PublicData.Slabcrops
                   + PublicData.LadleSkull
                   + PublicData.SlabScrapped
                   + PublicData.SlabLosses
                   + PublicData.TundishSkull);

            PublicData.TotCharg = PublicData.TotCharg + PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg = PublicData.TotQtyCharg + (PublicData.ChargingRatio[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}