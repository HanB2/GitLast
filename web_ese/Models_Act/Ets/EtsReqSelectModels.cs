using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Ets
{
	public class EtsReqSelectModels : SeachModel
	{       //페이징
		public PagingModel Paging { get; set; }  // 입출고 정보

		//뷰
		public StcGoods Item { get; set; }  // 입출고 정보

		//리스트 
		public List<StcGoods> Items { get; set; }  // 입출고 정보

		//공지유형 셀렉트 박스 데이터 세팅
		public List<schTypeArray> schTypeArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "의류",        opt_value = "0"    },
			new schTypeArray {      opt_key = "기계부품",        opt_value = "1" },
			new schTypeArray {      opt_key = "가전제품",        opt_value = "2" }
		};

		//검색조건 셀렉트 박스 데이터 세팅
		public List<schTypeArray> schTypeTxtArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "번호",        opt_value = "SEQNO" },
			new schTypeArray {      opt_key = "바코드",        opt_value = "BARCODE" }
		};


		//모델 생성자
		public EtsReqSelectModels()
		{
			this.Paging = new PagingModel();
			this.Item = new StcGoods();
			this.Items = new List<StcGoods>();
		}
	}
}