using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using BankService.common;
using BankService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BankService.Controllers
{
    
    public class ValidateController : ApiController
    {
        SqlDataReader rdr = null;
        SqlCommand cmd = null;
        [HttpGet]
        public IHttpActionResult Get(Validation ObjVal)
        {
            try
            {
                if (ObjVal == null)
                {
                    var MessageOut = new
                    {
                        StatusCode = 0,
                        Message = "Empty or invalid request",
                    };
                    return Ok(MessageOut);
                }
                //Open Connection to the Database and query the Student Details
                Connection.openconnection();
                using (cmd = new SqlCommand("SELECT AccountCode,AccountName,BillCurrency FROM [StudentInfo] WHERE AccountCode = @AccountCode", Connection.con))
                {
                    cmd.Parameters.AddWithValue("@AccountCode", ObjVal.AccountCode);
                    rdr = cmd.ExecuteReader();
                    //if the Student Record exists in the database, return the student details with success status code
                    if (rdr.Read())
                    {
                        var MessageOut = new
                        {
                            StatusCode = 200,
                            AccountCode = Convert.ToString(rdr["AccountCode"]),
                            AccountName = Convert.ToString(rdr["AccountName"]),
                            BillCurrency = Convert.ToString(rdr["BillCurrency"]),
                            ReturnDate = DateTime.Now.Date
                        };
                        return Ok(MessageOut);
                    }
                    //if the Student Record does not exists in the database, return the message with failure status code
                    else
                    {
                        var MessageOut = new
                        {
                            StatusCode = 404,
                            Message = "AccountCode" + " " + ObjVal.AccountCode + " " + "doesnt exist in the student staging table",
                        };
                        return Ok(MessageOut);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

    }
}