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
    public class PropertyAllotService : IPropertyAllotService
    {
        private readonly IRepository<PropertyAllot> _propertyAllotRepository;
        private readonly IRepository<PropertyAllotPicture> _propertyPictureRepository;
        private readonly IRepository<PropertyAllotFile> _propertyFileRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWorkContext _workContext;

        public PropertyAllotService(IRepository<PropertyAllot> propertyAllotRepository, IEventPublisher eventPublisher,
                         IRepository<PropertyAllotPicture> propertyPictureRepository, IRepository<PropertyAllotFile> propertyFileRepository,
            IWorkContext workContext)
        {
            _propertyAllotRepository = propertyAllotRepository;
            _eventPublisher = eventPublisher;
            _workContext = workContext;

            _propertyPictureRepository = propertyPictureRepository;
            _propertyFileRepository = propertyFileRepository;
        }

        public virtual IPagedList<QZCHY.Core.Domain.Properties.PropertyAllot> GetAllAllotRecords(IList<int> governmentIds, string checkState = "unchecked", string search = "", int pageIndex = 0, int pageSize = int.MaxValue, params PropertySortCondition[] sortConditions)
        {
            var query = _propertyAllotRepository.Table.AsNoTracking();
            var currentUser = _workContext.CurrentAccountUser;

            //query = GetAllProperties(governmentId, includeChildren);

            Expression<Func<QZCHY.Core.Domain.Properties.PropertyAllot, bool>> expression = p => !p.Deleted;

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
                        expression = expression.And(p => p.State!= PropertyApproveState.Start);
                    }
                    break;
                case "all":
                  //  expression = expression.And(p => p.State != PropertyApproveState.Start);
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

            var propertiesAllotRecords = new PagedList<QZCHY.Core.Domain.Properties.PropertyAllot>(query, pageIndex, pageSize);
            return propertiesAllotRecords;
        }

        public void DeletePropertyAllot(PropertyAllot p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            p.Deleted = true;
            UpdatePropertyAllot(p);
        }

        public PropertyAllot GetPropertyAllotById(int id)
        {
            var query = from c in _propertyAllotRepository.Table
                        where c.Id == id
                        select c;
            var p = query.FirstOrDefault();
            return p;
        }

        public void InsertPropertyAllot(PropertyAllot p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            _propertyAllotRepository.Insert(p);
            _eventPublisher.EntityInserted(p);
        }

        public void UpdatePropertyAllot(PropertyAllot p)
        {
            if (p == null)
                throw new ArgumentNullException("property is null");

            _propertyAllotRepository.Update(p);
            _eventPublisher.EntityUpdated(p);
        }

        public IList<PropertyAllot> GetAllotsByPropertyId(int id)
        {
            var query = from c in _propertyAllotRepository.Table
                        where c.Property.Id == id
                        select c;
            var allots = query.ToList();
            return allots;
        }

        #region 图片
        public void DeletePropertyAllotPicture(PropertyAllotPicture propertyAllotPicture)
        {
            if (propertyAllotPicture == null)
                throw new ArgumentNullException("propertyAllotPicture");

            _propertyPictureRepository.Delete(propertyAllotPicture);

            //event notification
            _eventPublisher.EntityDeleted(propertyAllotPicture);
        }

        public IList<PropertyAllotPicture> GetPropertyAllotPicturesByPropertyId(int propertyAllotId)
        {
            var query = from sp in _propertyPictureRepository.Table
                        where sp.PropertyAllotId == propertyAllotId
                        orderby sp.DisplayOrder
                        select sp;

            var propertyPictures = query.ToList();
            return propertyPictures;
        }

        public PropertyAllotPicture GetPropertyAllotPictureById(int propertyPictureId)
        {
            if (propertyPictureId == 0)
                return null;

            return _propertyPictureRepository.GetById(propertyPictureId);
        }

        public void InsertPropertyAllotPicture(PropertyAllotPicture propertyAllotPicture)
        {
            if (propertyAllotPicture == null)
                throw new ArgumentNullException("propertyAllotPicture");

            _propertyPictureRepository.Insert(propertyAllotPicture);

            //event notification
            _eventPublisher.EntityInserted(propertyAllotPicture);
        }

        public void UpdatePropertyAllotPicture(PropertyAllotPicture propertyAllotPicture)
        {
            if (propertyAllotPicture == null)
                throw new ArgumentNullException("propertyAllotPicture");

            _propertyPictureRepository.Update(propertyAllotPicture);

            //event notification
            _eventPublisher.EntityUpdated(propertyAllotPicture);
        }
        #endregion

        #region 文件
        public void DeletePropertyAllotFile(PropertyAllotFile propertyAllotFile)
        {
            if (propertyAllotFile == null)
                throw new ArgumentNullException("propertyAllotFile");

            _propertyFileRepository.Delete(propertyAllotFile);

            //event notification
            _eventPublisher.EntityDeleted(propertyAllotFile);
        }

        public IList<PropertyAllotFile> GetPropertyFilesByPropertyAllotId(int propertyAllotId)
        {
            var query = from sp in _propertyFileRepository.Table
                        where sp.PropertyAllotId == propertyAllotId
                        orderby sp.DisplayOrder
                        select sp;

            var propertyFiles = query.ToList();
            return propertyFiles;
        }

        public PropertyAllotFile GetPropertyFileById(int propertyFileId)
        {
            if (propertyFileId == 0)
                return null;

            return _propertyFileRepository.GetById(propertyFileId);
        }

        public void InsertPropertyFile(PropertyAllotFile propertyAllotFile)
        {
            if (propertyAllotFile == null)
                throw new ArgumentNullException("propertyAllotFile");

            _propertyFileRepository.Insert(propertyAllotFile);

            //event notification
            _eventPublisher.EntityInserted(propertyAllotFile);
        }

        public void UpdatePropertyFile(PropertyAllotFile propertyAllotFile)
        {
            if (propertyAllotFile == null)
                throw new ArgumentNullException("propertyAllotFile");

            _propertyFileRepository.Update(propertyAllotFile);

            //event notification
            _eventPublisher.EntityUpdated(propertyAllotFile);
        } 
        #endregion
    }
}
