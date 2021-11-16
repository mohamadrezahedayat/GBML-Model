using System.Linq;

namespace GBML_Model
{
    public class Formula5Std : StandardTimeFormula, IFormulaStd
    {
        public void Formula(CostCenter costCenter, PossibleProducts prod)
        {
            double sm = PublicData.TablesTechData.Where(c => c.Des.Equals("Sm") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//151
            double wg = PublicData.TablesTechData.Where(c => c.Des.Equals("Wg") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//119
            double nw = PublicData.TablesTechData.Where(c => c.Des.Equals("Nw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//116
            double iw = PublicData.TablesTechData.Where(c => c.Des.Equals("Iw") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//117
            double ft = PublicData.TablesTechData.Where(c => c.Des.Equals("Ft") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//152
            double nt = PublicData.TablesTechData.Where(c => c.Des.Equals("Nt") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//154
            double wt = PublicData.TablesTechData.Where(c => c.Des.Equals("Wt") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//155
            double et = PublicData.TablesTechData.Where(c => c.Des.Equals("Et") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//153
            double nf = PublicData.TablesTechData.Where(c => c.Des.Equals("Nf") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//112
            double na = PublicData.TablesTechData.Where(c => c.Des.Equals("Na") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//115
            double ns = PublicData.TablesTechData.Where(c => c.Des.Equals("Ns") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//113
            double k6 = PublicData.TablesTechData.Where(c => c.Des.Equals("K6") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//106
            double k7 = PublicData.TablesTechData.Where(c => c.Des.Equals("K7") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//107
            double k8 = PublicData.TablesTechData.Where(c => c.Des.Equals("K8") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//108
            double k9 = PublicData.TablesTechData.Where(c => c.Des.Equals("K9") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//109
            double cc = PublicData.TablesTechData.Where(c => c.Des.Equals("Cc") && c.CcCod == costCenter.Code).FirstOrDefault().NumCoef;//118

            if (PublicData.FormulaParameters.Tk * PublicData.FormulaParameters.Wd * sm != 0 && wg != 0)
                PublicData.Std[prod.Index] = ((((Power6 / ((PublicData.FormulaParameters.Tk)
                                                           * (PublicData.FormulaParameters.Wd) * PublicData.FormulaParameters.Sigma * (sm) * 60))
                                                + ((((ft) * (nf) * (k6)) + ((et) * ((ns) + (na)) * (k7))
                                                                         + ((nt) * (na) * (k8)) + ((wt) * ((na) + (ns) - (nf)) * (k9))) / ((wg) * 60)) * (PublicData.FormulaParameters.X))
                                               * (nw)) + (iw)) * (cc);
            else if (PublicData.FormulaParameters.Tk == 0)
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft5", "Tk", "act", prod.Index);
            else if (PublicData.FormulaParameters.Wd == 0)
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft5", "Wd", "act", prod.Index);
            else if (wg == 0)
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft5", "Wg", "", prod.Index);
            else
                InsertError(10016, costCenter.Code, PublicData.CoopsStatusId, "ft5", "Sm", "", prod.Index);

            PublicData.TotStd = PublicData.TotStd + PublicData.Std[prod.Index];
            PublicData.TotQtyStd = PublicData.TotQtyStd + (PublicData.Std[prod.Index] * PublicData.PossibleProducts[prod.Index].QtyProd);
        }
    }
}