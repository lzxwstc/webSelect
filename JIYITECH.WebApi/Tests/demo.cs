using Dapper;
using JIYITECH.WebApi.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using JIYITECH.WebApi.Configs;

namespace JIYITECH.WebApi.Tests
{
    public static class Demo
    {
        static public string connectionString { get; set; }
        public static void TestDapper()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var a = $"select ur.*,r.*,u.* from sys_user_role ur left join sys_role r on ur.roleId = r.id left join sys_user u on ur.userId = u.id where ur.roleId = 2";
                List<UserRole> userRoles = db.Query<UserRole, Role, User, UserRole>(a, (userRole, role, user) =>
                {
                    userRole.role = role;
                    userRole.user = user;
                    return userRole;
                }, splitOn: "roleName,userName").AsList();
            }

        }
    }
}
