﻿using System;
using System.Data;

namespace IOC.FW.Shared.Model.Repository
{
    [Serializable]
    public class ParameterData
    {
        public ParameterDirection Direction { get; set; }
        public DbType Type { get; set; }
        public int Size { get; set; }
        public string ParameterName { get; set; }
        public object Value { get; set; }
    }
}
