using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Config.Classes;
using Phoenix.Cpa.Common;
using Phoenix.Common.Functions;
using DataEncryption;

namespace Phoenix.Cpa.Dal
{
    public class clsDataAccessLayer
    {
        
        private readonly string CONNECTION_STRING = Encryption.decrypt(System.Configuration.ConfigurationSettings.AppSettings["CPAConnectionString"].ToString());
        //  private readonly string CONNECTION_STRING = "Data Source=10.1.6.126\\SQLEXPRESS;Initial Catalog=CPA_DB;User ID=Admin;Password=Phoenix";
        public SqlConnection m_Connection;
        private SqlCommand m_Command;
        private SqlDataAdapter m_Adapter;
        public SqlTransaction m_transaction;
        /// <summary>
        /// Initializes a new instance of the <see cref="CPADataAccessLayer" /> class.
        /// </summary>
        public clsDataAccessLayer()
        {
            this.m_Connection = new SqlConnection(CONNECTION_STRING);
            this.m_Command = new SqlCommand();
            this.m_Adapter = new SqlDataAdapter();

        }

        /// <summary>
        /// Fills the specified table.
        /// </summary>
        /// <param name="table">The table.</param>
        public void Fill(DataTable table)
        {
            try
            {
                if (m_Adapter != null)
                {
                    this.m_Adapter.Fill(table);
                }
            }
            catch (SqlException ex)
            {
                string error = ex.Message;
                clsError.ShowErrorScreen(ex.Message +
                  Environment.NewLine +
                  ex.TargetSite);
                clsLogFile.LogException(error, clsCPAConstant.CPA_TEXT);
            }
        }

        /// <summary>
        /// Fills the specified dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        public void Fill(DataSet dataset)
        {
            try
            {
                if (m_Adapter != null)
                {
                    this.m_Adapter.Fill(dataset);
                }
            }
            catch (SqlException ex)
            {
                string error = ex.Message;
                clsError.ShowErrorScreen(ex.Message +
                  Environment.NewLine +
                  ex.TargetSite);
                clsLogFile.LogException(error, clsCPAConstant.CPA_TEXT);
            }
        }

        /// <summary>
        /// Sets the command.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">Type of the command.</param>
        public void SetCommand(string query, CommandType commandType)
        {
            this.m_Command.Parameters.Clear();
            this.m_Command.Connection = this.m_Connection;
            this.m_Command.CommandType = commandType;
            this.m_Command.CommandText = query;


        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="paramter">The paramter.</param>
        public void AddParameter(SqlParameter paramter)
        {
            this.m_Command.Parameters.Add(paramter);
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="paramatername">The paramater name.</param>
        /// <param name="parametervalue">The parameter value.</param>
        public void AddParameter(string paramatername, object parametervalue)
        {
            this.m_Command.Parameters.AddWithValue(paramatername, parametervalue);
        }

      


        /// <summary>
        /// Executes the non query. execute with list param and rollback all if false
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query, CommandType commandType, List<SqlParameter[]> parameters)
        {
            if (this.m_Connection.State == ConnectionState.Closed)
                this.m_Connection.Open();
            int row = 0;
            SetCommand(query, commandType);
            if (m_transaction == null)
            {
                m_transaction = m_Connection.BeginTransaction();
            }
            m_Command.Transaction = m_transaction;
            try
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    foreach (SqlParameter param in parameters[i])
                    {
                        AddParameter(param);
                    }

                    row += this.m_Command.ExecuteNonQuery();


                    //  row += Convert.ToInt32(m_Command.Parameters["@Count"].Value);
                   this.m_Command.Parameters.Clear();
                }
                //m_transaction.Commit();
               
                //if (this.m_Connection.State == ConnectionState.Open)
                //    this.m_Connection.Close();
                
                return row;
            }
            catch (SqlException ex)
            {

                throw new System.ArgumentException(ex.Message);
            }            
        }




