using System.Collections;
using System.Linq;

namespace GBML_Model
{
    public class Formula1Charge : ChargeFormula, IFormulaCharge
    {
        public Formula1Charge(ReturnFormulaElements returnFormulaElements) : base(returnFormulaElements)
        {

        }
        public void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems)
        {
            double k1 = PublicData.TablesTechData.Where(c => c.Des.Equals("K1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//101
            double k2 = PublicData.TablesTechData.Where(c => c.Des.Equals("K2") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//102
            double k3 = PublicData.TablesTechData.Where(c => c.Des.Equals("K3") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//103

            ///Sponge iron 201
            PublicData.SpongeIron = Ret.ReturnElements_eaf(ref expenseItems, costCenter, prod, 201, k1, k2, k3, 1);

            ///Scrap 202
            PublicData.Scrap = Ret.ReturnElements_eaf(ref expenseItems, costCenter, prod, 202, k1, k2, k3, 1);

            ///Ferroalloy for Normal Heat  203
            PublicData.NormalHeat = Ret.ReturnElements_eaf(ref expenseItems, costCenter, prod, 203, k1, k2, k3, 1);

            ///Ferroalloy for Recycled steel 204
            PublicData.RecycledSteel = Ret.ReturnElements_eaf(ref expenseItems, costCenter, prod, 204, k1, k2, k3, 1);

            ///Recuperable Materials 205
            PublicData.RecuperableMaterials = Ret.ReturnElements_eaf(ref expenseItems, costCenter, prod, 205, k1, k2, k3, 1);


            PublicData.ChargingRatio[prod.Index] =
                (PublicData.SpongeIron
                 + PublicData.Scrap
                 + PublicData.NormalHeat
                 + PublicData.RecycledSteel)
                - PublicData.RecuperableMaterials;

            PublicData.TotCharg = PublicData.TotCharg + PublicData.ChargingRatio[prod.Index];
            PublicData.TotQtyCharg = PublicData.TotQtyCharg + (PublicData.ChargingRatio[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}