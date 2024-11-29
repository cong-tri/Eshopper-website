﻿using System.ComponentModel;

namespace Eshopper_website.Models
{
	public class BaseModel
	{
		[DisplayName("Created Date")]
		public DateTime CreatedDate { get; set; } = DateTime.Now;

		[DisplayName("Created By")]
		public string? CreatedBy { get; set; } = string.Empty;

		[DisplayName("Updated Date")]
		public DateTime UpdatedDate { get; set; } = DateTime.Now;

		[DisplayName("Updated By")]
		public string? UpdatedBy { get; set; }
	}
}