        public void ExecuteNonQueryNonReply(string query, CommandType commandType, SqlParameter[] parameters)
        {

            SetCommand(query, commandType);
            foreach (SqlParameter param in parameters)
            {
                AddParameter(param);
            }
            if (this.m_Connection.State == ConnectionState.Closed)
                this.m_Connection.Open();

            SqlTransaction tran = m_Connection.BeginTransaction();
            m_Command.Transaction = tran;

            try
            {
                this.m_Command.ExecuteNonQuery();
                tran.Commit();
                //tran.Rollback();

                if (this.m_Connection.State == ConnectionState.Open)
                    this.m_Connection.Close();


            }
            catch (SqlException ex)
            {
               // tran.Rollback();
                throw new System.ArgumentException(ex.Message);
            }

        }
        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="outputParamName">Name of the output param.</param>
        /// <returns>The affected row that stored procedure returns</returns>
        public int ExecuteNonQuery(string query, CommandType commandType, SqlParameter[] parameters, object outputParamName)
        {

            SetCommand(query, commandType);
            foreach (SqlParameter param in parameters)
            {
                AddParameter(param);
            }
            if (this.m_Connection.State == ConnectionState.Closed)
                this.m_Connection.Open();
            if (m_transaction == null)
            {
                m_transaction = m_Connection.BeginTransaction();
            }
            m_Command.Transaction = m_transaction;
            try
            {
                this.m_Command.ExecuteNonQuery();

              
                int row = Convert.ToInt32(m_Command.Parameters[outputParamName.ToString()].Value);

                //if (this.m_Connection.State == ConnectionState.Open)
                //    this.m_Connection.Close();

                return row;
            }
            catch (SqlException ex)
            {
                //tran.Rollback();
                throw new System.ArgumentException(ex.Message);
            }            
        }

        /// <summary>
        /// Execute non query no reply with transaction
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        public void ExecuteNonQueryNonReplyWithTransaction(string query, CommandType commandType, SqlParameter[] parameters)
        {
            try
            {
                if (this.m_Connection.State == ConnectionState.Closed)
                    this.m_Connection.Open();

                SetCommand(query, commandType);

                if (m_transaction == null)
                {
                    m_transaction = m_Connection.BeginTransaction();
                }
                m_Command.Transaction = m_transaction;

                foreach (SqlParameter param in parameters)
                {
                    AddParameter(param);
                }

                this.m_Command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new System.ArgumentException(ex.Message);
            }

        }


        /// <summary>
        /// Executes the data reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <returns></returns>
        public DataTable ExecuteDataReader(string query, CommandType commandType)
        {
            DataTable table = new DataTable();
            try
            {
                if (m_Adapter != null)
                {
                    SetCommand(query, commandType);
                    this.m_Adapter.SelectCommand = m_Command;
                    this.m_Adapter.Fill(table);
                }
                else
                {
                    this.m_Adapter = new SqlDataAdapter();
                    this.m_Adapter.SelectCommand = m_Command;
                    this.m_Adapter.Fill(table);
                }
                m_Adapter.Dispose();
            }
            catch (SqlException ex)
            {
                throw new System.ArgumentException(ex.Message);
            }
            return table;
        }

        /// <summary>
        /// Executes the data reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public DataTable ExecuteDataReader(string query, CommandType commandType, SqlParameter[] parameters)
        {
            DataTable table = new DataTable();
            try
            {
                if (m_Adapter != null)
                {
                    SetCommand(query, commandType);
                    foreach (SqlParameter param in parameters)
                    {
                        m_Command.Parameters.Add(param);
                    }
                    this.m_Adapter.SelectCommand = m_Command;
                    this.m_Adapter.Fill(table);
                }
                else
                {
                    this.m_Adapter = new SqlDataAdapter();
                    this.m_Adapter.SelectCommand = m_Command;
                    this.m_Adapter.Fill(table);
                }
                m_Adapter.Dispose();
            }
            catch (SqlException ex)
            {
                throw new System.ArgumentException(ex.Message);
            }
            return table;
        }

        /// <summary>
        /// Executes the data reader.
        /// </summary>
        /// <returns></returns>
        public SqlDataReader ExecuteDataReader()
        {
            SqlDataReader reader = null;
            try
            {
                if (this.m_Connection.State == ConnectionState.Closed)
                    this.m_Connection.Open();


                reader = this.m_Command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                string error = ex.Message;
                throw new System.ArgumentException(ex.Message);
            }
            return reader;
        }

        /// <summary>
        /// Executes the transaction.
        /// </summary>
        /// <returns></returns>
        public int ExecuteTransaction(ArrayList arrSQL)
        {
            int row = 0;
            if (this.m_Connection.State == ConnectionState.Closed)
                this.m_Connection.Open();
            SqlTransaction transaction = this.m_Connection.BeginTransaction();
            try
            {

                for (int i = 0; i < arrSQL.Count; i++)
                {

                    SetCommand(arrSQL[i].ToString(), CommandType.Text);
                    this.m_Command.Transaction = transaction;
                    this.m_Command.ExecuteNonQuery();
                }
                transaction.Commit();
                row = 1;
                if (this.m_Connection.State == ConnectionState.Open)
                    this.m_Connection.Close();
            }
            catch (SqlException ex)
            {
                string error = ex.Message;
                throw new System.ArgumentException(ex.Message);
            }

            return row;
        }

        public bool DeleteTransactions()
        {
            return true;
        }
    }
}
