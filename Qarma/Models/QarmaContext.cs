using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Qarma.Models
{
	public class QarmaContext : DbContext
	{
		public QarmaContext() : base("name=QarmaContext")
		{
		}
	}
}