using System;
using System.IO;
using Akka.Configuration;

namespace Flip.Host
{
    public class ConfigLoader
    {
        public Config Load(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException(nameof(path));

            var hocon = File.ReadAllText(path);

            if (string.IsNullOrEmpty(hocon)) throw new ArgumentException(nameof(hocon));

            var config = ConfigurationFactory.ParseString(hocon);

            return config;
        }
    }
}