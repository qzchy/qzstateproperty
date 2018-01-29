using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.Property
{
    public interface IMonthTotalService
    {
        void DeleteMonthTotal(MonthTotal  m);

        void InsertMonthTotal(MonthTotal m);

        void UpdateMonthTotal(MonthTotal m);

        MonthTotal GetMonthTotalByPId(int id);

       IList <MonthTotal> GetPropertyMonthTotal(int id,int month);

    }
}
