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
    public class PropertyOffService:IPropertyOffService
    {
        private readonly IRepository<PropertyOff> _propertyOffRepository;
        private readonly IRepository<PropertyOffPicture> _propertyPictureRepository;
        private readonly IRepository<PropertyOffFile> _propertyFileRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWorkContext _workContext;

        public PropertyOffService(IRepository<PropertyOff> propertyOffRepository,
                         IRepository<PropertyOffPicture> propertyPictureRepository, IRepository<PropertyOffFile> propertyFileRepository, 
                         IEventPublisher eventPublisher, IWorkContext workContext)
        {
            _propertyOffRepository = propertyOffRepository;
            _eventPublisher = eventPublisher;
            _workContext = workContext;
            _propertyPictureRepository = propertyPictureRepository;
            _propertyFileRepository = propertyFileRepository;
        }

        public IPagedList<QZCHY.Core.Domain.Properties.PropertyOff> GetAllOffRecords(IList<int> governmentIds, string checkState = "unchecked", string search = "", int pageIndex = 0,
            int pageSize = int.MaxValue, params PropertySortCondition[] sortConditions)
        {
            var query = _propertyOffRepository.Table.AsNoTracking();
            var currentUser = _workContext.CurrentAccountUser;

            //query = GetAllProperties(governmentId, includeChildren);

            Expression<Func<QZCHY.Core.Domain.Properties.PropertyOff, bool>> expression = p => !p.Deleted;

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
                        //  expression = expression.And(p => p.State == PropertyApproveState.DepartmentApprove || (p.State == PropertyApproveState.Start & p.SuggestGovernmentId == currentUser.Government.Id));
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
                        expression = expression.And(p => p.State != PropertyApproveState.Start);
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

            var propertiesOffRecords = new PagedList<QZCHY.Core.Domain.Properties.PropertyOff>(query, pageIndex, pageSize);
            return propertiesOffRecords;
        }
        public void DeletePropertyOff(PropertyOff p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            p.Deleted = true;
            UpdatePropertyOff(p);
        }

        public void InsertPropertyOff(PropertyOff p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            _propertyOffRepository.Insert(p);
            _eventPublisher.EntityInserted(p);
        }

        public void UpdatePropertyOff(PropertyOff p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            _propertyOffRepository.Update(p);
            _eventPublisher.EntityUpdated(p);
        }

        public PropertyOff GetPropertyOffById(int id)
        {
            var query = from c in _propertyOffRepository.Table
                        where c.Id == id
                        select c;
            var p = query.FirstOrDefault();
            return p;
        }

        public PropertyOff GetPropertyOffByPropertyId(int property_Id)
        {
            var query = from c in _propertyOffRepository.Table
                        where c.Property_Id == property_Id
                        select c;
            var p = query.FirstOrDefault();
            return p;
        }

        public IList<PropertyOff> GetOffsByPropertyId(int id)
        {
            var query = from c in _propertyOffRepository.Table
                        where c.Property.Id == id
                        select c;
            var offs = query.ToList();
            return offs;
        }

        #region 图片
        public void DeletePropertyOffPicture(PropertyOffPicture propertyOffPicture)
        {
            if (propertyOffPicture == null)
                throw new ArgumentNullException("propertyOffPicture");

            _propertyPictureRepository.Delete(propertyOffPicture);

            //event notification
            _eventPublisher.EntityDeleted(propertyOffPicture);
        }

        public IList<PropertyOffPicture> GetPropertyOffPicturesByPropertyId(int propertyOffId)
        {
            var query = from sp in _propertyPictureRepository.Table
                        where sp.PropertyOffId == propertyOffId
                        orderby sp.DisplayOrder
                        select sp;

            var propertyPictures = query.ToList();
            return propertyPictures;
        }

        public PropertyOffPicture GetPropertyOffPictureById(int propertyPictureId)
        {
            if (propertyPictureId == 0)
                return null;

            return _propertyPictureRepository.GetById(propertyPictureId);
        }

        public void InsertPropertyOffPicture(PropertyOffPicture propertyOffPicture)
        {
            if (propertyOffPicture == null)
                throw new ArgumentNullException("propertyOffPicture");

            _propertyPictureRepository.Insert(propertyOffPicture);

            //event notification
            _eventPublisher.EntityInserted(propertyOffPicture);
        }

        public void UpdatePropertyOffPicture(PropertyOffPicture propertyOffPicture)
        {
            if (propertyOffPicture == null)
                throw new ArgumentNullException("propertyOffPicture");

            _propertyPictureRepository.Update(propertyOffPicture);

            //event notification
            _eventPublisher.EntityUpdated(propertyOffPicture);
        }
        #endregion

        #region 文件
        public void DeletePropertyOffFile(PropertyOffFile propertyOffFile)
        {
            if (propertyOffFile == null)
                throw new ArgumentNullException("propertyOffFile");

            _propertyFileRepository.Delete(propertyOffFile);

            //event notification
            _eventPublisher.EntityDeleted(propertyOffFile);
        }

        public IList<PropertyOffFile> GetPropertyFilesByPropertyOffId(int propertyOffId)
        {
            var query = from sp in _propertyFileRepository.Table
                        where sp.PropertyOffId == propertyOffId
                        orderby sp.DisplayOrder
                        select sp;

            var propertyFiles = query.ToList();
            return propertyFiles;
        }

        public PropertyOffFile GetPropertyFileById(int propertyFileId)
        {
            if (propertyFileId == 0)
                return null;

            return _propertyFileRepository.GetById(propertyFileId);
        }

        public void InsertPropertyFile(PropertyOffFile propertyOffFile)
        {
            if (propertyOffFile == null)
                throw new ArgumentNullException("propertyOffFile");

            _propertyFileRepository.Insert(propertyOffFile);

            //event notification
            _eventPublisher.EntityInserted(propertyOffFile);
        }

        public void UpdatePropertyFile(PropertyOffFile propertyOffFile)
        {
            if (propertyOffFile == null)
                throw new ArgumentNullException("propertyOffFile");

            _propertyFileRepository.Update(propertyOffFile);

            //event notification
            _eventPublisher.EntityUpdated(propertyOffFile);
        }
        #endregion
    }
}
