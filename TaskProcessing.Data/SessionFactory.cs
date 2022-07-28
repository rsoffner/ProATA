using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using System.Reflection;

namespace TaskProcessing.Data
{
    public class SessionFactory
    {
        private static ISessionFactory _factory;

        private static void Init(string connectionString)
        {
            FluentConfiguration configuration = Fluently.Configure()
                           .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                           .Mappings(m => m.FluentMappings
                                .AddFromAssembly(Assembly.Load("TaskProcessing.Data"))
                                .AddFromAssembly(Assembly.Load("TaskScheduling.Data"))
                           );

            _factory = configuration.BuildSessionFactory();
        }

        public static ISessionFactory GetSessionFactory(string connectionString)
        {
            Init(connectionString);

            return _factory;
        }

        public static ISession GetNewSession(string connectionString)
        {
            return GetSessionFactory(connectionString).OpenSession();
        }
    }

    public class GuidConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column("Id");
            instance.GeneratedBy.Guid();
        }
    }
}
