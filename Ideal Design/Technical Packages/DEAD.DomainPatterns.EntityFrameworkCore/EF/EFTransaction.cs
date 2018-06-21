
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DEAD.DomainPatterns.EFCore
{
	public class EFTransaction : ITransaction, IDisposable
	{
		private EFUnitOfWork _uow;
		private IDbContextTransaction _trans;

		public EFTransaction(EFUnitOfWork uow)
		{
			_uow = uow;
			_trans = uow.Context.Database.BeginTransaction();
		}

		//public EFTransaction(EFUnitOfWork uow, System.Data.IsolationLevel level)
		//{
		//	_uow = uow;
		//	_trans = uow.Context.Database.BeginTransaction(level);
		//}


		public IUnitOfWork UnitOfWork
		{
			get { return _uow; }
		}

		public void Commit()
		{
			_trans.Commit();
		}

		public void Rollback()
		{
			_trans.Rollback();
		}

		public void Dispose()
		{
			_uow = null;
			_trans.Dispose();
		}
	}
}
