using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class BdNotice
	{

		//DB 필드
		
		public int SEQNO { get; set; } //	int(10) unsigned	PRI	auto_increment	일련번호 (자동증가)
		[MaxLength(5)]
		public string EST_CODE { get; set; }        //	varchar(5)			STATION 코드 ("00000" : ESM)
		
		public int WRITER_ID { get; set; }      //	int(11)			작성자 ID : ESM_USER 또는 EST_USER 의 SEQNO 

		[Required(ErrorMessage = "제목(CN) 값이 존재하지 않습니다.")]
		[MaxLength(50)]
		public string TITLE_CN { get; set; }        //	varchar(50)			제목(중국어)

		[Required(ErrorMessage = "제목(EN) 값이 존재하지 않습니다.")]
		[MaxLength(50)]
		public string TITLE_EN { get; set; }        //	varchar(50)			제목(영어)

		[Required(ErrorMessage = "제목(KR) 값이 존재하지 않습니다.")]
		[MaxLength(50)]
		public string TITLE_KR { get; set; }        //	varchar(50)			제목(한국어)

		[Required(ErrorMessage = "내용(CONTENTS_CN) 값이 존재하지 않습니다.")]
		[MaxLength(1000)]
		public string CONTENTS_CN { get; set; }     //	varchar(1000)			내용(중국어)

		[Required(ErrorMessage = "내용(CONTENTS_EN) 값이 존재하지 않습니다.")]
		[MaxLength(1000)]
		public string CONTENTS_EN { get; set; }     //	varchar(1000)			내용(영어)

		[Required(ErrorMessage = "내용(CONTENTS_KR) 값이 존재하지 않습니다.")]
		[MaxLength(1000)]
		public string CONTENTS_KR { get; set; }     //	varchar(1000)			내용(한국어)	
		public string REGDATE { get; set; }     //	datetime			등록일시
		public string UP_DATE { get; set; }     //	datetime			수정일시

		
		public int READ_NUM { get; set; }       //	int(11)			조회수
		
		public int POPUP_DISPLAY { get; set; }      //	smallint(5)			팝업여부(0 : 안보여줌, 1 : 보여줌)
		
		public string POPUP_START { get; set; }     //	char(8)			팝업시작날짜
		
		public string POPUP_END { get; set; }       //	char(8)			팝업종료날짜
		
		public int WEB_DISPLAY { get; set; }        //	smallint(5)			"0:모든사이트 보여줌, 1:업체사이트만, 2:CS사이트만수정  0 : ESM / 1 : EST / 2 : ESE 노출"
		[Required(ErrorMessage = "공지유형(BD_TYPE) 값이 존재하지 않습니다.")]
		public int BD_TYPE { get; set; }            //공지 유형


		//커스텀 필드
		public string BD_TYPE_txt { get; set; }				//공지 유형 문자열
		public string WRITER_NAME { get; set; }				//작성자명
		
	}

}
