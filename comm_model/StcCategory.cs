using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class StcCategory
	{
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)
		public int CATE_LEVEL { get; set; }     //	int			카테고리 단계(1~4)
		public int CATE_PARENT { get; set; }        //	int			상위 카테고리 : SEQNO
		public string CATE_NAME_KR { get; set; }        //	VARCHAR(100)			
		public string CATE_NAME_CN { get; set; }        //	VARCHAR(100)			
		public string CATE_NAME_EN { get; set; }        //	VARCHAR(100)			
		public int STAT { get; set; }        //	TINYINT			1 : 사용 / 0 : 미사용
		public string REG_DT { get; set; }      //	DATETIME			등록일자

	}
}
