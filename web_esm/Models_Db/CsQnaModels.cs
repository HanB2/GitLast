using comm_dbconn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_esm.Models_Db
{
	public class CsQnaModels : DatabaseConnection
	{
		string[] selectColumn =
		{
			"EST_CODE",
			"ESE_CODE",
			"WRITER_ID",
			"REGDATE",
			"QNA_TYPE",
			"TITLE",
			"QUESTION",
			"ANSWER",
			"ANSWER_ID",
			"ANSWER_DATE",

		};

	}
}