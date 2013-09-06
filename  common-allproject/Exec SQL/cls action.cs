

string allQueryValues = "";
        ArrayList arrSQL = new ArrayList();

        // query string
        string colList =
                clsCPAConstant.RPT_COL_YEARMONTH + "," +
            clsCPAConstant.RPT_COL_CUSTOMERID + "," +
            clsCPAConstant.RPT_COL_STL_OVERDRAFT_AVE + "," +
            clsCPAConstant.RPT_COL_STL_OVERDRAFT_REC + "," +
            clsCPAConstant.RPT_COL_STL_OVERDRAFT_PAY + "," +
            clsCPAConstant.RPT_COL_STL_COMMERCIALBILL_AVE + "," +
            clsCPAConstant.RPT_COL_STL_COMMERCIALBILL_REC + "," +
            clsCPAConstant.RPT_COL_STL_COMMERCIALBILL_PAY + "," +
            clsCPAConstant.RPT_COL_STL_LOAN_AVE + "," +
            clsCPAConstant.RPT_COL_STL_LOAN_REC + "," +
            clsCPAConstant.RPT_COL_STL_LOAN_PAY + "," +
            clsCPAConstant.RPT_COL_LTL_FIXED_AVE + "," +
            clsCPAConstant.RPT_COL_LTL_FIXED_REC + "," +
            clsCPAConstant.RPT_COL_LTL_FIXED_PAY + "," +
            clsCPAConstant.RPT_COL_LTL_FLOATING_AVE + "," +
            clsCPAConstant.RPT_COL_LTL_FLOATING_REC + "," +
            clsCPAConstant.RPT_COL_LTL_FLOATING_PAY + "," +
            clsCPAConstant.RPT_COL_BB_AVE + "," +
            clsCPAConstant.RPT_COL_BB_REC + "," +
            clsCPAConstant.RPT_COL_BB_PAY + "," +
            clsCPAConstant.RPT_COL_BR_AVE + "," +
            clsCPAConstant.RPT_COL_BR_REC + "," +
            clsCPAConstant.RPT_COL_BR_PAY + "," +
            clsCPAConstant.RPT_COL_OTHERAPP_AVE + "," +
            clsCPAConstant.RPT_COL_OTHERAPP_REC + "," +
            clsCPAConstant.RPT_COL_OTHERAPP_PAY + "," +
            clsCPAConstant.RPT_COL_DEP_LIQUID_AVE + "," +
            clsCPAConstant.RPT_COL_DEP_LIQUID_REC + "," +
            clsCPAConstant.RPT_COL_DEP_LIQUID_PAY + "," +
            clsCPAConstant.RPT_COL_DEP_FIXED_AVE + "," +
            clsCPAConstant.RPT_COL_DEP_FIXED_REC + "," +
            clsCPAConstant.RPT_COL_DEP_FIXED_PAY + "," +
            clsCPAConstant.RPT_COL_OTHERSOURCE_AVE + "," +
            clsCPAConstant.RPT_COL_OTHERSOURCE_REC + "," +
            clsCPAConstant.RPT_COL_OTHERSOURCE_PAY + "," +
            clsCPAConstant.RPT_COL_RESERVEREQ_AVE + "," +
            clsCPAConstant.RPT_COL_RESERVEREQ_REC + "," +
            clsCPAConstant.RPT_COL_RESERVEREQ_PAY + "," +
            clsCPAConstant.RPT_COL_GUARANTEE_AVE + "," +
            clsCPAConstant.RPT_COL_GUARANTEE_INC + "," +
            clsCPAConstant.RPT_COL_CLEANLC_AVE + "," +
            clsCPAConstant.RPT_COL_CLEANLC_INC + "," +
            clsCPAConstant.RPT_COL_ACCEPTANCE_AVE + "," +
            clsCPAConstant.RPT_COL_ACCEPTANCE_INC + "," +
            clsCPAConstant.RPT_COL_COMMITMENT_AVE + "," +
            clsCPAConstant.RPT_COL_COMMITMENT_INC + "," +
            clsCPAConstant.RPT_COL_OTHERS_AVE + "," +
            clsCPAConstant.RPT_COL_OTHERS_INC + "," +
            clsCPAConstant.RPT_COL_DOCLC_TUR + "," +
            clsCPAConstant.RPT_COL_DOCLC_INC + "," +
            clsCPAConstant.RPT_COL_EXPBILLHANDLING_TUR + "," +
            clsCPAConstant.RPT_COL_EXPBILLHANDLING_INC + "," +
            clsCPAConstant.RPT_COL_IMPBILLHANDLING_TUR + "," +
            clsCPAConstant.RPT_COL_IMPBILLHANDLING_INC + "," +
            clsCPAConstant.RPT_COL_COLLECTING_TUR + "," +
            clsCPAConstant.RPT_COL_COLLECTING_INC + "," +
            clsCPAConstant.RPT_COL_PAYMENT_TUR + "," +
            clsCPAConstant.RPT_COL_PAYMENT_INC + "," +
            clsCPAConstant.RPT_COL_REMITTANCE_TUR + "," +
            clsCPAConstant.RPT_COL_REMITTANCE_INC + "," +
            clsCPAConstant.RPT_COL_LOAN_TUR + "," +
            clsCPAConstant.RPT_COL_LOAN_INC + "," +
            clsCPAConstant.RPT_COL_OTHERS01_TUR + "," +
            clsCPAConstant.RPT_COL_OTHERS01_INC + "," +
            clsCPAConstant.RPT_COL_FOREIGNEXCHANGEPL_TUR + "," +
            clsCPAConstant.RPT_COL_FOREIGNEXCHANGEPL_INC + "," +
            clsCPAConstant.RPT_COL_OTHERS02_TUR + "," +
            clsCPAConstant.RPT_COL_OTHERS02_INC + "," +
            clsCPAConstant.RPT_COL_REPORTSTATUS;

     
        int result = 0;


// Phong change: if total row < 100, i must insert line by line and not return after insert becase it will break while loop
                    if (i == m_RowPerUnit * rowNum - 2 - NonCustomerUnit)
                    {
                        if (rowNum == CPANumber && allQueryValues != "")
                        {
                            #region Insert multi rows into table customer

                              allQueryValues = allQueryValues.Substring(0, allQueryValues.Length - 11);
                                string _sql = "INSERT INTO tblCPA_TempCustomer (" + colList + ") " + allQueryValues;

                                arrSQL.Add(_sql);

                                result = m_DAL.ExecuteTransaction(arrSQL);
                                if (result == 0) return;
                            _sql = "";
                            allQueryValues = "";
                            arrSQL.Clear();
                            CPARemain = 0;
                            #endregion

                        }
                        else
                        {
                            if (rowNum % m_MaxUnit == 0 && !"".Equals(allQueryValues))
                            {
                                #region Insert multi rows into table customer

                                allQueryValues = allQueryValues.Substring(0, allQueryValues.Length - 11);
                                string _sql = "INSERT INTO tblCPA_TempCustomer (" + colList + ") " + allQueryValues;

                                arrSQL.Add(_sql);
                               
                                    result = m_DAL.ExecuteTransaction(arrSQL);
                                    if (result == 0) return;
                               
                                _sql = "";
                                allQueryValues = "";
                                arrSQL.Clear();
                                CPARemain = CPARemain - m_MaxUnit;
                                #endregion
                            }
                        }

                    }