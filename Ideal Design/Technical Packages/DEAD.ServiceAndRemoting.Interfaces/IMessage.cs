using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEAD.ServiceAndRemoting.Interfaces
{
	public interface IMessage
	{
		IDictionary<string, object> Headers { get; }
		dynamic HeadersBag { get; }
		Object Body { get; }					 
		Type BodyType { get; }
	}
}
