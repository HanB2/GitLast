using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comm_model
{
	public class EseGroupPermission
	{
		//DB필드
		public int SQNO { get; set; }           //	int(10) unsigned		SEQNO
		public int GROUP_ID { get; set; }       //	int(10) unsigned			그룹 ID	
		public string ESE_CODE { get; set; }        //	varchar(8)			SENDER 코드	
		public string MENU_ID { get; set; }     //	varchar(50)			메뉴 ID	
		public int PER_SELECT { get; set; }     //	tinyint(4)			select 권한(1=권한있음, 0=권한없음)	
		public int PER_INSERT { get; set; }     //	tinyint(4)			insert 권한(1=권한있음, 0=권한없음)	
		public int PER_UPDATE { get; set; }     //	tinyint(4)			update 권한(1=권한있음, 0=권한없음)	
		public int PER_DELETE { get; set; }     //	tinyint(4)			delete 권한(1=권한있음, 0=권한없음)	

		//커스텀 필드
		public string MENU_NAME { get; set; }     //	varchar(50)		메뉴 명
		public bool CHK_PER_SELECT { get; set; }     //	체크박스 사용을 위한 추가 필드
		public bool CHK_PER_INSERT { get; set; }     //	체크박스 사용을 위한 추가 필드
		public bool CHK_PER_UPDATE { get; set; }     //	체크박스 사용을 위한 추가 필드
		public bool CHK_PER_DELETE { get; set; }     //	체크박스 사용을 위한 추가 필드
	}
}
