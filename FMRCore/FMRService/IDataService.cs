using System;
using System.Collections.Generic;
using System.Text;
using FMRCommon;

namespace FMRCore.FMRService
{
    public interface IDataService
    {
        List<Stock> GetStocks();
    }
}
