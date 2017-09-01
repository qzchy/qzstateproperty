using QZCHY.Core;
using QZCHY.Core.Data;
using QZCHY.Core.Infrastructure;

namespace QZCHY.Data
{
    public class EfStartUpTask : IStartupTask
    {
        public void Execute()
        {
            var provider = EngineContext.Current.Resolve<IDataProvider>();
            if (provider == null)
                throw new QZCHYException("No IDataProvider found");
            provider.SetDatabaseInitializer();
        }

        public int Order
        {
            //ensure that this task is run first 
            get { return -1000; }
        }
    }
}
