using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class StcInReqGoods
	{
		public int SEQNO { get; set; }			//일련번호 (자동증가)'	,
		public string REQ_SEQNO { get; set; }	//STC_IN_REQ 의 SEQNO'	,
		public string BARCODE { get; set; }		//바코드'	,
		public string BOXNUM { get; set; }		//박스번호'	,
		public int CNT { get; set; }			//상품 수량'
	}
}
