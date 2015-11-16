using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMyBusinessSolution1.Models.Mapping
{
	public class GroupConfiguration : EntityTypeConfiguration<Group>
	{
		public GroupConfiguration()
		{
			HasKey(x => x.Id);		//设定Key字段

			Property(x => x.Id)	  //对于这个字段 设定他的生成方式 这里使用自增
				.HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

			Property(x => x.Name).HasMaxLength(50).IsUnicode().IsRequired();  
		
		}

	}
}
