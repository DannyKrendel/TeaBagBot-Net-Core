using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using TeaBagBot.DataAccess;
using TeaBagBot.Models;

namespace TeaBagBot.Services
{
    public class SettingsService
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _path = @"C:\Users\Danny\Source\Repos\TeaBagBot\Config\AppSettings.json";

        public SettingsService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public AppSettings Load()
        {
            try
            {
                string json = _fileSystem.File.ReadAllText(_path);
                dynamic obj = JsonConvert.DeserializeObject(json);
                var settings = new AppSettings();
                settings.DiscordToken = obj.Discord.Token;
                settings.MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = obj.MongoDb.ConnectionString,
                    DatabaseName = obj.MongoDb.DatabaseName
                };
                return settings;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't load settings from '{_path}'.", ex);
            }
        }

        public IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(_path)
                .Build();
        }

        //public void Save(AppSettings settings)
        //{
        //    try
        //    {
        //        _fileSystem.Directory.CreateDirectory(_fileSystem.Path.GetDirectoryName(_path));
        //        var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
        //        _fileSystem.File.WriteAllText(_path, json);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Couldn't store settings to '{_path}'.", ex);
        //    }
        //}
    }
}
