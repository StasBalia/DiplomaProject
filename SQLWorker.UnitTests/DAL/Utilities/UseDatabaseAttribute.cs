using System.Reflection;
using Xunit.Sdk;

namespace SQLWorker.UnitTests.DAL.Utilities
{
    public class UseDatabaseAttribute : BeforeAfterTestAttribute
    {
        private readonly DatabaseFeeder _databaseFeeder;
        
        public UseDatabaseAttribute(string connectionString)
        {
            _databaseFeeder = new DatabaseFeeder(connectionString);
        }
        
        public override void Before(MethodInfo methodUnderTest)
        {
            _databaseFeeder.Up();
            base.Before(methodUnderTest);
        }
        
        public override void After(MethodInfo methodUnderTest)
        {
            _databaseFeeder.Down();
            base.After(methodUnderTest);
        }
    }
}