using System.Collections;
using System.Linq;

namespace GBML_Model
{
    public class Formula9Charge : ChargeFormula, IFormulaCharge
    {
        public Formula9Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double la = PublicData.TablesTechData.Where(c => c.Des.Equals("La") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//132

            /////////02350 Coil process Scraps head & tail
            PublicData.HeadAndTail = la;
            expenseItems.Add(Ret.ExpenseItem(0, 1, costCenter.Id, prod, 0, PublicData.HeadAndTail, 0, 0));


            PublicData.ChargingRatio[prod.Index] =
                PublicData.Products
                + (PublicData.HeadAndTail);

            PublicData.TotCharg = PublicData.TotCharg + PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg = PublicData.TotQtyCharg + (PublicData.ChargingRatio[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}