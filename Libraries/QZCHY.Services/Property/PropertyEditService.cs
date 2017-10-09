using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QZCHY.Core.Domain.Properties;
using QZCHY.Core.Data;
using QZCHY.Services.Events;
using QZCHY.Core;
using System.Data.Entity;
using System.Linq.Expressions;
using QZCHY.Data;
using QZCHY.Services.AccountUsers;

namespace QZCHY.Services.Property
{
    public class PropertyEditService : IPropertyEditService
    {
        private readonly IRepository<PropertyEdit> _propertyEditRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWorkContext _workContext;

        public PropertyEditService(IRepository<PropertyEdit> propertyEditRepository, IEventPublisher eventPublisher, IWorkContext workContext)
        {
            _propertyEditRepository = propertyEditRepository;
            _eventPublisher = eventPublisher;
            _workContext = workContext;
        }

        public void DeletePropertyEdit(PropertyEdit p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            p.Deleted = true;
            UpdatePropertyEdit(p);
        }

        public PropertyEdit GetPropertyEditById(int id)
        {
            var query = from c in _propertyEditRepository.Table
                        where c.Id == id
                        select c;
            var p = query.FirstOrDefault();
            return p;
        }

        public IList<PropertyEdit> GetPropertyEditByPropertyId(int property_Id)
        {
            var query = from c in _propertyEditRepository.Table
                        where c.Property.Id == property_Id && !c.Deleted
                        select c;
            var p = query.ToList();
            return p;
        }

        public void InsertPropertyEdit(PropertyEdit p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            _propertyEditRepository.Insert(p);
            _eventPublisher.EntityInserted(p);
        }

        public void UpdatePropertyEdit(PropertyEdit p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            _propertyEditRepository.Update(p);
            _eventPublisher.EntityUpdated(p);
        }

        public virtual IPagedList<QZCHY.Core.Domain.Properties.PropertyEdit> GetAllEditRecords(IList<int> governmentIds, string checkState = "", string search = "", int pageIndex = 0, int pageSize = int.MaxValue, params PropertySortCondition[] sortConditions)
        {
            var query = _propertyEditRepository.Table.AsNoTracking();
            var currentUser = _workContext.CurrentAccountUser;

            //query = GetAllProperties(governmentId, includeChildren);

            Expression<Func<QZCHY.Core.Domain.Properties.PropertyEdit, bool>> expression = p => !p.Deleted;

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
                    if (currentUser.IsGovAuditor() || currentUser.IsStateOwnerAuditor())
                    {
                        expression = expression.And(p => p.State == PropertyApproveState.AdminApprove);
                    }
                    else if (currentUser.Government.ParentGovernmentId == 0)
                    {
                        expression = expression.And(p => p.State == PropertyApproveState.DepartmentApprove || (p.State == PropertyApproveState.Start & p.SuggestGovernmentId == currentUser.Government.Id));
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
                        expression = expression.And(p => p.State == PropertyApproveState.Finish || p.State == PropertyApproveState.AdminApprove);
                    }
                    else
                    {
                        expression = expression.And(p => p.State == PropertyApproveState.Finish);
                    }
                    break;
                case "all":
                    //expression = expression.And(p => p.State != PropertyApproveState.Start);
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

            var propertiesNewCreateRecords = new PagedList<QZCHY.Core.Domain.Properties.PropertyEdit>(query, pageIndex, pageSize);
            return propertiesNewCreateRecords;
        }



    }
}
