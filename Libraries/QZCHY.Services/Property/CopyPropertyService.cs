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
    public class CopyPropertyService : ICopyPropertyService
    {

        private readonly IRepository<CopyProperty> _copyPropertyRepository;
        private readonly IEventPublisher _eventPublisher;

        public CopyPropertyService(IRepository<CopyProperty> copyPropertyRepository, IEventPublisher eventPublisher)
        {
            _copyPropertyRepository = copyPropertyRepository;
            _eventPublisher = eventPublisher;
        }

        public void DeleteCopyProperty(CopyProperty p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            p.Deleted = true;
            UpdateCopyProperty(p);
        }

        public CopyProperty GetCopyPropertyById(int id)
        {
            var query = from c in _copyPropertyRepository.Table
                        where c.Id == id
                        select c;
            var p = query.FirstOrDefault();
            return p;
        }

        /// <summary>
        /// 获取最后一个未Published的备份数据
        /// </summary>
        /// <param name="property_id"></param>
        /// <returns></returns>
        public CopyProperty GetCopyPropertyByPropertyId(int property_id)
        {
            var query = from c in _copyPropertyRepository.Table
                        where c.Property_Id == property_id&&c.Published==false
                        select c;
            var p = query.FirstOrDefault();
            return p;
        }

        public void InsertCopyProperty(CopyProperty p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            _copyPropertyRepository.Insert(p);
            _eventPublisher.EntityInserted(p);
        }

        public void UpdateCopyProperty(CopyProperty p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            _copyPropertyRepository.Update(p);
            _eventPublisher.EntityUpdated(p);
        }
    }
}
