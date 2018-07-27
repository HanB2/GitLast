using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Cs
{
    public class CsQnaModels : SeachModel
	{

		//페이징
		public PagingModel Paging { get; set; }  // 입출고 정보

		//뷰
		public BdQna Item { get; set; }

		//리스트 
		public List<BdQna> Items { get; set; }

		//문의유형 셀렉트 박스 데이터 세팅 (msh)
		public IEnumerable<schTypeArray> schTypeArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "중요문의",        opt_value = "1" },
			new schTypeArray {      opt_key = "일반문의",        opt_value = "2" },
			new schTypeArray {      opt_key = "업데이트",        opt_value = "3" }
		};

		//분류 셀렉트 박스 데이터 세팅(msh)
		public IEnumerable<schTypeArray> clafictArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "SENDER",        opt_value = "1" },
			new schTypeArray {      opt_key = "STATION",        opt_value = "1" }
		};

		//분류 셀렉트 박스 데이터 세팅(msh)
		public IEnumerable<schTypeArray> statusArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "미답변",        opt_value = "1" },
			new schTypeArray {      opt_key = "답변",        opt_value = "2" },
		
		};

		//검색조건 셀렉트 박스 데이터 세팅
		public IEnumerable<schTypeArray> schTypeTxtArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "번호",        opt_value = "SEQNO" },
			new schTypeArray {      opt_key = "제목",        opt_value = "TITLE" },
			new schTypeArray {      opt_key = "작성자",        opt_value = "WRITER_ID" }
		};

		// 정렬부분 세팅
		public List<schTypeArray> sortKeyArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "SEQNO",        opt_value = "최신" },
			new schTypeArray {      opt_key = "TITLE",        opt_value = "제목" },
			new schTypeArray {      opt_key = "QNA_TYPE",        opt_value = "문의유형" }
		};


		//모델 생성자
		public CsQnaModels()
		{
			this.Paging = new PagingModel();
			this.Item = new BdQna();
			this.Items = new List<BdQna>();
		}

	}
}