using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CIIC.HSR.TSP.ServiceBus.Messaging.SystemMessage
{


	public class Ping
	{


		public Guid Id { get; set; }


		public List<StampItem> Marks { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("ID:");
			sb.AppendLine(Id.ToString());
			if (Marks != null)
			{
				foreach (var item in Marks)
				{

					sb.Append(item.Time).Append(":\t").Append(item.Data).AppendLine();
				}
			}

			return sb.ToString();
		}

	}



}
