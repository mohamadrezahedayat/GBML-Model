using System.Collections;

namespace GBML_Model
{
    public class Formula2Charge : ChargeFormula, IFormulaCharge
    {
        public Formula2Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double k1 = 0;
            double k2 = 0;
            double k3 = 0;

            ///Ferroalloy for Normal Heat  203
            PublicData.NormalHeat = Ret.ReturnElements_eaf(ref expenseItems, costCenter, prod, 203, k1, k2, k3, 1);

            PublicData.ChargingRatio[prod.Index] =
                PublicData.Products
                - PublicData.NormalHeat;

            PublicData.TotCharg = PublicData.TotCharg + PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg = PublicData.TotQtyCharg + (PublicData.ChargingRatio[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}