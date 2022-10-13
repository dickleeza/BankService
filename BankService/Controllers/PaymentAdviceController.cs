using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using BankService.common;
using BankService.Models;

namespace BankService.Controllers
{
    public class PaymentAdviceController : ApiController
    {
        SqlDataReader rdr = null;
        SqlCommand cmd = null;
        // POST: PaymentAdvice

        [HttpPost]
        public IHttpActionResult Get(Bankadvice ObjVal)
        {
            try
            {
                int MResultCnt = 0;
                if (ObjVal == null)
                {
                    var MessageOut = new
                    {
                        StatusCode = 400,
                        Message = "Empty or invalid request",
                    };
                    return Ok(MessageOut);
                }
                //Open Connection to the Database and confirm the account being paid for exists
                Connection.openconnection();
                cmd = new SqlCommand("SELECT AccountCode,AccountName,BillCurrency FROM [StudentInfo] WHERE AccountCode = @AccountCode", Connection.con);
                cmd.Parameters.AddWithValue("@AccountCode", ObjVal.DocumentRefNo);
                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    //confirm if the Transaction reference already exists
                    Connection.openconnection();
                    cmd = new SqlCommand("SELECT TransReferenceCode FROM [FBankTransactions] WHERE TransReferenceCode = @TransReferenceCode", Connection.con);
                    cmd.Parameters.AddWithValue("@TransReferenceCode", ObjVal.TransactionRefCode);
                    rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        var MessageOut1 = new
                        {
                            StatusCode = 402,
                            Message = "Payment Advice with Transaction Reference Code " + ObjVal.TransactionRefCode + " already exists",
                        };
                        return Ok(MessageOut1);
                    }
                    else
                    {
                        //Add the payment Advice details
                        Connection.openconnection();
                        cmd = new SqlCommand("INSERT INTO FBankTransactions(TransReferenceCode,TransactionDate,Currency,DocumentRefNo,PaymentDate,PaymentCode,PaymentMode,PaymentAmount,AdditionalInfo,AccountNumber,AccountName,InstitutionCode,InstitutionName,BankCode,BranchCode,IPAddress,serviceName,messageID) " +
                                            "VALUES(@transreferencecode, @transactiondate, @currency, @documentrefno, @paymentdate, @paymentcode, @paymentmode, @paymentamount, @additionalinfo, @accountnumber, @accountname, @institutioncode, @institutionname, @bankcode, @branchcode, @ipaddress, @servicename, @messageid)", Connection.con);
                        cmd.Parameters.AddWithValue("@transreferencecode", ObjVal.TransactionRefCode);
                        cmd.Parameters.AddWithValue("@transactiondate", ObjVal.TransactionDate);
                        cmd.Parameters.AddWithValue("@currency", ObjVal.Currency);
                        cmd.Parameters.AddWithValue("@documentrefno", ObjVal.DocumentRefNo);
                        cmd.Parameters.AddWithValue("@paymentdate", ObjVal.PaymentDate);
                        cmd.Parameters.AddWithValue("@paymentcode", ObjVal.PaymentCode);
                        cmd.Parameters.AddWithValue("@paymentmode", ObjVal.PaymentMode);
                        cmd.Parameters.AddWithValue("@paymentamount", ObjVal.PaymentAmount);
                        cmd.Parameters.AddWithValue("@additionalinfo", ObjVal.AdditionalInfo);
                        cmd.Parameters.AddWithValue("@accountnumber", ObjVal.AccountNumber);
                        cmd.Parameters.AddWithValue("@accountname", ObjVal.AccountName);
                        cmd.Parameters.AddWithValue("@institutioncode", ObjVal.InstitutionCode);
                        cmd.Parameters.AddWithValue("@institutionname", ObjVal.InstitutionName);
                        cmd.Parameters.AddWithValue("@bankcode", ObjVal.BankCode);
                        cmd.Parameters.AddWithValue("@branchcode", ObjVal.BranchCode);
                        cmd.Parameters.AddWithValue("@ipaddress", HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                        cmd.Parameters.AddWithValue("@servicename", ObjVal.ServiceName);
                        cmd.Parameters.AddWithValue("@messageid", ObjVal.MessageID);
                        MResultCnt = cmd.ExecuteNonQuery();
                        if(MResultCnt > 0)
                        {
                            var MessageOut = new
                            {
                                StatusCode = 200,
                                ReturnDate = DateTime.Now.Date,
                                Message = "Payment Advice " + ObjVal.TransactionRefCode + " Posted Successfully",
                            };
                            return Ok(MessageOut);
                        }
                        else
                        {
                            var MessageOut = new
                            {
                                StatusCode = 405,
                                Message = "An Error occourred while trying to post Post the Payment Advice, Try agian later",
                            };
                            return Ok(MessageOut);
                        }
                    }
                }
                //if the Student Record does not exists in the database, return the message with failure status code
                else
                {
                    var MessageOut = new
                    {
                        StatusCode = 404,
                        Message = "AccountCode " + ObjVal.DocumentRefNo + " doesnt exist in the student staging table",
                    };
                    return Ok(MessageOut);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}