using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QZCHY.Core.Infrastructure;
using QZCHY.Core.Infrastructure.DependencyManagement;

namespace QZCHY.Web.Api.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order
        {
            get { return 2; }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {

            //TODO:
        }
    }
}