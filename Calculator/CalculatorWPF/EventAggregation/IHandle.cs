﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalculatorWPF.EventAggregation
{
    public interface IHandle<T>
    {
        void Handle(T message);
    }
}
