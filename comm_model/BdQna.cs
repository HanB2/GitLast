using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class BdQna
	{
		
		public int SEQNO { get; set; }      //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)

		
		[MaxLength(5)]
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드

		
		[MaxLength(8)]
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드

		public int WRITER_ID { get; set; }      //	int unsigned			작성자 ID : ESE_USER 의 SEQNO 
		public string REGDATE { get; set; }     //	datetime			등록일시

		[Required(ErrorMessage = "문의 유형이 존재하지 않습니다.")]
		public int QNA_TYPE { get; set; }       //	TINYINT			문의 유형 1 : 일반 문의 / 2 : 충전 문의 등등

		[Required(ErrorMessage = "제목이 존재하지 않습니다.")]
		[MaxLength(100)]
		public string TITLE { get; set; }       //	varchar(100)			제목
		
		public string QUESTION { get; set; }        //	text			문의 내용
		public string ANSWER { get; set; }      //	text			답변 내용
		public int ANSWER_ID { get; set; }      //	int unsigned			답변자 ID : ESM_USER 의 SEQNO 
		public string ANSWER_DATE { get; set; }     //	datetime			답변일시


		//커스텀 필드
		public string QNA_TYPE_txt { get; set; }             //공지 유형 문자열
		public string WRITER_NAME { get; set; }             //작성자명

	}
}
