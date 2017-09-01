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
   public class PropertyLendService : IPropertyLendService
    {
        private readonly IRepository<PropertyLend> _propertyLendRepository;
        private readonly IRepository<PropertyLendPicture> _propertyPictureRepository;
        private readonly IRepository<PropertyLendFile> _propertyFileRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWorkContext _workContext;

        public PropertyLendService(IRepository<PropertyLend> propertyLendRepository,
                         IRepository<PropertyLendPicture> propertyPictureRepository, IRepository<PropertyLendFile> propertyFileRepository, 
                         IEventPublisher eventPublisher, IWorkContext workContext)
        {
            _propertyLendRepository = propertyLendRepository;
            _eventPublisher = eventPublisher;
            _workContext = workContext;
            _propertyPictureRepository = propertyPictureRepository;
            _propertyFileRepository = propertyFileRepository;
        }
        public virtual IPagedList<QZCHY.Core.Domain.Properties.PropertyLend> GetAllLendRecords(IList<int> governmentIds, string checkState = "unchecked", string search = "", int pageIndex = 0, int pageSize = int.MaxValue, params PropertySortCondition[] sortConditions)
        {
            var query = _propertyLendRepository.Table.AsNoTracking();
            var currentUser = _workContext.CurrentAccountUser;

            //query = GetAllProperties(governmentId, includeChildren);

            Expression<Func<QZCHY.Core.Domain.Properties.PropertyLend, bool>> expression = p => !p.Deleted;

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
                        expression = expression.And(p => p.State == PropertyApproveState.Finish || p.State == PropertyApproveState.AdminApprove);
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

            var propertiesLendRecords = new PagedList<QZCHY.Core.Domain.Properties.PropertyLend>(query, pageIndex, pageSize);
            return propertiesLendRecords;
        }

        public void DeletePropertyLend(PropertyLend p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            p.Deleted = true;
            UpdatePropertyLend(p);
        }

        public void InsertPropertyLend(PropertyLend p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            _propertyLendRepository.Insert(p);
            _eventPublisher.EntityInserted(p);
        }

        public void UpdatePropertyLend(PropertyLend p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            _propertyLendRepository.Update(p);
            _eventPublisher.EntityUpdated(p);
        }

        public PropertyLend GetPropertyLendById(int id)
        {
            var query = from c in _propertyLendRepository.Table
                        where c.Id == id
                        select c;
            var p = query.FirstOrDefault();
            return p;
        }

        public IList<PropertyLend> GetLendsByPropertyId(int id)
        {
            var query = from c in _propertyLendRepository.Table
                        where c.Property.Id == id
                        select c;
            var lends = query.ToList();
            return lends;
        }

        #region 图片
        public void DeletePropertyLendPicture(PropertyLendPicture propertyLendPicture)
        {
            if (propertyLendPicture == null)
                throw new ArgumentNullException("propertyLendPicture");

            _propertyPictureRepository.Delete(propertyLendPicture);

            //event notification
            _eventPublisher.EntityDeleted(propertyLendPicture);
        }

        public IList<PropertyLendPicture> GetPropertyLendPicturesByPropertyId(int propertyLendId)
        {
            var query = from sp in _propertyPictureRepository.Table
                        where sp.PropertyLendId == propertyLendId
                        orderby sp.DisplayOrder
                        select sp;

            var propertyPictures = query.ToList();
            return propertyPictures;
        }

        public PropertyLendPicture GetPropertyLendPictureById(int propertyPictureId)
        {
            if (propertyPictureId == 0)
                return null;

            return _propertyPictureRepository.GetById(propertyPictureId);
        }

        public void InsertPropertyLendPicture(PropertyLendPicture propertyLendPicture)
        {
            if (propertyLendPicture == null)
                throw new ArgumentNullException("propertyLendPicture");

            _propertyPictureRepository.Insert(propertyLendPicture);

            //event notification
            _eventPublisher.EntityInserted(propertyLendPicture);
        }

        public void UpdatePropertyLendPicture(PropertyLendPicture propertyLendPicture)
        {
            if (propertyLendPicture == null)
                throw new ArgumentNullException("propertyLendPicture");

            _propertyPictureRepository.Update(propertyLendPicture);

            //event notification
            _eventPublisher.EntityUpdated(propertyLendPicture);
        }
        #endregion

        #region 文件
        public void DeletePropertyLendFile(PropertyLendFile propertyLendFile)
        {
            if (propertyLendFile == null)
                throw new ArgumentNullException("propertyLendFile");

            _propertyFileRepository.Delete(propertyLendFile);

            //event notification
            _eventPublisher.EntityDeleted(propertyLendFile);
        }

        public IList<PropertyLendFile> GetPropertyFilesByPropertyLendId(int propertyLendId)
        {
            var query = from sp in _propertyFileRepository.Table
                        where sp.PropertyLendId == propertyLendId
                        orderby sp.DisplayOrder
                        select sp;

            var propertyFiles = query.ToList();
            return propertyFiles;
        }

        public PropertyLendFile GetPropertyFileById(int propertyFileId)
        {
            if (propertyFileId == 0)
                return null;

            return _propertyFileRepository.GetById(propertyFileId);
        }

        public void InsertPropertyFile(PropertyLendFile propertyLendFile)
        {
            if (propertyLendFile == null)
                throw new ArgumentNullException("propertyLendFile");

            _propertyFileRepository.Insert(propertyLendFile);

            //event notification
            _eventPublisher.EntityInserted(propertyLendFile);
        }

        public void UpdatePropertyFile(PropertyLendFile propertyLendFile)
        {
            if (propertyLendFile == null)
                throw new ArgumentNullException("propertyLendFile");

            _propertyFileRepository.Update(propertyLendFile);

            //event notification
            _eventPublisher.EntityUpdated(propertyLendFile);
        }
        #endregion
    }
}
