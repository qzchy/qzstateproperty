using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QZCHY.Core.Domain.Properties;
using QZCHY.Core.Data;
using QZCHY.Services.Events;

namespace QZCHY.Services.Property
{
    public class MonthTotalService : IMonthTotalService
    {
        private readonly IRepository<MonthTotal> _monthTotalRepository;
        private readonly IEventPublisher _eventPublisher;

        public MonthTotalService(IRepository<MonthTotal> monthTotalRepository, IEventPublisher eventPublisher)
        {
            _monthTotalRepository = monthTotalRepository;
            _eventPublisher = eventPublisher;
        }

        public void DeleteMonthTotal(MonthTotal m)
        {
            if (m == null)
                throw new ArgumentNullException("property is null");

            m.Deleted = true;
            UpdateMonthTotal(m);
        }

        public MonthTotal GetMonthTotalByPId(int id)
        {
            var query = from c in _monthTotalRepository.Table
                        where c.Property_ID == id
                        select c;
            var p = query.FirstOrDefault();
            return p;
        }

        public void InsertMonthTotal(MonthTotal m)
        {
            if (m == null)
                throw new ArgumentNullException("property is null");

            _monthTotalRepository.Insert(m);
            _eventPublisher.EntityInserted(m);
        }

        public void UpdateMonthTotal(MonthTotal m)
        {
            if (m == null)
                throw new ArgumentNullException("property is null");

            _monthTotalRepository.Update(m);
            _eventPublisher.EntityUpdated(m);
        }

        public IList<MonthTotal> GetPropertyMonthTotal(int id,int month)
        {
            var query = from c in _monthTotalRepository.Table
                        where c.Property_ID == id && (c.Month.Month == month|| c.Month.Month+1==month ||c.Month.Month==12)
                        select c;
            return query.ToList();

            throw new NotImplementedException();
        }
    }
}
