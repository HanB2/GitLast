using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EsmGroup
	{
		
		public int GROUP_ID { get; set; }       //	int(10) unsigned	PRI	그룹 ID (자동증가)

		[Required(ErrorMessage = "그룹명이 존재하지 않습니다.")]
		[MaxLength(20)]
		public string GROUP_NAME { get; set; }      //	varchar(20)	UNI	그룹명

	}
}
