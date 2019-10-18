using TeaBagBot.Core.Entities;
using TeaBagBot.Core.Storage;
using System;

namespace TeaBagBot.Core
{
    public class ConfigService
    {
        private readonly IDataStorage _storage;

        public ConfigService(IDataStorage storage)
        {
            _storage = storage;
        }

        public TeaBagConfig LoadConfig()
        {
            try
            {
                return _storage.RestoreObject<TeaBagConfig>("Config");
            }
            catch (Exception ex)
            {
                throw new ConfigException($"Couldn't load config.", ex);
            }
        }

        public void SaveConfig(TeaBagConfig config)
        {
            try
            {
                _storage.StoreObject(config, "Config");
            }
            catch (Exception ex)
            {
                throw new ConfigException($"Couldn't save config.", ex);
            }
        }
    }
}
