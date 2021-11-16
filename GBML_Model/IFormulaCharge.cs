using System.Collections;

namespace GBML_Model
{
    public interface IFormulaCharge
    {
        void Formula(CostCenter costCenter, PossibleProducts prod, ref ArrayList expenseItems);
    }
}