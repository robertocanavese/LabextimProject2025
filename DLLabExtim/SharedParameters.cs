using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DLLabExtim
{
    public class SharedConfiguration
    {

        public string FtpBaseUrl { get; internal set; }
        public string FtpUsername { get; internal set; }
        public string FtpPassword { get; internal set; }
        public string FtpInputPath { get; internal set; }
        public string FtpOutputPath { get; internal set; }

        public string LocRootPath { get; internal set; }
        public string LocInputPath { get; internal set; }
        public string LocOutputPath { get; internal set; }
        public string LocErrorPath { get; internal set; }
        public string LocArchInPath { get; internal set; }
        public string LocArchOutPath { get; internal set; }

        
        public SharedConfiguration()
        {

            try
            {
                FtpBaseUrl = ConfigurationManager.AppSettings["ftpBaseUrl"].ToString();
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("Labextim - errore in fase di caricamento del parametro {0} ({1})", "ftpBaseUrl", _ex.Message));
            }
            try
            {
                FtpInputPath = ConfigurationManager.AppSettings["ftpInputPath"].ToString();
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("Labextim - errore in fase di caricamento del parametro {0} ({1})", "ftpInputPath", _ex.Message));
            }

            try
            {
                FtpOutputPath = ConfigurationManager.AppSettings["ftpOutputPath"].ToString();
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("Labextim - errore in fase di caricamento del parametro {0} ({1})", "ftpOutputPath", _ex.Message));
            }
            try
            {
                FtpUsername = ConfigurationManager.AppSettings["ftpUsername"].ToString();
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("Labextim - errore in fase di caricamento del parametro {0} ({1})", "ftpUsername", _ex.Message));
            }
            try
            {
                FtpPassword = ConfigurationManager.AppSettings["ftpPassword"].ToString();
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("Labextim - errore in fase di caricamento del parametro {0} ({1})", "ftpPassword", _ex.Message));
            }


            try
            {
                LocRootPath = ConfigurationManager.AppSettings["locRootPath"].ToString();
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("Labextim - errore in fase di caricamento del parametro {0} ({1})", "locRootPath", _ex.Message));
            }
            try
            {
                LocInputPath = ConfigurationManager.AppSettings["locInputPath"].ToString();
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("Labextim - errore in fase di caricamento del parametro {0} ({1})", "locInputPath", _ex.Message));
            }
            try
            {
                LocOutputPath = ConfigurationManager.AppSettings["locOutputPath"].ToString();
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("Labextim - errore in fase di caricamento del parametro {0} ({1})", "locOutputPath", _ex.Message));
            }
            try
            {
                LocErrorPath = ConfigurationManager.AppSettings["locErrorPath"].ToString();
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("Labextim - errore in fase di caricamento del parametro {0} ({1})", "locErrorPath", _ex.Message));
            }
            try
            {
                LocArchInPath = ConfigurationManager.AppSettings["locArchInPath"].ToString();
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("Labextim - errore in fase di caricamento del parametro {0} ({1})", "locArchInPath", _ex.Message));
            }
            try
            {
                LocArchOutPath = ConfigurationManager.AppSettings["locArchOutPath"].ToString();
            }
            catch (Exception _ex)
            {
                throw new Exception(string.Format("Labextim - errore in fase di caricamento del parametro {0} ({1})", "locArchOutPath", _ex.Message));
            }

        }



    }
}
