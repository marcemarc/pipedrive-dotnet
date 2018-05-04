﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Pipedrive.CustomFields
{
    public class PersonField : IField
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        List<Email> Email { get; set; }

        [JsonProperty("phone")]
        List<Phone> Phone { get; set; }

        [JsonProperty("value")]
        public long Value { get; set; }
    }
}