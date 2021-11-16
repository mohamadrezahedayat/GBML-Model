using System.Linq;

namespace GBML_Model
{
    public class Formula4Std : StandardTimeFormula, IFormulaStd
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double nw = PublicData.TablesTechData.Where(c => c.Des.Equals("Nw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //116
            double iw = PublicData.TablesTechData.Where(c => c.Des.Equals("Iw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //117
            double cc = PublicData.TablesTechData.Where(c => c.Des.Equals("Cc") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //118
            double st1 = PublicData.TablesTechData.Where(c => c.Des.Equals("St1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//157
            double st2 = PublicData.TablesTechData.Where(c => c.Des.Equals("St2") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//158
            double st3 = PublicData.TablesTechData.Where(c => c.Des.Equals("St3") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//163
            double st4 = PublicData.TablesTechData.Where(c => c.Des.Equals("St4") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//168
            double p1 = PublicData.TablesTechData.Where(c => c.Des.Equals("P1") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //160
            double p2 = PublicData.TablesTechData.Where(c => c.Des.Equals("P2") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //161
            double p3 = PublicData.TablesTechData.Where(c => c.Des.Equals("P3") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //162
            double p4 = PublicData.TablesTechData.Where(c => c.Des.Equals("P4") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;  //164
            double cr = PublicData.TablesTechData.Where(c => c.Des.Equals("Cr") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//165

            if (cr != 0)
                PublicData.Std[prod.Index] = ((((1 / (cr)) * (((st1) * ((p1) / 100))
                                                              + ((st2) * ((p2) / 100)) + ((st3) * ((p3) / 100))
                                                              + ((st4) * ((p4) / 100)))) * (nw)) + (iw)) * (cc);
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft4", "Cr", "", prod.Index);

            PublicData.TotStd = PublicData.TotStd + PublicData.Std[prod.Index];
            PublicData.TotQtyStd = PublicData.TotQtyStd + (PublicData.Std[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}