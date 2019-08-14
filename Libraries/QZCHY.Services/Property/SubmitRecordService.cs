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
    public class SubmitRecordService : ISubmitRecordService
    {

        private readonly IRepository<SubmitRecord> _submitRecordRepository;
        private readonly IEventPublisher _eventPublisher;

        public SubmitRecordService(IRepository<SubmitRecord> submitRecordRepository, IEventPublisher eventPublisher)
        {
            _submitRecordRepository = submitRecordRepository;
            _eventPublisher = eventPublisher;

        }

        public void DeleteSubmitRecord(SubmitRecord m)
        {
            if (m == null)
                throw new ArgumentNullException("property is null");

            m.Deleted = true;
            UpdateSubmitRecord(m);

        }

        public SubmitRecord GetSubmitRecordByGId(int id,string recordDate)
        {
            var query = from c in _submitRecordRepository.Table
                        where c.Goverment_ID == id&&c.RecordDate ==recordDate
                        select c;
            var p = query.FirstOrDefault();
            return p;
        }

        public void InsertSubmitRecord(SubmitRecord m)
        {
            if (m == null)
                throw new ArgumentNullException("property is null");

            _submitRecordRepository.Insert(m);
            _eventPublisher.EntityInserted(m);
        }

        public void UpdateSubmitRecord(SubmitRecord m)
        {
            if (m == null)
                throw new ArgumentNullException("property is null");

            _submitRecordRepository.Update(m);
            _eventPublisher.EntityUpdated(m);
        }
    }
}
