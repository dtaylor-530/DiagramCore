﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionCore
{
    public interface ITarget
    {
        void AddConnectorTip(IConnectionPoint connectionPoint);
    }


}
