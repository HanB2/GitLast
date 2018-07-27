using comm_model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Cs
{
    public class CsNoticeModels : SeachModel
	{

		//페이징
		public PagingModel Paging { get; set; }  

		//뷰
		public BdNotice Item { get; set; }  

		//리스트 
		public List<BdNotice> Items { get; set; }  

		//공지유형 셀렉트 박스 데이터 세팅
		public List<schTypeArray> schTypeArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "중요공지",        opt_value = "1"    },
			new schTypeArray {      opt_key = "일반공지",        opt_value = "2" },
			new schTypeArray {      opt_key = "업데이트",        opt_value = "3" }
		};

		//검색조건 셀렉트 박스 데이터 세팅
		public List<schTypeArray> schTypeTxtArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "번호",        opt_value = "SEQNO" },
			new schTypeArray {      opt_key = "제목",        opt_value = "TITLE_KR" },
			new schTypeArray {      opt_key = "작성자",        opt_value = "WRITER_ID" }
		};

		// 정렬부분 세팅
		public List<schTypeArray> sortKeyArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "SEQNO",        opt_value = "최신" },	
			new schTypeArray {      opt_key = "BD_TYPE",        opt_value = "공지유형" }
		};







		//모델 생성자
		public CsNoticeModels()
		{
			this.Paging = new PagingModel();
			this.Item = new BdNotice();
			this.Items = new List<BdNotice>();
			
		}

		//라디오 버튼으로 선택 용 모델(msh)
	
		public bool RadioBoxPop { get; set; }

	}


}