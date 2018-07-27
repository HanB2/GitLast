using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class ConfExchangeRate
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)	
		public string CURRENCY_UNIT { get; set; }       //	char(3)			화폐단위	
		public double AMNT { get; set; }        //	double			환율	
		public string DATETIME_UPD { get; set; }        //	datetime			환율입력날짜	

	}
}
