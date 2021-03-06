
using System;
using System.Data;
using System.Linq;
using DataLayer;

namespace GBML_Model
{
    public class StandardTimeFormula
    {
        public double Power6 { get; private set; }
        public double Power3 { get; private set; }
        public StandardTimeFormula()
        {
            Power6 = Math.Pow(10, 6);
            Power3 = Math.Pow(10, -3);
        }
        public void InsertError(int codError, int codCcntr, int coopsStatusId, string formulaNumber, string fieldName, string tableNum, int productIndex)
        {
            if (fieldName != "" && tableNum == "")
            {
                tableNum = PublicData.TablesTechData.Where(a => a.Des == fieldName).FirstOrDefault().CodTchdt.ToString();
            }

            var desCc =
                PublicData.CostCenter.FirstOrDefault(c => c.Code == codCcntr)?.Description;
            var codProd =
                PublicData.PossibleProducts.FirstOrDefault(a => a.Index == productIndex)?.CodProd;
            var workDatabase = new Database(PublicData.ConnectionServer);
            string commandString = $@"
                Insert Into Coa_Operation_Errors
                    (Operation_Error_Id,
                    Num_Err_Operr,
                    Coops_Operation_Status_Id,
                    Des_Att1_Operr,
                    Cod_Att1_Operr,
                    Des_Att2_Operr,
                    Cod_Att2_Operr,
                    Des_Att3_Operr,
                    Cod_Att3_Operr,
                    Des_Att4_Operr,
                    Cod_Att4_Operr,
                    Des_Att5_Operr,
                    Cod_Att5_Operr,
                    Des_Att6_Operr,
                    Cod_Att6_Operr,
                    Des_Att7_Operr,
                    Cod_Att7_Operr,
                    Des_Att10_Operr,
                    Typ_Err_Operr)
              Values
                (Coa_Operation_Errors_Seq.Nextval,
                {codError},
                {coopsStatusId},
                {codCcntr},
                'مرکز هزينه',
                '{desCc}',
                'شرح',
                '{formulaNumber}',
                'فرمول',
                '{codProd}',
                'محصول',
                '{fieldName}',
                'فيلد',
                {tableNum},
                'شماره جدول',
                (Select Mlare.Cod_Mlare_Parent || Mlare.Cod_Mlare
                From Coa_Machine_Load_Areas_Viw Mlare
                Where Mlare.Flg_Sta_Mlare = 1),
                'ناحيه-زير ناحيه',
                (Select Mlare.Machine_Load_Area_Id
                From Coa_Machine_Load_Areas Mlare
                Where Mlare.Flg_Sta_Mlare = 1),
                1)";
            workDatabase.ExecuteCommand(commandString, CommandType.Text);
        }
    }
}