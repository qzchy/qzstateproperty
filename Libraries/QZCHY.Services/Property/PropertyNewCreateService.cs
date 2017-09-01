using QZCHY.Core;
using QZCHY.Core.Data;
using QZCHY.Core.Domain.Properties;
using QZCHY.Data;
using QZCHY.Services.AccountUsers;
using QZCHY.Services.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.Property
{
   public class PropertyNewCreateService: IPropertyNewCreateService
    {
        private readonly IRepository<PropertyNewCreate> _propertyNewCreateRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWorkContext _workContext;

        public PropertyNewCreateService(IRepository<PropertyNewCreate> propertyNewCreateRepository, IEventPublisher eventPublisher,  IWorkContext workContext)
            {
              _propertyNewCreateRepository = propertyNewCreateRepository;
             _eventPublisher=eventPublisher;
            _workContext = workContext;
        }

        public virtual IPagedList<QZCHY.Core.Domain.Properties.PropertyNewCreate> GetAllNewCreateRecords(IList<int> governmentIds, string checkState = "", string search = "", int pageIndex = 0, int pageSize = int.MaxValue, params PropertySortCondition[] sortConditions)
        {
            var query = _propertyNewCreateRepository.Table.AsNoTracking();

            var currentUser = _workContext.CurrentAccountUser;

            //query = GetAllProperties(governmentId, includeChildren);

            Expression<Func<QZCHY.Core.Domain.Properties.PropertyNewCreate, bool>> expression = p => !p.Deleted;

            if (governmentIds != null && governmentIds.Count > 0)
            {
                expression = expression.And(p => governmentIds.Contains(p.SuggestGovernmentId));
            }

            //字符串查询
            if (!string.IsNullOrEmpty(search))
            {
                expression = expression.And(p => p.Title.Contains(search) || p.ASuggestion.Contains(search) || p.DSuggestion.Contains(search));
            }

            switch (checkState)
            {
                case "unchecked":
                    if (currentUser.IsGovAuditor()|| currentUser.IsStateOwnerAuditor())
                    {
                        expression = expression.And(p => p.State == PropertyApproveState.AdminApprove);
                    }
                    else if(currentUser.Government.ParentGovernmentId==0)
                    {
                        expression = expression.And(p => p.State == PropertyApproveState.DepartmentApprove);
                    }
                    else
                    {
                        expression = expression.And(p => p.State == PropertyApproveState.Start);
                    }
                    break;
                case "checked":
                    if (currentUser.IsGovAuditor() || currentUser.IsStateOwnerAuditor())
                    {
                        expression = expression.And(p => p.State == PropertyApproveState.Finish);
                    }
                    else if (currentUser.Government.ParentGovernmentId == 0)
                    {
                        expression = expression.And(p => p.State == PropertyApproveState.Finish || p.State == PropertyApproveState.DepartmentApprove);
                    }
                    else
                    {
                        expression = expression.And(p => p.State == PropertyApproveState.Finish);
                    }
                    break;
                case "all":
                   // expression = expression.And(p => p.State != PropertyApproveState.Start);
                    break;

            }

            query = query.Where(expression);

            if (sortConditions != null && sortConditions.Length != 0)
            {
                query = query.Sort(sortConditions);
            }
            else
            {
                query = query.Sort(new PropertySortCondition[1] {
                    new PropertySortCondition("DisplayOrder", System.ComponentModel.ListSortDirection.Ascending)
                });
            }

            var propertiesNewCreateRecords = new PagedList<QZCHY.Core.Domain.Properties.PropertyNewCreate>(query, pageIndex, pageSize);
            return propertiesNewCreateRecords;
        }

        public void DeletePropertyNewCreate(PropertyNewCreate p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            p.Deleted = true;
            UpdatePropertyNewCreate(p);
        }

        public void InsertPropertyNewCreate(PropertyNewCreate p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            _propertyNewCreateRepository.Insert(p);
            _eventPublisher.EntityInserted(p);
        }

        public void UpdatePropertyNewCreate(PropertyNewCreate p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            _propertyNewCreateRepository.Update(p);
            _eventPublisher.EntityUpdated(p);
        }

        public PropertyNewCreate GetPropertyNewCreateById(int id)
        {
            var query = from c in _propertyNewCreateRepository.Table
                        where c.Id == id
                        select c;
            var p = query.FirstOrDefault();
            return p;
        }

        public PropertyNewCreate GetPropertyNewCreateByPropertyId(int property_Id)
        {
            var query = from c in _propertyNewCreateRepository.Table
                        where c.Property_Id == property_Id
                        select c;
            var p = query.FirstOrDefault();
            return p;
        }
    }
}
