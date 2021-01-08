using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static Dapper.SqlMapper;

namespace DevTalk.Customers.Infrastructure.SqlMappings
{
    public class SqliteGuidTypeHandler : TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            parameter.Value = value.ToString().ToUpper();
        }

        public override Guid Parse(object value)
        {
            return new Guid((string)value);
        }
    }
}
