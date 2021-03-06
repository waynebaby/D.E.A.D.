﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMyBusinessSolution1.Models.Mapping
{
	public class UserConfiguration : EntityTypeConfiguration<User>
	{
		public UserConfiguration()
		{
			HasKey(x => x.Id);

			Property(x => x.Id)
				.HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

			Property(x => x.Name).HasMaxLength(50).IsUnicode().IsRequired();

			
			
			HasOptional(u => u.Group)
				.WithMany(g => g.Users)
				.HasForeignKey(u=>u.GroupId)
				.WillCascadeOnDelete(false);
		}

	}
}
