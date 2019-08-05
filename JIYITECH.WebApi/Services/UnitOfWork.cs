using JIYITECH.WebApi.Configs;
using JIYITECH.WebApi.Repositories;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Services
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IRolePermissionRepository RolePermissionRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IMenuRepository MenuRepository { get; }
        IDayDayUpRepository DayDayUpRepository { get; }
        IReportsRepository ReportsRepository { get; }
        void Commit();
        void Rollback();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection connection;
        private IDbTransaction transaction;
        private IUserRepository userRepository;
        private IRoleRepository roleRepository;
        private IRolePermissionRepository rolePermissionRepository;
        private IPermissionRepository permissionRepository;
        private IMenuRepository menuRepository;
        private IUserRoleRepository userRoleRepository;
        private IDayDayUpRepository dayDayUpRepository;
        private IReportsRepository reportsRepository;
        private bool disposed;

        public UnitOfWork(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
            transaction = connection.BeginTransaction();
        }
        public IUserRepository UserRepository
        {
            get
            {
                return userRepository ?? (userRepository = new UserRepository(transaction));
            }
        }

        public IDayDayUpRepository DayDayUpRepository
        {
            get
            {
                return dayDayUpRepository ?? (dayDayUpRepository = new DayDayUpRepository(transaction));
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                return roleRepository ?? (roleRepository = new RoleRepository(transaction));
            }
        }

        public IUserRoleRepository UserRoleRepository
        {
            get
            {
                return userRoleRepository ?? (userRoleRepository = new UserRoleRepository(transaction));
            }
        }

        public IRolePermissionRepository RolePermissionRepository
        {
            get
            {
                return rolePermissionRepository ?? (rolePermissionRepository = new RolePermissionRepository(transaction));
            }
        }

        public IPermissionRepository PermissionRepository
        {
            get
            {
                return permissionRepository ?? (permissionRepository = new PermissionRepository(transaction));
            }
        }


        public IMenuRepository MenuRepository
        {
            get
            {
                return menuRepository ?? (menuRepository = new MenuRepository(transaction));
            }
        }

        public IReportsRepository ReportsRepository
        {
            get
            {
                return reportsRepository ?? (reportsRepository = new ReportsRepository(transaction));
            }
        }

        public void Commit()
        {
            transaction.Commit();
            transaction.Dispose();
            transaction = connection.BeginTransaction();
            resetRepositories();
        }

        private void resetRepositories()
        {
            userRepository = null;
            roleRepository = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (transaction != null)
                    {
                        transaction.Dispose();
                        transaction = null;
                    }
                    if (connection != null)
                    {
                        connection.Dispose();
                        connection = null;
                    }
                }
                disposed = true;
            }
        }

        public void Rollback()
        {
            transaction.Rollback();
            transaction.Dispose();
            transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            resetRepositories();
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
