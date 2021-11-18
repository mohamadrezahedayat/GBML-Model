using System.Collections;

namespace GBML_Model
{
    public class Formula11Charge : ChargeFormula, IFormulaCharge
    {
        public Formula11Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double k1 = 0;
            double k2 = 0;
            double k3 = 0;

            //Sponge iron 201
            PublicData.SpongeIron = Ret.ReturnElements_eaf(ref expenseItems, costCenter, prod, 201, k1, k2, k3, 0);

            //Recuperable Materials 205
            PublicData.RecuperableMaterials = Ret.ReturnElements_eaf(ref expenseItems, costCenter, prod, 205, k1, k2, k3, 0);

            PublicData.ChargingRatio[prod.Index] =
                PublicData.Products
                - PublicData.SpongeIron//201
                + PublicData.RecuperableMaterials//205
                ;

            PublicData.TotCharg = PublicData.TotCharg + PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg = PublicData.TotQtyCharg + (PublicData.ChargingRatio[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}