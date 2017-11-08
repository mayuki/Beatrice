using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beatrice.Configuration
{
    public class FeatureDefinition
    {
        public string Feature { get; set; }
        public IConfigurationSection Options { get; set; }
    }
}
