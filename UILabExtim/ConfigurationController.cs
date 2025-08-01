using System;
using System.Collections.Generic;
using System.Linq;
using CMLabExtim;
using DLLabExtim;

namespace UILabExtim
{
    public class ConfigurationController : BaseController
    {
        public bool SetConfiguration(Dictionary<string, string> data)
        {
            try
            {
                using (var _db = new GeneralDataContext())
                {
                    foreach (var _datum in data)
                    {
                        var _configuration = _db.Configuration.SingleOrDefault(d => d.ConfigKey == _datum.Key);
                        if (_configuration != null)
                            _configuration.ConfigValue = _datum.Value;
                        else
                            _db.Configuration.InsertOnSubmit(new Configuration
                            {
                                ConfigKey = _datum.Key,
                                ConfigValue = _datum.Value
                            });
                    }
                    _db.SubmitChanges();
                    GlobalConfiguration = GetConfiguration();
                    return true;
                }
            }
            catch (Exception _ex)
            {
                Log.Write("Errore nel salvataggio della configurazione - ", _ex); 
                return false;
            }
        }
    }
}