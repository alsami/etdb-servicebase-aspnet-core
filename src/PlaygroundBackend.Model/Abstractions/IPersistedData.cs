﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaygroundBackend.Model.Abstractions
{
    public interface IPersistedData
    {
        int Id { get; set; }
    }
}