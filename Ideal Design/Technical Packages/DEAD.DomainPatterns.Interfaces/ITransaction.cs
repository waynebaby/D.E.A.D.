using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DomainPatterns
{
	public interface ITransaction	:IDisposable 
	{
		IUnitOfWork UnitOfWork { get; }
		void Commit();
		void Rollback();

	}
}
