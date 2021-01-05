using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EZ_PaintProduction.Modulos
{
    public class DataAccessLayer
    {
        private Modelos.ezAirdbEntidades _connectionModel;
        private Boolean _statusConnection;
        private string _stringConnection;
        public DataAccessLayer()
        {
            Encrypt_Aes cryptAes = new Encrypt_Aes();
            try
            {
                //get values from appconfig
                string ipServer = cryptAes.DecryptString(Properties.Settings.Default.serverIP);
                string dataBase = cryptAes.DecryptString(Properties.Settings.Default.serverDB);
                string idLog = cryptAes.DecryptString(Properties.Settings.Default.serverUser);
                string passwordLog = cryptAes.DecryptString(Properties.Settings.Default.serverPassword);
                _stringConnection = crateConnectionString(ipServer, dataBase, idLog, passwordLog);
                testConnection();
            }
            catch (Exception)
            {
                MessageBox.Show("Configuration file corrupted", "ERROR #10", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        //this overload can use for test connection
        public DataAccessLayer(string ipServer, string nameDB, string user, string password)
        {
            _stringConnection = crateConnectionString(ipServer, nameDB, user, password);
            testConnection();
        }
        // the name field
        public Modelos.ezAirdbEntidades ConectionModel    // the Name property
        {
            get
            {
                //create a new dbcontext
                establishConnection();
                return _connectionModel;
            }
            //get => _connectionModel;
            set => _connectionModel = value;
        }
        public Boolean StatusConnection    // the Name property
        {
            get => _statusConnection;
            set => _statusConnection = value;
        }
        public string StringConnection    // the Name property
        {
            get => _stringConnection;
            set => _stringConnection = value;
        }

        private void testConnection()
        {
            establishConnection();
            using (_connectionModel)
            {
                if (_connectionModel.Database.Exists() == false)
                {
                    _statusConnection = false;
                }
                else
                {
                    _statusConnection = true;
                }
            }
        }

        private string crateConnectionString(string ipServer, string dataBase, string idLog, string passwordLog)
        {
            try
            {
                //this only work with sql
                EntityConnectionStringBuilder constructorDeConexion = new EntityConnectionStringBuilder();
                constructorDeConexion.Provider = "System.Data.SqlClient";
                constructorDeConexion.ProviderConnectionString = "data source=" + ipServer + ";initial catalog=" + dataBase + ";persist security info=True;user id=" + idLog + "; password=" + passwordLog + ";multipleactiveresultsets=True;application name=EntityFramework";
                constructorDeConexion.Metadata = "res://*/Modelos.Model1.csdl|res://*/Modelos.Model1.ssdl|res://*/Modelos.Model1.msl";
                return constructorDeConexion.ConnectionString;
            }
            catch (Exception)
            {
                return "";
            }
        }
        private void establishConnection()
        {
            _connectionModel = null;
            try
            {
                if (_stringConnection == "")
                {
                    _statusConnection = false;
                    _connectionModel = null;
                }
                else
                {
                    //crate a new databasecontext
                    _connectionModel = new Modelos.ezAirdbEntidades(_stringConnection);
                    _statusConnection = true;
                }
            }
            catch (Exception)
            {
                _connectionModel = null;
                _statusConnection = false;
            }

        }
    }
}
